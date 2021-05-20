using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Public_Portal_Webservice.Models;
using Public_Portal_Webservice.Models.viewModel;

namespace Public_Portal_Webservice.Controllers
{

    [Authorize(Roles = "2")]
    public class CHAccountController : Controller
    {

        PCP5Entities db = new PCP5Entities();

        public ActionResult CHPanelOverview()
        {

            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();


            ///general stats percentage counting
            float KarachiPublicAccounts = db.Accounts.Where(a => a.Role.Equals(5)).Count();
            float TownAccounts = db.Accounts.Where(a => a.Town_id == vm.acc.Town_id)
                                            .Where(b => b.Role.Equals(5))
                                            .Count();
            ViewBag.accountsPer = TownAccounts / KarachiPublicAccounts * 100;

            float KarachiComplaints = db.Complaints.Count();
            float TownComplaints = db.Complaints.Where(a => a.UCArea.TownID == vm.acc.Town_id).Count();
            ViewBag.ComplaintsPer = TownComplaints / KarachiComplaints * 100;

            float townall = db.Complaints.Where(a => a.UCArea.TownID == vm.acc.Town_id).Count();
            float Resolved = db.Complaints.Where(a => a.UCArea.TownID == vm.acc.Town_id)
                                            .Where(b => b.stage_id == 6)
                                            .Count();
            ViewBag.resolvedPer = Resolved / townall * 100;
            return View(vm);
        }


        public ActionResult ChManageAreaComplaints(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 1)
                                    .ToList();
            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();
            if ("all".Equals(cat))
            {
                ViewBag.cat = "all";
            }
            else
            {
                vm.comps = vm.comps.Where(v => v.Category.Category1.Equals(cat)).ToList();
                ViewBag.cat = cat;
            }

