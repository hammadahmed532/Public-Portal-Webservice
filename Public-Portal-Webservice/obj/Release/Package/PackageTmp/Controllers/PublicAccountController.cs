using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Public_Portal_Webservice.Models;
using Public_Portal_Webservice.Models.viewModel;

namespace Public_Portal_Webservice.Controllers
{
    [Authorize(Roles = "5")]
    public class PublicAccountController : Controller
    {
        PCP5Entities db = new PCP5Entities();

        public ActionResult AccountProfile()
        {

            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
           
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            vm.comps = db.Complaints.Where(b => b.account_id == vm.acc.Account_id);
            vm.supcomps = db.SupportingComplaints.Where(b => b.account_id == vm.acc.Account_id).ToList();
            vm.votes = db.Votings.Where(b => b.account_id == vm.acc.Account_id).ToList();
          
            vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                   .Where(b => b.is_Notified == false).ToList();


            return View(vm);

        }

        public ActionResult AccountProfileSecurity()
        {

            String username = System.Web.HttpContext.Current.User.Identity.Name;
            PCP5Entities db = new PCP5Entities();
            vmPCP vm = new vmPCP();
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                   .Where(b => b.is_Notified == false).ToList();

            return View(vm);

        }

        public ActionResult AccountProfileInfo()
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            PCP5Entities db = new PCP5Entities();
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            vm.towns = db.Towns.ToList();


            vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                   .Where(b => b.is_Notified == false).ToList();
            return View(vm);


        }




        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditAccountProfession(String ProfessionSelected)
        {

            String username = System.Web.HttpContext.Current.User.Identity.Name;

            using (PCP5Entities db = new PCP5Entities())
            {
                vmPCP vm = new vmPCP();
                ////////////////////////////
                vm.acc = db.Accounts.SingleOrDefault(a => a.username.Equals(username));
                vm.acc.profession = ProfessionSelected;

                /////////////////////////////

                db.Accounts.Attach(vm.acc);
                var entry = db.Entry(vm.acc);

                entry.Property(e => e.profession).IsModified = true;

                db.SaveChanges();
                ViewBag.educationChanged = "Education Changed";

            }
            vmPCP pc = new vmPCP();
            PCP5Entities db2 = new PCP5Entities();
            pc.acc = db2.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            pc.towns = db2.Towns.ToList();


            return View("AccountProfileInfo", pc);
        }
        [HttpPost]
        public ActionResult EditAccountPassword()
        {
            return View("AccountProfile");
        }
        public ActionResult EditAccountTown(String TownSelected)
        {

                int town_id_int = Convert.ToInt32(TownSelected);
                vmPCP vm = new vmPCP();

                String username = System.Web.HttpContext.Current.User.Identity.Name;
                PCP5Entities db = new PCP5Entities();
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.ucs = db.UCAreas.Where(d => d.TownID == town_id_int);
                Session["TownAccount"] = town_id_int;

                return View(vm);

         }


        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult EditAccountUC(String UCSelected)
        {

            int Town_id_int = Convert.ToInt32(Session["TownAccount"].ToString());
            int UC_id_int = Convert.ToInt32(UCSelected);
            String username = System.Web.HttpContext.Current.User.Identity.Name;

            using (PCP5Entities db = new PCP5Entities())
            {

                UCArea ucc = db.UCAreas.Where(d => d.ID == UC_id_int).FirstOrDefault();
                Account account2 = db.Accounts.SingleOrDefault(a => a.username.Equals(username));
                account2.UC_Area_id = UC_id_int;
                account2.Town_id = Town_id_int;

                var entry = db.Entry(account2);

                entry.Property(e => e.UC_Area_id).IsModified = true;
                entry.Property(e => e.Town_id).IsModified = true;

                db.SaveChanges();
                ViewBag.UCChanged = "Location Changed";
            }

            vmPCP pc = new vmPCP();

            PCP5Entities db2 = new PCP5Entities();
            pc.acc = db2.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            pc.towns = db2.Towns.ToList();


            return View("AccountProfileInfo", pc);
        }

    }
}