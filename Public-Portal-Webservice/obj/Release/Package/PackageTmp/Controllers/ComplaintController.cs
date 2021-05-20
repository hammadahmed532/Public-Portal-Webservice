using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Public_Portal_Webservice.Models;
using Public_Portal_Webservice.Models.viewModel;

namespace Public_Portal_Webservice.Controllers
{
    public class ComplaintController : Controller
    {
        private PCP5Entities db = new PCP5Entities();
        [NonAction]
        public vmPCP orderByRegisteredFirst(vmPCP vm)
        {
            vm.comps = vm.comps.OrderBy(a=>a.date_creation);
            return vm;
        }
        [NonAction]
        public vmPCP orderByRegisteredLast(vmPCP vm)
        {

            vm.comps = vm.comps.OrderByDescending(a => a.date_creation);
            return vm;
        }
        [NonAction]
        public vmPCP orderBySevereFirst(vmPCP vm)
        {
            /////////////////////////start//////////////////////////////

            List<complaintsSevere> severeList = new List<complaintsSevere>();

            int VoteCounter = 0, SuppCounter = 0, total = 0;
            foreach (var item in vm.comps)
            {
                VoteCounter = item.Votings.Count();
                SuppCounter = item.SupportingComplaints.Count();
                VoteCounter = VoteCounter * 2;
                SuppCounter = SuppCounter * 10;
                total = VoteCounter + SuppCounter;

                System.Diagnostics.Debug.WriteLine("total " + total);

                complaintsSevere compSevSingle = new complaintsSevere();
                compSevSingle.Complaint_id = item.complaint_Id;
                compSevSingle.SeverityCounter = total;
                severeList.Add(compSevSingle);
            }

            severeList = severeList.OrderByDescending(a => a.SeverityCounter).ToList();

            int count = 1;
            foreach (var i in severeList)
            {
                if (count == 1)
                {
                    vm.comps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                }
                else
                {
                    var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    vm.comps = vm.comps.Union(one);
                }
                count++;
            }
            ///////////////////////////end/////////////////////////

            return vm;
        }
        [NonAction]
        public vmPCP orderBySevereLast(vmPCP vm)
        {
            /////////////////////////start//////////////////////////////

            List<complaintsSevere> severeList = new List<complaintsSevere>();

            int VoteCounter = 0, SuppCounter = 0, total = 0;
            foreach (var item in vm.comps)
            {
                VoteCounter = item.Votings.Count();
                SuppCounter = item.SupportingComplaints.Count();
                VoteCounter = VoteCounter * 2;
                SuppCounter = SuppCounter * 10;
                total = VoteCounter + SuppCounter;

                System.Diagnostics.Debug.WriteLine("total " + total);

                complaintsSevere compSevSingle = new complaintsSevere();
                compSevSingle.Complaint_id = item.complaint_Id;
                compSevSingle.SeverityCounter = total;
                severeList.Add(compSevSingle);
            }

            severeList = severeList.OrderBy(a => a.SeverityCounter).ToList();

            int count = 1;
            foreach (var i in severeList)
            {
                if (count == 1)
                {
                    vm.comps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                }
                else
                {
                    var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    vm.comps = vm.comps.Union(one);
                }
                count++;
            }
            ///////////////////////////end/////////////////////////

            return vm;
        }
        public ActionResult Map(int? Town_id,int? UC_Id, int? category,int? Status,int? timePeriod)
        {

            try
            {
                vmPCP vm = new vmPCP();

                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                if (Town_id == 0 && category == null && Status == null && timePeriod == null)
                {
                    vm.comps = db.Complaints.ToList();
                    ViewBag.header = "All Town Complaints";
                    ViewBag.townId = 0;

                }
                else if (category == null && Status == null && timePeriod==null)
                {
                    vm.comps = db.Complaints.Where(a => a.UCArea.TownID == Town_id).ToList();
                    vm.town = db.Towns.Find(Town_id);
                    ViewBag.header = "All " + vm.town.town_name + " Complaints";
                    ViewBag.townId = vm.town.town_id;
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id).ToList();


                }
                else
                {
                    //getting town complaints or all
                    if (Town_id==0)
                    {
                        vm.comps = db.Complaints.ToList();
                        ViewBag.header = "All Town Complaints";
                        ViewBag.townId = 0;
                    }
                    else
                    {
                        vm.comps = db.Complaints.Where(a => a.UCArea.TownID == Town_id).ToList();
                        vm.town = db.Towns.Find(Town_id);
                        ViewBag.header = vm.town.town_name + " Town Complaints";
                        ViewBag.townId = vm.town.town_id;

                         ViewBag.TownName = vm.town.town_name;
                        vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id).ToList();
                    }
                    //checking UC
                    if (UC_Id == 0) {
                    }
                    else
                    {
                        vm.comps = vm.comps.Where(a => a.UC_area_id == UC_Id).ToList();
                        ViewBag.UCId = UC_Id;
                    }

                    //checking status of complaints
                    if (Status==0)
                    {

                    }else if (Status== 40)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("already resolved")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status== 50)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("budget")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status== 60)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("bogus")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else
                    {
                        vm.comps = vm.comps.Where(b => b.stage_id == Status).ToList();
                        ViewBag.stageId =Status;
                    }
                    //getting categories of complaints
                    if (category==0)
                    {

                    }
                    else
                    {
                        vm.comps = vm.comps.Where(a => a.category_id == category).ToList();
                         ViewBag.categoryId = category;
                    }