            if ("severe".Equals(orderby))
            {
                vm.comps = (from q in vm.comps
                            orderby q.SupportingComplaints.Count() descending
                            select q).ToList();
                ViewBag.status = "severe-first";
            }
            else if ("recent".Equals(orderby))
            {
                vm.comps = vm.comps.OrderByDescending(a => a.date_last_modified).ToList();
                ViewBag.status = "received-last";
            }
            else if ("received".Equals(orderby))
            {
                vm.comps = vm.comps.OrderBy(a => a.date_last_modified).ToList();
                ViewBag.status = "received-first";
            }
            vm.categories = db.Categories.ToList();


            ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 1).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 22).ToList().Count;
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 7).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 19).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 15).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 11).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 21).ToList().Count;

            ////////////

            if ((string)TempData["msg"] == "done")
            {
                TempData["msg"] = "Complaints forwarded";

            }
            else
            {
                TempData["msg"] = "";

            }

            return View(vm);

        }


        public void CHManageAreaComplaintsAction(List<ComplaintsWithRadioBtn> RadioArray)
        {
            var comps = RadioArray;
            int comp_id;
            String selected;


            foreach (var it in comps)
            {
                comp_id = it.complaint_Id;
                selected = it.SelectedRadio;
                if ("A".Equals(selected))
                {

                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 2;
                    ccomp.date_stage_2 = DateTime.Now;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_2).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }
                else if ("R".Equals(selected))
                {
                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 23;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }

            }
            //return RedirectToAction("ChManageAreaComplaints", new { orderby = "severe", cat = "all" });
        }

        public ActionResult CHViewReviewed(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 22)
                                    .ToList();
            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();
            if ("all".Equals(cat))
            {
                ViewBag.cat = "all";
            }
            else
            {
                vm.comps = vm.comps.Where(v => v.Category.Category1.Equals(cat)).ToList();
                ViewBag.cat = cat;
            }

            if ("severe".Equals(orderby))
            {
                vm.comps = (from q in vm.comps
                            orderby q.SupportingComplaints.Count() descending
                            select q).ToList();
                ViewBag.status = "severe-first";
            }
            else if ("recent".Equals(orderby))
            {
                vm.comps = vm.comps.OrderByDescending(a => a.date_last_modified).ToList();
                ViewBag.status = "received-last";
            }
            else if ("received".Equals(orderby))
            {
                vm.comps = vm.comps.OrderBy(a => a.date_last_modified).ToList();
                ViewBag.status = "received-first";
            }
            vm.categories = db.Categories.ToList();


            ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 1).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 22).ToList().Count;
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 7).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 19).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 15).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 11).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 21).ToList().Count;

            ////////////

            if ((string)TempData["msg"] == "done")
            {
                TempData["msg"] = "Complaints forwarded";

            }
            else
            {
                TempData["msg"] = "";

            }

            return View(vm);
        }
        public void CHViewReviewedAction(List<ComplaintsWithRadioBtn> RadioArray)
        {
            var comps = RadioArray;
            int comp_id;
            String selected;

            foreach (var it in comps)
            {
                comp_id = it.complaint_Id;
                selected = it.SelectedRadio;
                if ("A".Equals(selected))
                {

                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 23;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }
                else if ("R".Equals(selected))
                {
                    //    Complaint ccomp = db.Complaints.Find(comp_id);
                    //    ccomp.date_last_modified = DateTime.Now;
                    //    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    //    ccomp.is_Notified = false;
                    //    ccomp.stage_id = 22;
                    //    db.Complaints.Attach(ccomp);
                    //    var entry = db.Entry(ccomp);

                    //    entry.Property(e => e.date_last_modified).IsModified = true;
                    //    entry.Property(e => e.is_Notified).IsModified = true;
                    //    entry.Property(e => e.stage_id).IsModified = true;
                    //    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    //    db.SaveChanges();

                    //    Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                    //    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                    //    cdr.Remarks_Coordinator = it.Remarks;

                    //    db.Complaint_Det_Remarks.Attach(cdr);
                    //    var entry2 = db.Entry(cdr);
                    //    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    //    db.SaveChanges();

                    //    TempData["msg"] = "Complaint Forwarded ";
                }

            }
            //return RedirectToAction("CHViewReviewed",new { orderby="severe",cat="all"});
        }


        public ActionResult ResolvedComplaints(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 7)
                                    .ToList();

            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();

            if ("all".Equals(cat))
            {
                ViewBag.cat = "all";
            }
            else
            {
                vm.comps = vm.comps.Where(v => v.Category.Category1.Equals(cat)).ToList();
                ViewBag.cat = cat;
            }

            if ("severe".Equals(orderby))
            {
                vm.comps = (from q in vm.comps
                            orderby q.SupportingComplaints.Count() descending
                            select q).ToList();
                ViewBag.status = "severe-first";
            }
            else if ("recent".Equals(orderby))
            {
                vm.comps = vm.comps.OrderByDescending(a => a.date_last_modified).ToList();
                ViewBag.status = "received-last";
            }
            else if ("received".Equals(orderby))
            {
                vm.comps = vm.comps.OrderBy(a => a.date_last_modified).ToList();
                ViewBag.status = "received-first";
            }
            vm.categories = db.Categories.ToList();

            ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 1).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 22).ToList().Count;
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 7).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 19).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 15).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 11).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 21).ToList().Count;

            ////////////

            if ((string)TempData["msg"] == "done")
            {
                TempData["msg"] = "Complaints forwarded";

            }
            else
            {
                TempData["msg"] = "";

            }

            return View(vm);
        }
        public void ResolvedComplaintsAction(List<ComplaintsWithRadioBtn> RadioArray)
        {

            var comps = RadioArray;
            int comp_id;
            String selected;

            foreach (var it in comps)
            {
                comp_id = it.complaint_Id;
                selected = it.SelectedRadio;
                if ("A".Equals(selected))
                {

                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 8;
                    ccomp.date_stage_5 = DateTime.Now;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_5).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }
                else if ("R".Equals(selected))
                {
                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 23;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                    cdr.Remarks_Coordinator = it.Remarks;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }

            }
            //return RedirectToAction("ResolvedComplaints", new { orderby="severe",cat="all"});
        }
        public ActionResult CHViewBudgedted(String orderby, String cat)
        {

            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                  .Where(c => c.stage_id == 19)
                                    .ToList();
            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();

            if ("all".Equals(cat))
            {
                ViewBag.cat = "all";
            }
            else
            {
                vm.comps = vm.comps.Where(v => v.Category.Category1.Equals(cat)).ToList();
                ViewBag.cat = cat;
            }

            if ("severe".Equals(orderby))
            {
                vm.comps = (from q in vm.comps
                            orderby q.SupportingComplaints.Count() descending
                            select q).ToList();
                ViewBag.status = "severe-first";
            }
            else if ("recent".Equals(orderby))
            {
                vm.comps = vm.comps.OrderByDescending(a => a.date_last_modified).ToList();
                ViewBag.status = "received-last";
            }
            else if ("received".Equals(orderby))
            {
                vm.comps = vm.comps.OrderBy(a => a.date_last_modified).ToList();
                ViewBag.status = "received-first";
            }
            vm.categories = db.Categories.ToList();

            ViewBag.msg = TempData["msg"];


            ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 1).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 22).ToList().Count;
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 7).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 19).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 15).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 11).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 21).ToList().Count;

            ////////////
            if ((string)TempData["msg"] == "done")
            {
                TempData["msg"] = "Complaints forwarded";

            }
            else
            {
                TempData["msg"] = "";

            }
            return View(vm);

        }
        public void CHViewBudgedtedAction(List<ComplaintsWithRadioBtn> RadioArray)
        {

            var comps = RadioArray;
            int comp_id;
            String selected;
            foreach (var it in comps)
            {
                comp_id = it.complaint_Id;
                selected = it.SelectedRadio;
                if ("A".Equals(selected))
                {

                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 20;
                    ccomp.date_stage_5 = DateTime.Now;

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_5).IsModified = true;

                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }
                else if ("R".Equals(selected))
                {
                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 24;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                    cdr.Remarks_Coordinator = it.Remarks;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }

            }
            //return RedirectToAction("CHViewBudgedted", new { orderby = "severe", cat = "all" });
        }

        public ActionResult CHViewRejected(String orderby, String cat)
        {

            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 15)
                                    .ToList();
            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();

            if ("all".Equals(cat))
            {
                ViewBag.cat = "all";
            }
            else
            {
                vm.comps = vm.comps.Where(v => v.Category.Category1.Equals(cat)).ToList();
                ViewBag.cat = cat;
            }

            if ("severe".Equals(orderby))
            {
                vm.comps = (from q in vm.comps
                            orderby q.SupportingComplaints.Count() descending
                            select q).ToList();
                ViewBag.status = "severe-first";
            }
            else if ("recent".Equals(orderby))
            {
                vm.comps = vm.comps.OrderByDescending(a => a.date_last_modified).ToList();
                ViewBag.status = "received-last";
            }
            else if ("received".Equals(orderby))
            {
                vm.comps = vm.comps.OrderBy(a => a.date_last_modified).ToList();
                ViewBag.status = "received-first";
            }
            vm.categories = db.Categories.ToList();



            ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 1).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 22).ToList().Count;
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 7).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 19).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 15).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 11).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 21).ToList().Count;

            ////////////
            if ((string)TempData["msg"] == "done")
            {
                TempData["msg"] = "Complaints forwarded";

            }
            else
            {
                TempData["msg"] = "";

            }
            return View(vm);
        }
        public void CHViewRejectedAction(List<ComplaintsWithRadioBtn> RadioArray)
        {

            var comps = RadioArray;
            int comp_id;
            String selected;
            foreach (var it in comps)
            {
                comp_id = it.complaint_Id;
                selected = it.SelectedRadio;
                if ("A".Equals(selected))
                {

                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 16;
                    ccomp.date_stage_5 = DateTime.Now;

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_5).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }
                else if ("R".Equals(selected))
                {
                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 23;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                    cdr.Remarks_Coordinator = it.Remarks;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }

            }
            //return RedirectToAction("CHViewRejected", new { orderby = "severe", cat = "all" });
        }

        public ActionResult CHViewAlreadyResolved(String orderby, String cat)
        {

            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                      .Where(c => c.stage_id == 11)
                                    .ToList();
            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();

            if ("all".Equals(cat))
            {
                ViewBag.cat = "all";
            }
            else
            {
                vm.comps = vm.comps.Where(v => v.Category.Category1.Equals(cat)).ToList();
                ViewBag.cat = cat;
            }

            if ("severe".Equals(orderby))
            {
                vm.comps = (from q in vm.comps
                            orderby q.SupportingComplaints.Count() descending
                            select q).ToList();
                ViewBag.status = "severe-first";
            }
            else if ("recent".Equals(orderby))
            {
                vm.comps = vm.comps.OrderByDescending(a => a.date_last_modified).ToList();
                ViewBag.status = "received-last";
            }
            else if ("received".Equals(orderby))
            {
                vm.comps = vm.comps.OrderBy(a => a.date_last_modified).ToList();
                ViewBag.status = "received-first";
            }
            vm.categories = db.Categories.ToList();


            ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 1).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 22).ToList().Count;
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 7).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 19).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 15).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 11).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 21).ToList().Count;

            ////////////

            if ((string)TempData["msg"] == "done")
            {
                TempData["msg"] = "Complaints forwarded";

            }
            else
            {
                TempData["msg"] = "";

            }

            return View(vm);
        }
        public void CHViewAlreadyResolvedAction(List<ComplaintsWithRadioBtn> RadioArray)
        {

            var comps = RadioArray;
            int comp_id;
            String selected;
            foreach (var it in comps)
            {
                comp_id = it.complaint_Id;
                selected = it.SelectedRadio;
                if ("A".Equals(selected))
                {

                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 12;
                    ccomp.date_stage_5 = DateTime.Now;

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_5).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }
                else if ("R".Equals(selected))
                {
                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 23;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                    cdr.Remarks_Coordinator = it.Remarks;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }

            }
            //return RedirectToAction("CHViewAlreadyResolved", new { orderby = "severe", cat = "all" });
        }
        public ActionResult BudgetBasket(String orderby, String cat)
        {

            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 21)
                                    .ToList();
            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();
            if ("all".Equals(cat))
            {
                ViewBag.cat = "all";
            }
            else
            {
                vm.comps = vm.comps.Where(v => v.Category.Category1.Equals(cat)).ToList();
                ViewBag.cat = cat;
            }

            if ("severe".Equals(orderby))
            {
                vm.comps = (from q in vm.comps
                            orderby q.SupportingComplaints.Count() descending
                            select q).ToList();
                ViewBag.status = "severe-first";
            }
            else if ("recent".Equals(orderby))
            {
                vm.comps = vm.comps.OrderByDescending(a => a.date_last_modified).ToList();
                ViewBag.status = "received-last";
            }
            else if ("received".Equals(orderby))
            {
                vm.comps = vm.comps.OrderBy(a => a.date_last_modified).ToList();
                ViewBag.status = "received-first";
            }
            vm.categories = db.Categories.ToList();


            ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 1).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 22).ToList().Count;
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 7).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 19).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 15).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 11).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 21).ToList().Count;

            ////////////

            if ((string)TempData["msg"] == "done")
            {
                TempData["msg"] = "Complaints forwarded";

            }
            else
            {
                TempData["msg"] = "";

            }

            return View(vm);
        }
        public ActionResult CHViewForwardedComplaints(String orderby, String cat)
        {

            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            var comp_id_2 = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                      .Where(c => c.stage_id == 2)
                                    .Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                    .ToList();
            var comp_id_3 = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                   .Where(c => c.stage_id == 3)
                                    .Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                    .ToList();
            vm.comps = comp_id_2.Union(comp_id_3).ToList();


            if ("all".Equals(cat))
            {
                ViewBag.cat = "all";
            }
            else
            {
                vm.comps = vm.comps.Where(v => v.Category.Category1.Equals(cat)).ToList();
                ViewBag.cat = cat;
            }

            if ("severe".Equals(orderby))
            {
                vm.comps = (from q in vm.comps
                            orderby q.SupportingComplaints.Count() descending
                            select q).ToList();
                ViewBag.status = "severe-first";
            }
            else if ("recent".Equals(orderby))
            {
                vm.comps = vm.comps.OrderByDescending(a => a.date_last_modified).ToList();
                ViewBag.status = "received-last";
            }
            else if ("received".Equals(orderby))
            {
                vm.comps = vm.comps.OrderBy(a => a.date_last_modified).ToList();
                ViewBag.status = "received-first";
            }
            vm.categories = db.Categories.ToList();



            ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 1).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 22).ToList().Count;
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 7).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 19).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 15).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 11).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 21).ToList().Count;

            ////////////


            return View(vm);
        }





    }
}