                    //getting time period of complaints
                    if (timePeriod==0)
                    {
                    }
                    else if (timePeriod==3)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-3)) >= 0);
                        ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 6)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-6)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 9)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-9)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 12)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-12)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 24)
                    {
                    vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-24)) >= 0); ViewBag.TimePeriodId = timePeriod;

                    }
                else if (timePeriod == 36)
                    {
                    vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-36)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                }
                vm.categories = db.Categories.ToList();
                vm.towns = db.Towns.ToList();

                ViewBag.count = vm.comps.Count();
                ViewBag.pageMaporTownorAnnouncements = "Map";

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }
                return View(vm);
            }
            catch (Exception nu)
            {
                System.Diagnostics.Debug.WriteLine(nu);
                return RedirectToAction("Index");
    }
}
        public ActionResult TownWiseAnnouncements(int? town_id,int?category,int?Status,int? timePeriod )
        {

            try
            {
                vmPCP vm = new vmPCP();
                ViewBag.pageMaporTownorAnnouncements = "Announcements";
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.annoucementsMultiple = db.Announcement1.Where(uc => uc.UCArea.TownID == town_id).ToList();              
                vm.ucs = db.UCAreas.Where(a => a.TownID == town_id).ToList();

                vm.categories = db.Categories.ToList();
                vm.town = db.Towns.Where(t => t.town_id == town_id).FirstOrDefault();
                if (town_id==0 &&category == null && Status == null && timePeriod == null)
                {
                    vm.annoucementsMultiple = db.Announcement1.ToList();
                    ViewBag.header = "All Announcements";
                    ViewBag.townId = 0;
                    vm.towns = db.Towns.ToList();
                    
                }
                else if (category == null && Status == null && timePeriod == null)
                {
                    vm.annoucementsMultiple = db.Announcement1.Where(a => a.UCArea.TownID == town_id).ToList();
                    vm.town = db.Towns.Find(town_id);
                    ViewBag.header = "All " + vm.town.town_name + " Announcements";
                    ViewBag.townId = vm.town.town_id;
                    vm.ucs = db.UCAreas.Where(a => a.TownID == town_id).ToList();
                    vm.towns = db.Towns.ToList();
        
                }
                else
                {
                    var townNameDb = db.Towns.Where(b => b.town_id == town_id).FirstOrDefault();
                    ViewBag.TownName = townNameDb.town_name;
                    ViewBag.count = vm.comps.Count();
                    ViewBag.header = "All " + townNameDb.town_name + " Announcements";
                    ViewBag.townId = town_id;

                    if (Status == 0)
                    {

                    }
                   else
                    {
                        vm.comps = vm.comps.Where(b => b.stage_id == Status).ToList();
                        ViewBag.stageId = Status;
                    }
                    //getting categories of complaints
                    if (category == 0)
                    {

                    }
                    else
                    {
                        vm.comps = vm.comps.Where(a => a.category_id == category).ToList();
                        ViewBag.categoryId = category;
                    }

                    //getting time period of complaints
                    if (timePeriod == 0)
                    {
                    }
                    else if (timePeriod == 3)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-3)) >= 0);
                        ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 6)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-6)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 9)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-9)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 12)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-12)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 24)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-24)) >= 0); ViewBag.TimePeriodId = timePeriod;

                    }
                    else if (timePeriod == 36)
                    {

                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-36)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                }


                

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }
                return View(vm);
            }
            catch (Exception nu)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult UCWiseAnnouncements(int? town_id,int? UC_id, int? category, int? Status, int? timePeriod)
        {
            try
            {
                vmPCP vm = new vmPCP();
                ViewBag.pageMaporTownorAnnouncements = "Announcements";
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.annoucementsMultiple = db.Announcement1.Where(uc => uc.UCArea.TownID == town_id).ToList();
                vm.ucs = db.UCAreas.Where(a => a.TownID == town_id).ToList();

                vm.categories = db.Categories.ToList();
                vm.town = db.Towns.Where(t => t.town_id == town_id).FirstOrDefault();
                ViewBag.UCId = UC_id;
                if (UC_id == 0 && category == null && Status == null && timePeriod == null)
                {
                    vm.annoucementsMultiple = db.Announcement1.ToList();
                    ViewBag.header = "All Announcements";
                    ViewBag.UCId = 0;
                    vm.towns = db.Towns.ToList();

                }
                else if (category == null && Status == null && timePeriod == null)
                {
                    vm.annoucementsMultiple = db.Announcement1.Where(a => a.UCArea.TownID == town_id).ToList();
                    vm.ucSingle = db.UCAreas.Find(UC_id);
                    ViewBag.header = "All " + vm.ucSingle.UCName + " Announcements";
                    //ViewBag.townId = vm.town.town_id;
                    vm.ucs = db.UCAreas.Where(a => a.TownID == town_id).ToList();
                    //var townNameDb = db.Towns.Where(b => b.town_id == town_id).FirstOrDefault();
                    //ViewBag.TownName = townNameDb.town_name;

                }
                else
                {
                    var townNameDb = db.Towns.Where(b => b.town_id == town_id).FirstOrDefault();
                    ViewBag.TownName = townNameDb.town_name;
                    ViewBag.count = vm.comps.Count();
                    vm.ucSingle = db.UCAreas.Find(UC_id);
                    ViewBag.header = "All " + vm.ucSingle.UCName + " Announcements";
                    ViewBag.townId = town_id;

                    if (Status == 0)
                    {

                    }
                    else
                    {
                        vm.comps = vm.comps.Where(b => b.stage_id == Status).ToList();
                        ViewBag.stageId = Status;
                    }
                    //getting categories of complaints
                    if (category == 0)
                    {

                    }
                    else
                    {
                        vm.comps = vm.comps.Where(a => a.category_id == category).ToList();
                        ViewBag.categoryId = category;
                    }

                    //getting time period of complaints
                    if (timePeriod == 0)
                    {
                    }
                    else if (timePeriod == 3)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-3)) >= 0);
                        ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 6)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-6)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 9)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-9)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 12)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-12)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 24)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-24)) >= 0); ViewBag.TimePeriodId = timePeriod;

                    }
                    else if (timePeriod == 36)
                    {

                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-36)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                }




                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }
                return View(vm);
            }
            catch (Exception nu)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Index()
        {
            vmPCP vm = new vmPCP();

            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            if (User.IsInRole("5"))
            {
                vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                       .Where(b => b.is_Notified == false).ToList();
            }
            vm.TotalComplaints = db.Complaints.Count();
            vm.totalResolvedComplaints = db.Complaints.Where(a => a.stage_id ==9).Count();
            vm.comps = db.Complaints.Where(a => a.stage_id == 9).OrderByDescending(b => b.date_last_modified).Take(4);
            vm.annoucementsMultiple = db.Announcement1.OrderByDescending(b => b.date_creation).Take(4);
            return View(vm);

        }



        [Authorize(Roles = "5")]
        [HttpGet]
        public ActionResult ViewMyComplaints(int? category, int? Status, int? timePeriod, int? OrderId)
        {
            try
            {
                vmPCP vm = new vmPCP();
                
                ViewBag.pageMaporTownorAnnouncements = "Mine";
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            

                vm.comps = db.Complaints.Where(a => a.account_id == vm.acc.Account_id).ToList();
                vm.categories = db.Categories.ToList();
                ViewBag.count = vm.comps.Count();
                ViewBag.header = "My Complaints";
                ViewBag.SevereType = "mine";

                if (category == null && Status == null && timePeriod == null && OrderId == null)
                {
                    vm = orderByRegisteredFirst(vm);

                }
                else
                {
                    if (Status == 0)
                    {

                    }
                    else if (Status == 40)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("already resolved")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status == 50)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("budget")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status == 60)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("bogus")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else
                    {
                        vm.comps = vm.comps.Where(b => b.stage_id == Status).ToList();
                        ViewBag.stageId = Status;
                    }
                    //getting categories of complaints
                    if (category == 0)
                    {

                    }
                    else
                    {
                        vm.comps = vm.comps.Where(a => a.category_id == category).ToList();
                        ViewBag.categoryId = category;
                    }

                    //getting time period of complaints
                    if (timePeriod == 0)
                    {
                    }
                    else if (timePeriod == 3)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-3)) >= 0);
                        ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 6)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-6)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 9)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-9)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 12)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-12)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 24)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-24)) >= 0); ViewBag.TimePeriodId = timePeriod;

                    }
                    else if (timePeriod == 36)
                    {

                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-36)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    //getting order of complaints
                    if (OrderId == 1)
                    {
                        vm = orderByRegisteredFirst(vm);
                        ViewBag.OrderById = OrderId;

                    }
                    else if (OrderId == 2)
                    {
                        vm = orderByRegisteredLast(vm);
                        ViewBag.OrderById = OrderId;
                    }
                    else if (OrderId == 3)
                    {
                        vm = orderBySevereFirst(vm);
                        ViewBag.OrderById = OrderId;
                    }
                    else if (OrderId == 4)
                    {
                        vm = orderBySevereLast(vm);
                        ViewBag.OrderById = OrderId;
                    }
                }

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }

                return View(vm);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "3")]
        [HttpGet]
        public ActionResult ViewMyAnnouncements(int? category, int? Status, int? timePeriod)
        {
            try
            {
                vmPCP vm = new vmPCP();

                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                }
                ViewBag.pageMaporTownorAnnouncements = "Mine";
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == vm.acc.UCArea.TownID).ToList();
                vm.severeComps = (from q in vm.severeComps
                                  orderby q.SupportingComplaints.Count() descending
                                  select q).Take(5);


                vm.comps = db.Complaints.Where(a => a.account_id == vm.acc.Account_id).ToList();
                vm.categories = db.Categories.ToList();
                ViewBag.count = vm.comps.Count();
                ViewBag.header = "My Announcements";
                ViewBag.SevereType = "mine";

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }

                return View(vm);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }


        [Authorize]
        public ActionResult MyAreaComplaints(int? category,int? Status,int? timePeriod,int? OrderId)
        {
            try
            {
                vmPCP vm = new vmPCP();
                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";
                    ViewBag.TownName = vm.town.town_name;


                }
                ViewBag.pageMaporTownorAnnouncements = "MyArea";
               String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == vm.acc.UCArea.TownID).ToList();
                vm.severeComps = (from q in vm.severeComps
                                  orderby q.SupportingComplaints.Count() descending
                                  select q).Take(5);
                if (User.IsInRole("1") || User.IsInRole("2"))
                {
                    vm.comps = db.Complaints.Where(uc => uc.UCArea.TownID == vm.acc.Town_id).ToList();
                }
                else
                {
                    vm.comps = db.Complaints.Where(uc => uc.UC_area_id == vm.acc.UC_Area_id).ToList();
                }

                if (category == null && Status == null && timePeriod == null && OrderId==null)
                {
                    vm = orderByRegisteredFirst(vm);

                }
                else
                {
                    if (Status == 0)
                    {

                    }
                    else if (Status == 40)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("already resolved")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status == 50)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("budget")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status == 60)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("bogus")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else
                    {
                        vm.comps = vm.comps.Where(b => b.stage_id == Status).ToList();
                        ViewBag.stageId = Status;
                    }
                    //getting categories of complaints
                    if (category == 0)
                    {

                    }
                    else
                    {
                        vm.comps = vm.comps.Where(a => a.category_id == category).ToList();
                        ViewBag.categoryId = category;
                    }

                    //getting time period of complaints
                    if (timePeriod == 0)
                    {
                    }
                    else if (timePeriod == 3)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-3)) >= 0);
                        ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 6)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-6)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 9)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-9)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 12)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-12)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 24)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-24)) >= 0); ViewBag.TimePeriodId = timePeriod;

                    }
                    else if (timePeriod == 36)
                    {

                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-36)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    } 
                    //getting order of complaints
                    if (OrderId == 1)
                    {
                        vm = orderByRegisteredFirst(vm);
                        ViewBag.OrderById = OrderId;

                    }
                    else if (OrderId == 2)
                    {
                        vm = orderByRegisteredLast(vm);
                        ViewBag.OrderById = OrderId;
                    }
                    else if (OrderId == 3)
                    {
                        vm = orderBySevereFirst(vm);
                        ViewBag.OrderById = OrderId;
                    }
                    else if (OrderId == 4)
                    {
                        vm = orderBySevereLast(vm);
                        ViewBag.OrderById = OrderId;
                    }
                }
                
                vm.categories = db.Categories.ToList();

                ViewBag.header = "My Area Complaints";
                ViewBag.SevereType = "myArea";

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }

                return View(vm);

            }
            catch (Exception nu)
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        public ActionResult MyAreaComplaintsCategory(String cat)
        {
            try
            {
                vmPCP vm = new vmPCP();
                int category_id_int = Convert.ToInt32(cat);
                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                }

                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == vm.acc.UCArea.TownID).ToList();
                vm.severeComps = (from q in vm.severeComps
                                  orderby q.SupportingComplaints.Count() descending
                                  select q).Take(5);

                vm.comps = db.Complaints.Where(uc => uc.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(c => c.category_id == category_id_int)
                                        .ToList();


                var townNameDb = db.Towns.Where(b => b.town_id == vm.acc.Town.town_id).FirstOrDefault();
                ViewBag.TownName = townNameDb.town_name;
                ViewBag.count = vm.comps.Count();
                ViewBag.categorySelected = db.Categories.Where(a => a.category_id == category_id_int).FirstOrDefault().Category1;
                vm.categories = db.Categories.ToList();

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }


                return View(vm);
            }
            catch (Exception er)
            {
                return RedirectToAction("Index");
            }
        }





        [Authorize(Roles = "5,3")]
        [HttpGet]
        public ActionResult RegisterNewComplaint()
        {
            vmPCP vm = new vmPCP();

            if (!string.IsNullOrEmpty(Session["TownId"] as string))
            {

                int Town_id_int = Convert.ToInt32(Session["TownId"]);
                vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                ViewBag.hasTownId = "yes";
                ViewBag.TownName = vm.town.town_name;

            }

            vm.categories = db.Categories.ToList();

            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            vm.comps = db.Complaints.Where(a => a.UC_area_id == vm.acc.UC_Area_id)
                                 .Where(b=>b.stage_id<=2 )   
                                .ToList();

            if (User.IsInRole("5"))
            {
                vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                       .Where(b => b.is_Notified == false).ToList();
            }

            return View(vm);
        }

        [Authorize(Roles = "5,3")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterNewComplaint(vmPCP model)
        {
            //try
            // {
            string fileName = string.Empty;
            string extension = string.Empty;


            using (PCP5Entities pcc = new PCP5Entities())
            {
                Account acc = new Account();
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
                Complaint ccc = new Complaint();

                ccc.c_title = model.compReg.c_title;
                ccc.description = model.compReg.description;
                ccc.category_id = model.compReg.category_id;
                ccc.date_creation = DateTime.Now;
                ccc.date_last_modified = DateTime.Now;
                ccc.account_id = acc.Account_id;
                ccc.UC_area_id = acc.UC_Area_id;
                if (model.compReg.map_lat == null)
                {
                    ccc.map_lat = 24.822262567785341;
                    ccc.map_long = 67.034111916304255;
                }
                else
                {
                    ccc.map_lat = model.compReg.map_lat;
                    ccc.map_long = model.compReg.map_long;
                }


                //adding image 1
                fileName = Path.GetFileNameWithoutExtension(model.compReg.imageFile1.FileName);
                extension = Path.GetExtension(model.compReg.imageFile1.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                ccc.image1 = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                model.compReg.imageFile1.SaveAs(fileName);

                fileName = string.Empty;
                extension = string.Empty;
                if (model.compReg.imageFile2 != null)
                {
                    //adding image 2
                    fileName = Path.GetFileNameWithoutExtension(model.compReg.imageFile2.FileName);
                    extension = Path.GetExtension(model.compReg.imageFile2.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    ccc.image2 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    model.compReg.imageFile2.SaveAs(fileName);

                    fileName = string.Empty;
                    extension = string.Empty;
                }
                if (model.compReg.imageFile3 != null)
                {
                    //adding image 3
                    fileName = Path.GetFileNameWithoutExtension(model.compReg.imageFile2.FileName);
                    extension = Path.GetExtension(model.compReg.imageFile2.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    ccc.image2 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    model.compReg.imageFile2.SaveAs(fileName);
                }
                if (User.IsInRole("3"))
                {
                    ccc.stage_id = 3;
                 }
                else
                {
                    ccc.stage_id = 1;
                }
                ccc.complaint_Type = "first";

                pcc.Complaints.Add(ccc);
                pcc.SaveChanges();
            }



            return RedirectToAction("ViewMyComplaints");
            //////////////////////
            ////}
            //catch(Exception er)
            //{
            //    vmPCP vm = new vmPCP();

            //    if (!string.IsNullOrEmpty(Session["TownId"] as string))
            //    {

            //        int Town_id_int = Convert.ToInt32(Session["TownId"]);
            //        vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
            //        vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
            //        ViewBag.hasTownId = "yes";
            //        ViewBag.TownName = vm.town.town_name;

            //       return View("Index");
            //    }


            //    vm.categories = db.Categories.ToList();


            //    String username = System.Web.HttpContext.Current.User.Identity.Name;
            //    vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            //    if (User.IsInRole("public"))
            //    {
            //        vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
            //                               .Where(b => b.is_Notified == false).ToList();
            //    }

            //    ViewBag.Message = "Enter Values for fields";

            //     return View("RegisterNewComplaint",vm);
            //}
        }




        public ActionResult ComplaintsAccountSupportingComplaint()
        {
            try
            {
                vmPCP vm = new vmPCP();

                // vm.town = db.Towns.Where(a => a.town_id == Town_id_int).FirstOrDefault();
                String username = System.Web.HttpContext.Current.User.Identity.Name;

                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.categories = db.Categories.ToList();
                ViewBag.count = vm.comps.Count();


                return View(vm);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }

        }
        public ActionResult SingleAnnouncement(int announcementId,String url)
        {
            vmPCP vm = new vmPCP();
            vm.singleAnnouncement = db.Announcement1.Find(announcementId);

            return View();
        }

        public ActionResult SingleComplaint(String Complaint_ID,String url)
        {
            try
            {

                int comp_id_int = Convert.ToInt32(Complaint_ID);

                String username = System.Web.HttpContext.Current.User.Identity.Name;

                Session["mId"] = Complaint_ID;
                vmPCP vm = new vmPCP();

                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                }
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
                vm.categories = db.Categories.ToList();
                vm.com = db.Complaints.Where(c => c.complaint_Id == comp_id_int).FirstOrDefault();
                vm.supcomps = db.SupportingComplaints.Where(s => s.Complaint_id == comp_id_int).ToList();
                vm.votes = db.Votings.Where(v => v.complaint_id == comp_id_int).ToList();

                ViewBag.TotalSupComps = vm.supcomps.Count();
                ViewBag.TotalVotes = vm.votes.Count();
                ViewBag.TownName = db.Towns.Where(c => c.town_id == vm.com.UCArea.TownID).FirstOrDefault().town_name;
                ViewBag.returnUrl = url;

                vm.status = db.Status.ToList();

                //checking if user can add vote
                if (Request.IsAuthenticated && User.IsInRole("5"))
                {
                    var voteCheck = db.Votings.Where(v => v.complaint_id == comp_id_int)
                       .Where(c => c.account_id.Equals(vm.acc.Account_id)).FirstOrDefault();

                    if (voteCheck == null)
                    {
                        ViewBag.canAddVote = "yes";
                    }
                    else if (vm.com.account_id.Equals(vm.acc.Account_id))
                    {

                    }
                    else if (voteCheck != null)
                    {
                        ViewBag.canAddVote = "alreadyAdded";
                    }
                }
                else
                {
                    //admin or not logged in
                }


                //checking if user already added subcomplaint or not

                if (Request.IsAuthenticated && User.IsInRole("5"))
                {
                    var supCheck = db.SupportingComplaints.Where(s => s.Complaint_id == comp_id_int)
                    .Where(b => b.account_id.Equals(vm.acc.Account_id)).FirstOrDefault();


                    if (vm.com.account_id.Equals(vm.acc.Account_id))
                    {
                        ViewBag.SupportingCompMessage = "Complaint registered By you";

                    }
                    else if (supCheck != null)
                    {
                        ViewBag.SupportingCompMessage = "Already added supporting complaint";

                    }
                    else
                    {
                        if (vm.com.UC_area_id.Equals(vm.acc.UC_Area_id))
                        {
                            ViewBag.SupportingCompMessage = "yes";
                        }
                        else
                        {
                            ViewBag.SupportingCompMessage = "Cannot add supporting complaint, complaint not in your UC area";
                        }
                    }
                }
                else if (!Request.IsAuthenticated)
                {
                    ViewBag.SupportingCompMessage = "Login to Register a supporting complaint";
                }
                else
                { //cannot add cuz admin
                }


                if (User.IsInRole("5"))
                {
                    var NotifiedChange = db.Complaints.Where(c => c.complaint_Id == comp_id_int)
                                                    .Where(a => a.account_id.Equals(vm.acc.Account_id));
                    if (NotifiedChange != null)
                    {

                        Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == comp_id_int).FirstOrDefault();
                        ccomp.is_Notified = true;

                        db.Complaints.Attach(ccomp);
                        var entry = db.Entry(ccomp);

                        entry.Property(e => e.is_Notified).IsModified = true;
                        db.SaveChanges();

                    }

                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                       .Where(b => b.is_Notified == false).ToList();

                }
                ViewBag.Status = TempData["Status"];
                return View(vm);
            }
            catch (Exception er)
            {
                return RedirectToAction("Index");
            }
        }


        [Authorize(Roles = "5")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SingleComplaint(vmPCP comm)
        {

            int comp_id_int = Convert.ToInt32(Session["mId"].ToString());

            try
            {
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                using (PCP5Entities pcc = new PCP5Entities())
                {
                    var Account_id = pcc.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    SupportingComplaint sup = new SupportingComplaint();
                    var chck = db.SupportingComplaints.Where(v => v.Complaint_id == comp_id_int)
                      .Where(b => b.account_id.Equals(Account_id)).FirstOrDefault();

                    if (chck == null)
                    {
                        sup.Description = comm.supcomp.Description;
                        sup.account_id = Account_id;
                        sup.Complaint_id = comp_id_int;
                        sup.time_created = DateTime.Now;

                        pcc.SupportingComplaints.Add(sup);
                        pcc.SaveChanges();

                    }

                }
                vmPCP vm = new vmPCP();

                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                }
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
                vm.categories = db.Categories.ToList();
                vm.com = db.Complaints.Where(c => c.complaint_Id == comp_id_int).FirstOrDefault();
                vm.supcomps = db.SupportingComplaints.Where(s => s.Complaint_id == comp_id_int).ToList();
                vm.votes = db.Votings.Where(v => v.complaint_id == comp_id_int).ToList();

                ViewBag.TotalSupComps = vm.supcomps.Count();
                ViewBag.TotalVotes = vm.votes.Count();
                ViewBag.TownName = db.Towns.Where(c => c.town_id == vm.com.UCArea.TownID).FirstOrDefault().town_name;



                //checking if user can add vote
                if (Request.IsAuthenticated && User.IsInRole("5"))
                {
                    var voteCheck = db.Votings.Where(v => v.complaint_id == comp_id_int)
                       .Where(c => c.account_id.Equals(vm.acc.Account_id)).FirstOrDefault();

                    if (voteCheck == null)
                    {
                        ViewBag.canAddVote = "yes";
                    }
                    else if (vm.com.account_id.Equals(vm.acc.Account_id))
                    {
                        //my complaint
                    }
                    else if (voteCheck != null)
                    {
                        ViewBag.canAddVote = "alreadyAdded";
                    }
                }
                else
                {
                    //admin or not logged in
                }


                //checking if user already added subcomplaint or not

                if (Request.IsAuthenticated && User.IsInRole("5"))
                {
                    var chck = db.SupportingComplaints.Where(v => v.Complaint_id == comp_id_int)
                    .Where(b => b.account_id.Equals(vm.acc.Account_id)).FirstOrDefault();

                    if (vm.com.account_id.Equals(vm.acc.Account_id))
                    {
                        ViewBag.SupportingCompMessage = "Complaint registered By you";
                    }
                    else if (chck != null)
                    {
                        ViewBag.SupportingCompMessage = "Already added supporting complaint";
                    }
                    else
                    {
                        if (vm.com.UC_area_id.Equals(vm.acc.UC_Area_id))
                        {
                            ViewBag.SupportingCompMessage = "yes";
                        }
                        else
                        {
                            ViewBag.SupportingCompMessage = "Cannot add supporting complaint, complaint not in your UC area";
                        }
                    }
                }
                else if (!Request.IsAuthenticated)
                {
                    ViewBag.SupportingCompMessage = "Login to Register a supporting complaint";
                }
                else
                {//cannot add cuz admin
                }
                TempData["Status"] = "Supporting Complaint Successfully Registered ";

                return RedirectToAction("SingleComplaint", new { Complaint_ID = comp_id_int });
            }
            catch (Exception er)
            {
                TempData["Status"] = "problem adding supporting complaint".ToString();

                return RedirectToAction("SingleComplaint", new { Complaint_ID = comp_id_int });
            }
        }




        [Authorize(Roles = "5")]
        public ActionResult addVote(int complaint_id)
        {
            try
            {
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                using (PCP5Entities pcc = new PCP5Entities())
                {
                    var Account_id = pcc.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;

                    Voting vote = new Voting();

                    var chck = db.Votings.Where(v => v.complaint_id == complaint_id)
                      .Where(b => b.account_id.Equals(Account_id)).FirstOrDefault();

                    if (chck == null)
                    {
                        vote.account_id = Account_id;
                        vote.complaint_id = complaint_id;
                        vote.time_created = DateTime.Now;

                        pcc.Votings.Add(vote);
                        pcc.SaveChanges();

                    }
                    TempData["Status"] = "Vote Added".ToString();

                }
                return RedirectToAction("SingleComplaint", new { Complaint_ID = complaint_id });

            }
            catch (Exception er)
            {
                TempData["Status"] = "problem adding Vote ".ToString();
                return RedirectToAction("SingleComplaint", new { Complaint_ID = complaint_id });
            }


        }

        [Authorize(Roles = "5")]
        public ActionResult DeleteVote(int complaint_id)
        {

            try
            {
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                using (PCP5Entities pcc = new PCP5Entities())
                {
                    var Account_id = pcc.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;

                    Voting vote = pcc.Votings.Where(a => a.account_id.Equals(Account_id))
                        .Where(c => c.complaint_id.Equals(complaint_id)).FirstOrDefault();


                    pcc.Votings.Remove(vote);
                    pcc.SaveChanges();

                }

                TempData["Status"] = "Vote removed".ToString();
                return RedirectToAction("SingleComplaint", new { Complaint_ID = complaint_id });

            }
            catch (Exception er)
            {
                TempData["Status"] = "problem removing Vote ".ToString();
                return RedirectToAction("SingleComplaint", new { Complaint_ID = complaint_id });
            }


        }









        public ActionResult CategoryComplaint(String cat)
        {
            try
            {
                vmPCP vm = new vmPCP();
                Session["category"] = cat;
                int cat_id = Convert.ToInt32(cat);

                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";
                    vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == Town_id_int).ToList();
                    vm.severeComps = (from q in vm.severeComps
                                      orderby q.SupportingComplaints.Count() descending
                                      select q).Take(5);


                }

                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
                vm.categories = db.Categories.ToList();
                vm.ucs = db.UCAreas.Where(uc => uc.TownID == vm.town.town_id).ToList();

                vm.comps = db.Complaints
                   .Where(c => c.Category.category_id.Equals(cat_id))
                    .Where(town => town.UCArea.TownID == vm.town.town_id).ToList();




                Category categoryselect = db.Categories.Where(c => c.category_id.Equals(cat_id)).FirstOrDefault();
                ViewBag.categorySelected = categoryselect.Category1.ToString();

                var townNameDb = db.Towns.Where(b => b.town_id == vm.town.town_id).FirstOrDefault();
                ViewBag.TownName = townNameDb.town_name;
                ViewBag.count = vm.comps.Count();
                ViewBag.header = categoryselect.Category1.ToString() + " Complaints of " + townNameDb.town_name;
                ViewBag.SevereType = "townCategory";

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }

                return View(vm);
            }
            catch (Exception nu)
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult SeverCheckAndRedirect(String page_type)
        {
            if (page_type.Equals("town"))
            {
                return RedirectToAction("SeverityLevelComplaint");
            }
            else if (page_type.Equals("townCategory"))
            {
                return RedirectToAction("SeverityLevelComplaintCategory");
            }
            else if (page_type.Equals("uc"))
            {

                return RedirectToAction("SeverityLevelComplaintUC");
            }
            else if (page_type.Equals("ucCategory"))
            {

                return RedirectToAction("SeverityLevelComplaintUCCategory");
            }
            else if (page_type.Equals("mine"))
            {

                return RedirectToAction("SeverityLevelofMyComplaints");
            }
            else if (page_type.Equals("myArea"))
            {

                return RedirectToAction("SeverityLevelComplaintMyArea");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public ActionResult TownWiseComplaint(int T_ID,int? category,int? Status,int? timePeriod,int? OrderId)
        {
            try
            {
                vmPCP vm = new vmPCP();
                Session["TownId"] = T_ID;
                
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
                ViewBag.pageMaporTownorAnnouncements = "Town";
                //int townId = Int32.Parse(T_ID.ToString());
                vm.comps = db.Complaints.Where(uc => uc.UCArea.TownID == T_ID).ToList();

                vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == T_ID).ToList();
                vm.severeComps = (from q in vm.severeComps
                                  orderby q.SupportingComplaints.Count() descending
                                  select q).Take(5);

                vm.ucs = db.UCAreas.Where(a => a.TownID == T_ID).ToList();

                vm.categories = db.Categories.ToList();
                vm.town = db.Towns.Where(t => t.town_id == T_ID).FirstOrDefault();

               if (category == null && Status == null && timePeriod == null && OrderId==null)
                {
                    vm.comps = db.Complaints.Where(a => a.UCArea.TownID == T_ID).ToList();
                    vm.town = db.Towns.Find(T_ID);
                    ViewBag.header = "Complaints of " + vm.town.town_name + " Town";
                    ViewBag.townId = vm.town.town_id;
                    vm.ucs = db.UCAreas.Where(a => a.TownID == T_ID).ToList();
                    vm = orderByRegisteredFirst(vm);

                }
                else
                {
                    if (Status == 0)
                    {

                    }
                    else if (Status == 40)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("already resolved")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status == 50)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("budget")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status == 60)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("bogus")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else
                    {
                        vm.comps = vm.comps.Where(b => b.stage_id == Status).ToList();
                        ViewBag.stageId = Status;
                    }
                    //getting categories of complaints
                    if (category == 0)
                    {

                    }
                    else
                    {
                        vm.comps = vm.comps.Where(a => a.category_id == category).ToList();
                        ViewBag.categoryId = category;
                    }

                    //getting time period of complaints
                    if (timePeriod == 0)
                    {
                    }
                    else if (timePeriod == 3)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-3)) >= 0);
                        ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 6)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-6)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 9)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-9)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 12)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-12)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 24)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-24)) >= 0); ViewBag.TimePeriodId = timePeriod;

                    }
                    else if (timePeriod == 36)
                    {

                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-36)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    //getting order of complaints
                  if(OrderId==1)
                    {
                        vm = orderByRegisteredFirst(vm);
                        ViewBag.OrderById = OrderId;

                    }
                    else if (OrderId==2)
                    {
                        vm = orderByRegisteredLast(vm);
                        ViewBag.OrderById = OrderId;
                    }
                    else if (OrderId == 3)
                    {
                        vm = orderBySevereFirst(vm);
                        ViewBag.OrderById = OrderId;
                    }
                    else if (OrderId == 4)
                    {
                        vm = orderBySevereLast(vm);
                        ViewBag.OrderById = OrderId;
                    }
                }
            

                var townNameDb = db.Towns.Where(b => b.town_id == T_ID).FirstOrDefault();
                ViewBag.TownName = townNameDb.town_name;
                ViewBag.count = vm.comps.Count();
                ViewBag.header = "Complaints of " + vm.town.town_name + " Town";
                ViewBag.SevereType = "town";
                ViewBag.townId = T_ID;


                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }
                return View(vm);
            }
            catch (Exception nu)
            {
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public ActionResult UCComplaint(int T_ID,int UC_Id, int? category, int? Status, int? timePeriod,int? OrderId)
        {
            try
            {
                vmPCP vm = new vmPCP();
                
                    int Town_id_int = T_ID;
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";
                ViewBag.pageMaporTownorAnnouncements = "Town";

                vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == Town_id_int).ToList();
                    vm.severeComps = (from q in vm.severeComps
                                      orderby q.SupportingComplaints.Count() descending
                                      select q).Take(5);

                    ViewBag.TownName = vm.town.town_name;

                
                vm.categories = db.Categories.ToList();
                vm.ucs = db.UCAreas.Where(uc => uc.TownID == vm.town.town_id).ToList();



                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
                
                vm.comps = db.Complaints.Where(a => a.UC_area_id == UC_Id).ToList();

                if (category == null && Status == null && timePeriod == null && OrderId == null)
                { 
                    vm.town = db.Towns.Find(T_ID);
                    ViewBag.header = "All " + vm.town.town_name + " Complaints";
                    ViewBag.townId = vm.town.town_id;
                    vm.ucs = db.UCAreas.Where(a => a.TownID == T_ID).ToList();
                    vm = orderByRegisteredFirst(vm);

                }
                else
                {
                    if (Status == 0)
                    {

                    }
                    else if (Status == 40)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("already resolved")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status == 50)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("budget")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else if (Status == 60)
                    {
                        vm.comps = vm.comps.Where(b => b.complaint_Type.Equals("bogus")).ToList();
                        ViewBag.stageId = Status;
                    }
                    else
                    {
                        vm.comps = vm.comps.Where(b => b.stage_id == Status).ToList();
                        ViewBag.stageId = Status;
                    }
                    //getting categories of complaints
                    if (category == 0)
                    {

                    }
                    else
                    {
                        vm.comps = vm.comps.Where(a => a.category_id == category).ToList();
                        ViewBag.categoryId = category;
                    }

                    //getting time period of complaints
                    if (timePeriod == 0)
                    {
                    }
                    else if (timePeriod == 3)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-3)) >= 0);
                        ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 6)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-6)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 9)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-9)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 12)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-12)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    }
                    else if (timePeriod == 24)
                    {
                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-24)) >= 0); ViewBag.TimePeriodId = timePeriod;

                    }
                    else if (timePeriod == 36)
                    {

                        vm.comps = vm.comps.Where(c => DateTime.Compare(c.date_creation, DateTime.Today.AddMonths(-36)) >= 0); ViewBag.TimePeriodId = timePeriod;
                    } 
                    //getting order of complaints
                    if (OrderId == 1)
                    {
                        vm = orderByRegisteredFirst(vm);
                        ViewBag.OrderById = OrderId;

                    }
                    else if (OrderId == 2)
                    {
                        vm = orderByRegisteredLast(vm);
                        ViewBag.OrderById = OrderId;
                    }
                    else if (OrderId == 3)
                    {
                        vm = orderBySevereFirst(vm);
                        ViewBag.OrderById = OrderId;
                    }
                    else if (OrderId == 4)
                    {
                        vm = orderBySevereLast(vm);
                        ViewBag.OrderById = OrderId;
                    }
                }

                UCArea ucs = db.UCAreas.Where(b => b.ID.Equals(UC_Id)).FirstOrDefault();
                ViewBag.UCSelected = ucs.UCName;
                ViewBag.count = vm.comps.Count();
                ViewBag.header = "Complaints of " + ucs.UCName.ToString() + " UC";
                ViewBag.SevereType = "uc";
                ViewBag.townId = T_ID;
                ViewBag.UCId = UC_Id;

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }


                return View(vm);
            }
            catch (Exception er)
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult UCComplaintCategory(String cat)
        {
            try
            {

                int cat_id = Convert.ToInt32(cat);
                Session["category"] = cat;

                String UC_id_String = Session["UCSelected"].ToString();
                int UC_id_int = Convert.ToInt32(UC_id_String);

                vmPCP vm = new vmPCP();

                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                    vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == Town_id_int).ToList();
                    vm.severeComps = (from q in vm.severeComps
                                      orderby q.SupportingComplaints.Count() descending
                                      select q).Take(5);


                }

                vm.categories = db.Categories.ToList();
                vm.ucs = db.UCAreas.Where(uc => uc.TownID == vm.town.town_id).ToList();



                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(a => a.UC_area_id == UC_id_int)
                                        .Where(d => d.category_id == cat_id)
                                        .ToList();

                UCArea ucs = db.UCAreas.Where(b => b.ID == UC_id_int).FirstOrDefault();
                ViewBag.UCSelected = ucs.UCName;

                Category categoryselect = db.Categories.Where(e => e.category_id == cat_id).FirstOrDefault();
                ViewBag.categorySelected = categoryselect.Category1.ToString();
                ViewBag.count = vm.comps.Count();
                ViewBag.header = categoryselect.Category1.ToString() + " Complaints of " + ucs.UCName.ToString() + " UC";
                ViewBag.SevereType = "ucCategory";

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }

                return View(vm);
            }
            catch (Exception er)
            {
                return RedirectToAction("Index");
            }
        }



        public ActionResult SeverityLevelComplaint()
        {
            try
            {
                vmPCP vm = new vmPCP();
                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                }

                vm.categories = db.Categories.ToList();

                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
                vm.comps = db.Complaints.Where(c => c.UCArea.TownID == vm.town.town_id).ToList();

                System.Diagnostics.Trace.WriteLine("You click me start.................." + DateTime.Now.ToString("hh.mm.ss.ffffff"));

                /////////////////////////start//////////////////////////////

                List<complaintsSevere> severeList = new List<complaintsSevere>();

                int VoteCounter = 0, SuppCounter = 0, total = 0;
                foreach (var item in vm.comps)
                {
                    VoteCounter = item.Votings.Count();
                    SuppCounter = item.SupportingComplaints.Count();
                    VoteCounter = VoteCounter * 2;
                    SuppCounter = SuppCounter * 10;
                    total = VoteCounter + SuppCounter;

                    System.Diagnostics.Debug.WriteLine("total " + total);

                    complaintsSevere compSevSingle = new complaintsSevere();
                    compSevSingle.Complaint_id = item.complaint_Id;
                    compSevSingle.SeverityCounter = total;
                    severeList.Add(compSevSingle);
                }

                severeList = severeList.OrderByDescending(a => a.SeverityCounter).ToList();

                int count = 1;
                foreach (var i in severeList)
                {
                    if (count == 1)
                    {
                        vm.comps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    }
                    else
                    {
                        var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        vm.comps = vm.comps.Union(one);
                    }
                    count++;
                }
                ///////////////////////////end/////////////////////////



                ViewBag.count = vm.comps.Count();
                ViewBag.header = "Severe Complaints of " + vm.town.town_name.ToString() + " Town";
                ViewBag.SevereType = "town";


                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }

                return View(vm);
            }
            catch (Exception er)
            {
                return RedirectToAction("Index");

            }
        }
        public ActionResult SeverityLevelComplaintUC()
        {
            try
            {
                vmPCP vm = new vmPCP();
                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                    vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == Town_id_int).ToList();



                    /////////////////////////start//////////////////////////////

                    List<complaintsSevere> severeList2 = new List<complaintsSevere>();

                    int VoteCounter2 = 0, SuppCounter2 = 0, total2 = 0;
                    foreach (var item in vm.severeComps)
                    {
                        VoteCounter2 = item.Votings.Count();
                        SuppCounter2 = item.SupportingComplaints.Count();
                        VoteCounter2 = VoteCounter2 * 2;
                        SuppCounter2 = SuppCounter2 * 10;
                        total2 = VoteCounter2 + SuppCounter2;
                        complaintsSevere compSevSingle = new complaintsSevere();
                        compSevSingle.Complaint_id = item.complaint_Id;
                        compSevSingle.SeverityCounter = total2;
                        severeList2.Add(compSevSingle);
                    }
                    severeList2 = severeList2.OrderByDescending(a => a.SeverityCounter).ToList();
                    int count2 = 1;
                    foreach (var i in severeList2)
                    {
                        if (count2 == 1)
                        {
                            vm.severeComps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        }
                        else
                        {
                            var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                            vm.severeComps = vm.severeComps.Union(one);
                        }
                        count2++;
                    }
                    /////////////////////////end///////////////////////////


                    vm.severeComps = vm.severeComps.Take(5);
                }
                vm.categories = db.Categories.ToList();

                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();



                int UC_id_int = Convert.ToInt32(Session["UCSelected"]);

                vm.comps = db.Complaints.Where(a => a.UC_area_id == UC_id_int).ToList();
                UCArea ucs = db.UCAreas.Where(b => b.ID.Equals(UC_id_int)).FirstOrDefault();
                ViewBag.UCSelected = ucs.UCName;

                ViewBag.header = "Severe Complaints of " + ucs.UCName.ToString() + " UC";
                ViewBag.SevereType = "uc";




                /////////////////////////start//////////////////////////////

                List<complaintsSevere> severeList = new List<complaintsSevere>();

                int VoteCounter = 0, SuppCounter = 0, total = 0;
                foreach (var item in vm.comps)
                {
                    VoteCounter = item.Votings.Count();
                    SuppCounter = item.SupportingComplaints.Count();
                    VoteCounter = VoteCounter * 2;
                    SuppCounter = SuppCounter * 10;
                    total = VoteCounter + SuppCounter;
                    complaintsSevere compSevSingle = new complaintsSevere();
                    compSevSingle.Complaint_id = item.complaint_Id;
                    compSevSingle.SeverityCounter = total;
                    severeList.Add(compSevSingle);
                }
                severeList = severeList.OrderByDescending(a => a.SeverityCounter).ToList();
                int count = 1;
                foreach (var i in severeList)
                {
                    if (count == 1)
                    {
                        vm.comps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    }
                    else
                    {
                        var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        vm.comps = vm.comps.Union(one);
                    }
                    count++;
                }
                ////////////////////////////end////////////////////////


                ViewBag.count = vm.comps.Count();

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }

                return View(vm);
            }
            catch (Exception err)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult SeverityLevelComplaintCategory()
        {
            try
            {

                vmPCP vm = new vmPCP();

                int category_id_int = Convert.ToInt32(Session["category"]);
                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                    vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == Town_id_int).ToList();
                    /////////////////////////start//////////////////////////////

                    List<complaintsSevere> severeList2 = new List<complaintsSevere>();

                    int VoteCounter2 = 0, SuppCounter2 = 0, total2 = 0;
                    foreach (var item in vm.severeComps)
                    {
                        VoteCounter2 = item.Votings.Count();
                        SuppCounter2 = item.SupportingComplaints.Count();
                        VoteCounter2 = VoteCounter2 * 2;
                        SuppCounter2 = SuppCounter2 * 10;
                        total2 = VoteCounter2 + SuppCounter2;
                        complaintsSevere compSevSingle = new complaintsSevere();
                        compSevSingle.Complaint_id = item.complaint_Id;
                        compSevSingle.SeverityCounter = total2;
                        severeList2.Add(compSevSingle);
                    }
                    severeList2 = severeList2.OrderByDescending(a => a.SeverityCounter).ToList();
                    int count2 = 1;
                    foreach (var i in severeList2)
                    {
                        if (count2 == 1)
                        {
                            vm.severeComps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        }
                        else
                        {
                            var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                            vm.severeComps = vm.severeComps.Union(one);
                        }
                        count2++;
                    }
                    //////////////////////////end//////////////////////////

                    vm.severeComps = vm.severeComps.Take(5);


                }
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
                vm.categories = db.Categories.ToList();
                vm.ucs = db.UCAreas.Where(uc => uc.TownID == vm.town.town_id).ToList();

                vm.comps = db.Complaints
                   .Where(c => c.Category.category_id.Equals(category_id_int))
                    .Where(town => town.UCArea.TownID == vm.town.town_id).ToList();


                Category categoryselect = db.Categories.Where(c => c.category_id.Equals(category_id_int)).FirstOrDefault();
                ViewBag.categorySelected = categoryselect.Category1.ToString();

                var townNameDb = db.Towns.Where(b => b.town_id == vm.town.town_id).FirstOrDefault();
                ViewBag.TownName = townNameDb.town_name;
                ViewBag.header = "Severe Complaints of " + townNameDb.town_name.ToString() + " Town";
                ViewBag.SevereType = "townCategory";



                //////////////////////////start/////////////////////////////

                List<complaintsSevere> severeList = new List<complaintsSevere>();

                int VoteCounter = 0, SuppCounter = 0, total = 0;
                foreach (var item in vm.comps)
                {
                    VoteCounter = item.Votings.Count();
                    SuppCounter = item.SupportingComplaints.Count();
                    VoteCounter = VoteCounter * 2;
                    SuppCounter = SuppCounter * 10;
                    total = VoteCounter + SuppCounter;
                    complaintsSevere compSevSingle = new complaintsSevere();
                    compSevSingle.Complaint_id = item.complaint_Id;
                    compSevSingle.SeverityCounter = total;
                    severeList.Add(compSevSingle);
                }
                severeList = severeList.OrderByDescending(a => a.SeverityCounter).ToList();
                int count = 1;
                foreach (var i in severeList)
                {
                    if (count == 1)
                    {
                        vm.comps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    }
                    else
                    {
                        var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        vm.comps = vm.comps.Union(one);
                    }
                    count++;
                }
                ///////////////////////////end/////////////////////////



                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }

                return View(vm);
            }
            catch (Exception err)
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult SeverityLevelComplaintUCCategory()
        {
            try
            {


                int cat_id = Convert.ToInt32(Session["category"]);
                vmPCP vm = new vmPCP();

                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                    vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == Town_id_int).ToList();
                    ////////////////////////start///////////////////////////////

                    List<complaintsSevere> severeList2 = new List<complaintsSevere>();

                    int VoteCounter2 = 0, SuppCounter2 = 0, total2 = 0;
                    foreach (var item in vm.severeComps)
                    {
                        VoteCounter2 = item.Votings.Count();
                        SuppCounter2 = item.SupportingComplaints.Count();
                        VoteCounter2 = VoteCounter2 * 2;
                        SuppCounter2 = SuppCounter2 * 10;
                        total2 = VoteCounter2 + SuppCounter2;
                        complaintsSevere compSevSingle = new complaintsSevere();
                        compSevSingle.Complaint_id = item.complaint_Id;
                        compSevSingle.SeverityCounter = total2;
                        severeList2.Add(compSevSingle);
                    }
                    severeList2 = severeList2.OrderByDescending(a => a.SeverityCounter).ToList();
                    int count2 = 1;
                    foreach (var i in severeList2)
                    {
                        if (count2 == 1)
                        {
                            vm.severeComps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        }
                        else
                        {
                            var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                            vm.severeComps = vm.severeComps.Union(one);
                        }
                        count2++;
                    }
                    //////////////////////end//////////////////////////////

                    vm.severeComps = vm.severeComps.Take(5);

                }

                String UC_id_String = Session["UCSelected"].ToString();
                int UC_id_int = Convert.ToInt32(UC_id_String);


                vm.categories = db.Categories.ToList();
                vm.ucs = db.UCAreas.Where(uc => uc.TownID == vm.town.town_id).ToList();


                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(a => a.UC_area_id == UC_id_int)
                                        .Where(d => d.category_id == cat_id)
                                        .ToList();

                UCArea ucs = db.UCAreas.Where(b => b.ID == UC_id_int).FirstOrDefault();
                ViewBag.UCSelected = ucs.UCName;

                Category categoryselect = db.Categories.Where(e => e.category_id == cat_id).FirstOrDefault();
                ViewBag.categorySelected = categoryselect.Category1.ToString();

                ViewBag.header = "Severe " + categoryselect.Category1.ToString() + " Complaints of " + ucs.UCName.ToString() + " UC";
                ViewBag.SevereType = "ucCategory";


                ////////////////////////start///////////////////////////////

                List<complaintsSevere> severeList = new List<complaintsSevere>();

                int VoteCounter = 0, SuppCounter = 0, total = 0;
                foreach (var item in vm.comps)
                {
                    VoteCounter = item.Votings.Count();
                    SuppCounter = item.SupportingComplaints.Count();
                    VoteCounter = VoteCounter * 2;
                    SuppCounter = SuppCounter * 10;
                    total = VoteCounter + SuppCounter;
                    complaintsSevere compSevSingle = new complaintsSevere();
                    compSevSingle.Complaint_id = item.complaint_Id;
                    compSevSingle.SeverityCounter = total;
                    severeList.Add(compSevSingle);
                }
                severeList = severeList.OrderByDescending(a => a.SeverityCounter).ToList();
                int count = 1;
                foreach (var i in severeList)
                {
                    if (count == 1)
                    {
                        vm.comps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    }
                    else
                    {
                        var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        vm.comps = vm.comps.Union(one);
                    }
                    count++;
                }
                /////////////////////////end///////////////////////////

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }

                return View(vm);
            }
            catch (Exception er)
            {
                return RedirectToAction("Index");
            }

        }

        [Authorize]
        public ActionResult SeverityLevelComplaintMyArea()
        {
            try
            {

                vmPCP vm = new vmPCP();

                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";
                    var townNameDb = db.Towns.Where(b => b.town_id == vm.town.town_id).FirstOrDefault();
                    ViewBag.TownName = townNameDb.town_name;

                }
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();


                vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == vm.acc.UCArea.TownID).ToList();
                ////////////////////////start///////////////////////////////

                List<complaintsSevere> severeList2 = new List<complaintsSevere>();

                int VoteCounter2 = 0, SuppCounter2 = 0, total2 = 0;
                foreach (var item in vm.severeComps)
                {
                    VoteCounter2 = item.Votings.Count();
                    SuppCounter2 = item.SupportingComplaints.Count();
                    VoteCounter2 = VoteCounter2 * 2;
                    SuppCounter2 = SuppCounter2 * 10;
                    total2 = VoteCounter2 + SuppCounter2;
                    complaintsSevere compSevSingle = new complaintsSevere();
                    compSevSingle.Complaint_id = item.complaint_Id;
                    compSevSingle.SeverityCounter = total2;
                    severeList2.Add(compSevSingle);
                }
                severeList2 = severeList2.OrderByDescending(a => a.SeverityCounter).ToList();
                int count2 = 1;
                foreach (var i in severeList2)
                {
                    if (count2 == 1)
                    {
                        vm.severeComps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    }
                    else
                    {
                        var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        vm.severeComps = vm.severeComps.Union(one);
                    }
                    count2++;
                }
                ///////////////////////end/////////////////////////////

                vm.severeComps = vm.severeComps.Take(5);

                vm.comps = db.Complaints.Where(uc => uc.UC_area_id == vm.acc.UC_Area_id).ToList();
                vm.categories = db.Categories.ToList();


                ViewBag.header = "Severe Complaints of My Area";
                ViewBag.SevereType = "myArea";


                ////////////////////////start///////////////////////////////

                List<complaintsSevere> severeList = new List<complaintsSevere>();

                int VoteCounter = 0, SuppCounter = 0, total = 0;
                foreach (var item in vm.comps)
                {
                    VoteCounter = item.Votings.Count();
                    SuppCounter = item.SupportingComplaints.Count();
                    VoteCounter = VoteCounter * 2;
                    SuppCounter = SuppCounter * 10;
                    total = VoteCounter + SuppCounter;
                    complaintsSevere compSevSingle = new complaintsSevere();
                    compSevSingle.Complaint_id = item.complaint_Id;
                    compSevSingle.SeverityCounter = total;
                    severeList.Add(compSevSingle);
                }
                severeList = severeList.OrderByDescending(a => a.SeverityCounter).ToList();
                int count = 1;
                foreach (var i in severeList)
                {
                    if (count == 1)
                    {
                        vm.comps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    }
                    else
                    {
                        var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        vm.comps = vm.comps.Union(one);
                    }
                    count++;
                }
                ///////////////////////////end/////////////////////////

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }


                return View(vm);
            }
            catch (Exception er)
            {
                return RedirectToAction("Index");
            }
        }




        [Authorize(Roles = "5")]
        public ActionResult SeverityLevelofMyComplaints()
        {
            try
            {
                vmPCP vm = new vmPCP();
                if (!string.IsNullOrEmpty(Session["TownId"] as string))
                {

                    int Town_id_int = Convert.ToInt32(Session["TownId"]);
                    vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                    vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                    ViewBag.hasTownId = "yes";

                }

                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == vm.acc.UCArea.TownID).ToList();
                //////////////////////start/////////////////////////////////

                List<complaintsSevere> severeList2 = new List<complaintsSevere>();

                int VoteCounter2 = 0, SuppCounter2 = 0, total2 = 0;
                foreach (var item in vm.severeComps)
                {
                    VoteCounter2 = item.Votings.Count();
                    SuppCounter2 = item.SupportingComplaints.Count();
                    VoteCounter2 = VoteCounter2 * 2;
                    SuppCounter2 = SuppCounter2 * 10;
                    total2 = VoteCounter2 + SuppCounter2;
                    complaintsSevere compSevSingle = new complaintsSevere();
                    compSevSingle.Complaint_id = item.complaint_Id;
                    compSevSingle.SeverityCounter = total2;
                    severeList2.Add(compSevSingle);
                }
                severeList2 = severeList2.OrderByDescending(a => a.SeverityCounter).ToList();
                int count2 = 1;
                foreach (var i in severeList2)
                {
                    if (count2 == 1)
                    {
                        vm.severeComps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    }
                    else
                    {
                        var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        vm.severeComps = vm.severeComps.Union(one);
                    }
                    count2++;
                }
                /////////////////////end///////////////////////////////

                vm.severeComps = vm.severeComps.Take(5);



                vm.comps = db.Complaints.Where(a => a.account_id == vm.acc.Account_id).ToList();
                vm.categories = db.Categories.ToList();

                ViewBag.header = "Severe My Complaints";
                ViewBag.SevereType = "mine";


                ////////////////////////start///////////////////////////////

                List<complaintsSevere> severeList = new List<complaintsSevere>();

                int VoteCounter = 0, SuppCounter = 0, total = 0;
                foreach (var item in vm.comps)
                {
                    VoteCounter = item.Votings.Count();
                    SuppCounter = item.SupportingComplaints.Count();
                    VoteCounter = VoteCounter * 2;
                    SuppCounter = SuppCounter * 10;
                    total = VoteCounter + SuppCounter;
                    complaintsSevere compSevSingle = new complaintsSevere();
                    compSevSingle.Complaint_id = item.complaint_Id;
                    compSevSingle.SeverityCounter = total;
                    severeList.Add(compSevSingle);
                }
                severeList = severeList.OrderByDescending(a => a.SeverityCounter).ToList();
                int count = 1;
                foreach (var i in severeList)
                {
                    if (count == 1)
                    {
                        vm.comps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    }
                    else
                    {
                        var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                        vm.comps = vm.comps.Union(one);
                    }
                    count++;
                }
                ///////////////////////end/////////////////////////////

                if (User.IsInRole("5"))
                {
                    vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .Where(b => b.is_Notified == false).ToList();
                }


                return View(vm);
            }
            catch (Exception er)
            {
                return RedirectToAction("Index");
            }

        }


        [HttpGet]
        public ActionResult TitleComplaint(vmPCP vm)
        {

            if (!string.IsNullOrEmpty(Session["TownId"] as string))
            {

                int Town_id_int = Convert.ToInt32(Session["TownId"]);
                vm.ucs = db.UCAreas.Where(a => a.TownID == Town_id_int).ToList();
                vm.town = db.Towns.Where(b => b.town_id == Town_id_int).FirstOrDefault();
                ViewBag.hasTownId = "yes";

            }

            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            vm.categories = db.Categories.ToList();


            vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == vm.town.town_id).ToList();
            //////////////////////start/////////////////////////////////

            List<complaintsSevere> severeList2 = new List<complaintsSevere>();

            int VoteCounter2 = 0, SuppCounter2 = 0, total2 = 0;
            foreach (var item in vm.severeComps)
            {
                VoteCounter2 = item.Votings.Count();
                SuppCounter2 = item.SupportingComplaints.Count();
                VoteCounter2 = VoteCounter2 * 2;
                SuppCounter2 = SuppCounter2 * 10;
                total2 = VoteCounter2 + SuppCounter2;
                complaintsSevere compSevSingle = new complaintsSevere();
                compSevSingle.Complaint_id = item.complaint_Id;
                compSevSingle.SeverityCounter = total2;
                severeList2.Add(compSevSingle);
            }
            severeList2 = severeList2.OrderByDescending(a => a.SeverityCounter).ToList();
            int count2 = 1;
            foreach (var i in severeList2)
            {
                if (count2 == 1)
                {
                    vm.severeComps = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                }
                else
                {
                    var one = db.Complaints.Where(a => a.complaint_Id == i.Complaint_id).ToList();
                    vm.severeComps = vm.severeComps.Union(one);
                }
                count2++;
            }
            vm.severeComps = vm.severeComps.Take(5);
            /////////////////////end///////////////////////////////



            vm.comps = db.Complaints
                .Where(town => town.UCArea.TownID == vm.town.town_id)
                .Where(a => a.c_title.Contains(vm.com.c_title)).ToList();
            ViewBag.Searchtitle = vm.com.c_title;

            if (User.IsInRole("5"))
            {
                
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
                vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                       .Where(b => b.is_Notified == false).ToList();
            }

            return View(vm);
        }



        public ActionResult OurMission()
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            if (User.IsInRole("5"))
            {
                vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                       .Where(b => b.is_Notified == false).ToList();
            }
            return View(vm);

        }


    }
}