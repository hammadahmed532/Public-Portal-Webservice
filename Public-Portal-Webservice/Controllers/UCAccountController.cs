using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Public_Portal_Webservice.Models;
using Public_Portal_Webservice.Models.viewModel;

namespace Public_Portal_Webservice.Controllers
{
    [Authorize(Roles = "3")]
        

    public class UCAccountController : Controller
    {

             PCP5Entities db = new PCP5Entities();

            public ActionResult UCPanelOverview()
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
            public ActionResult UCManageAreaComplaints(String orderby, String cat)
            {

                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;
            
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(c => c.stage_id == 2).ToList();
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
                ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 2).ToList().Count;
                ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 23).ToList().Count;
                ViewBag.Resolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 6).ToList().Count;
                ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                   .Count();
                ViewBag.Budgeted = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 18).ToList().Count;
                ViewBag.Rejected = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 14).ToList().Count;
                ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 10).ToList().Count;
            int BudgetBasketCount;
            BudgetBasketCount = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 25).ToList().Count;
            BudgetBasketCount = BudgetBasketCount + (db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 26).ToList()).Count;
            ViewBag.BudgetBasket = BudgetBasketCount;
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
            public void UCManageAreaComplaintsAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                        ccomp.date_stage_3 = DateTime.Now;
                        ccomp.stage_id = 3;
                        db.Complaints.Attach(ccomp);
                        var entry = db.Entry(ccomp);

                        entry.Property(e => e.date_last_modified).IsModified = true;
                        entry.Property(e => e.is_Notified).IsModified = true;
                        entry.Property(e => e.stage_id).IsModified = true;
                        entry.Property(e => e.date_stage_3).IsModified = true;
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
                        ccomp.stage_id = 3;
                        db.Complaints.Attach(ccomp);
                        var entry = db.Entry(ccomp);

                        entry.Property(e => e.date_last_modified).IsModified = true;
                        entry.Property(e => e.is_Notified).IsModified = true;
                        entry.Property(e => e.stage_id).IsModified = true;
                        entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                        db.SaveChanges();


                    }

                }

                TempData["msg"] = "done";
                ViewBag.msg = "Forwarded to Field officer";
            }


            public ActionResult UCViewReviewed(String orderby, String cat)
            {

                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;

                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(c => c.stage_id == 23).ToList();
                


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
                ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 2).ToList().Count;
                ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 23).ToList().Count;
                ViewBag.Resolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 6).ToList().Count;
                ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                   .Count();
                ViewBag.Budgeted = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 18).ToList().Count;
                ViewBag.Rejected = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 14).ToList().Count;
                ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 10).ToList().Count;
            int BudgetBasketCount;
            BudgetBasketCount = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 25).ToList().Count;
            BudgetBasketCount = BudgetBasketCount + (db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 26).ToList()).Count;
            ViewBag.BudgetBasket = BudgetBasketCount;
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
            public void UCViewReviewedAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                        ccomp.stage_id = 24;
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
                        Complaint ccomp = db.Complaints.Find(comp_id);
                        ccomp.date_last_modified = DateTime.Now;
                        ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;

                        ccomp.is_Notified = false;
                        ccomp.stage_id = 22;

                        db.Complaints.Attach(ccomp);
                        var entry = db.Entry(ccomp);

                        entry.Property(e => e.date_last_modified).IsModified = true;
                        entry.Property(e => e.is_Notified).IsModified = true;
                        entry.Property(e => e.stage_id).IsModified = true;
                        entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                        db.SaveChanges();

                        Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                        cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                        cdr.Remarks_Member = it.Remarks;

                        db.Complaint_Det_Remarks.Attach(cdr);
                        var entry2 = db.Entry(cdr);
                        entry2.Property(a => a.Remarks_Member).IsModified = true;
                        db.SaveChanges();

                        TempData["msg"] = "done";

                    }

                }
                //return RedirectToAction("UCViewReviewed",new { orderby="severe",cat="all"});
            }
            public ActionResult UCViewForwardedComplaints(String orderby, String cat)
            {
                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                    .ToList();
                vm.categories = db.Categories.ToList();

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

                ///////////////
                ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 2).ToList().Count;
                ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 23).ToList().Count;
                ViewBag.Resolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 6).ToList().Count;
                ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                   .Count();
                ViewBag.Budgeted = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 18).ToList().Count;
                ViewBag.Rejected = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 14).ToList().Count;
                ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 10).ToList().Count;
            int BudgetBasketCount;
            BudgetBasketCount = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 25).ToList().Count;
            BudgetBasketCount = BudgetBasketCount + (db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 26).ToList()).Count;
            ViewBag.BudgetBasket = BudgetBasketCount;
            ////////////
            ///

            return View(vm);
            }
            public ActionResult UCViewResolved(String orderby, String cat)
            {

                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;

                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(c => c.stage_id == 6).ToList();
                vm.categories = db.Categories.ToList();

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

                ///////////////
                ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 2).ToList().Count;
                ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 23).ToList().Count;
                ViewBag.Resolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 6).ToList().Count;
                ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                   .Count();
                ViewBag.Budgeted = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 18).ToList().Count;
                ViewBag.Rejected = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 14).ToList().Count;
                ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 10).ToList().Count;
            int BudgetBasketCount;
            BudgetBasketCount = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 25).ToList().Count;
            BudgetBasketCount = BudgetBasketCount + (db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 26).ToList()).Count;
            ViewBag.BudgetBasket = BudgetBasketCount;
            ////////////
            ///
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

        
            public void UCViewResolvedAction(List<ComplaintsWithRadioBtn> RadioArray)
            {

                var comps = RadioArray;
                int comp_id;
                String selected;
                String rem;
                foreach (var it in comps)
                {
                    comp_id = it.complaint_Id;
                    selected = it.SelectedRadio;
                    rem = it.Remarks;
                    if ("A".Equals(selected))
                    {

                        Complaint ccomp = db.Complaints.Find(comp_id);
                        ccomp.date_last_modified = DateTime.Now;
                        ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                        ccomp.is_Notified = false;
                        ccomp.stage_id = 7;
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
                        Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                        cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                        cdr.Remarks_Member = rem;
                        db.Complaint_Det_Remarks.Attach(cdr);
                        var entry2 = db.Entry(cdr);
                        entry2.Property(e => e.Remarks_Member).IsModified = true;
                        db.SaveChanges();

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


                        TempData["msg"] = "done";
                    }

                }
                //return RedirectToAction("UCViewResolved",new { orderyby="severe",cat="all"});

            }

        public ActionResult UCBudgetBasket(String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;

            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                    .Where(c => c.stage_id == 25).ToList();
            vm.comps = vm.comps.Union(db.Complaints.Where(f => f.stage_id == 26).Where(c => c.UC_area_id == vm.acc.UC_Area_id).ToList());


            if ("all".Equals(cat))
            {
                ViewBag.cat = "all";
            }
            else
            {
                vm.comps = vm.comps.Where(v => v.Category.Category1.Equals(cat)).ToList();
                ViewBag.cat = cat;
            }
            

            vm.categories = db.Categories.ToList();
            ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 2).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 23).ToList().Count;
            ViewBag.Resolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 6).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 18).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 14).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 10).ToList().Count;
            int BudgetBasketCount;
            BudgetBasketCount = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 25).ToList().Count;
            BudgetBasketCount =BudgetBasketCount +(db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 26).ToList()).Count;
            ViewBag.BudgetBasket = BudgetBasketCount;


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
        public void UCBudgetBasketAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                    ccomp.stage_id = 27;
                 
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                    cdr.Remarks_Member = it.Remarks;
                    
                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }
                //else if ("R".Equals(selected))
                //{

                //    Complaint ccomp = db.Complaints.Find(comp_id);
                //    ccomp.date_last_modified = DateTime.Now;
                //    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;

                //    ccomp.is_Notified = false;
                //    ccomp.stage_id = 24;

                //    db.Complaints.Attach(ccomp);
                //    var entry = db.Entry(ccomp);

                //    entry.Property(e => e.date_last_modified).IsModified = true;
                //    entry.Property(e => e.is_Notified).IsModified = true;
                //    entry.Property(e => e.stage_id).IsModified = true;
                //    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                //    db.SaveChanges();

                //    Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                //    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                //    cdr.Remarks_Member = it.Remarks;

                //    db.Complaint_Det_Remarks.Attach(cdr);
                //    var entry2 = db.Entry(cdr);
                //    entry2.Property(a => a.Remarks_Member).IsModified = true;
                //    db.SaveChanges();

                //    TempData["msg"] = "done";

                //}
            }
        }
            
        
        public ActionResult UCViewBudgedted(String orderby, String cat)
            {

                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;

                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(c => c.stage_id == 18).ToList();
                vm.categories = db.Categories.ToList();

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


                ///////////////
                ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 2).ToList().Count;
                ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 23).ToList().Count;
                ViewBag.Resolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 6).ToList().Count;
                ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                   .Count();
                ViewBag.Budgeted = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 18).ToList().Count;
                ViewBag.Rejected = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 14).ToList().Count;
                ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 10).ToList().Count;
            int BudgetBasketCount;
            BudgetBasketCount = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 25).ToList().Count;
            BudgetBasketCount = BudgetBasketCount + (db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 26).ToList()).Count;
            ViewBag.BudgetBasket = BudgetBasketCount;
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
            public void UCViewBudgedtedAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                        ccomp.stage_id = 19;
                        ccomp.Expected_amount = it.expectedAmount;

                        db.Complaints.Attach(ccomp);
                        var entry = db.Entry(ccomp);

                        entry.Property(e => e.date_last_modified).IsModified = true;
                        entry.Property(e => e.is_Notified).IsModified = true;
                        entry.Property(e => e.stage_id).IsModified = true;
                        entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                        entry.Property(e => e.Expected_amount).IsModified = true;
                        db.SaveChanges();

                        Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                         cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                         cdr.Remarks_Member = it.Remarks;
                    cdr.severity_lvl = it.rating;

                        db.Complaint_Det_Remarks.Attach(cdr);
                        var entry2 = db.Entry(cdr);

                    entry2.Property(a => a.severity_lvl).IsModified = true;
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
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
                        cdr.Remarks_Member = it.Remarks;

                        db.Complaint_Det_Remarks.Attach(cdr);
                        var entry2 = db.Entry(cdr);
                        entry2.Property(a => a.Remarks_Member).IsModified = true;
                        db.SaveChanges();

                        TempData["msg"] = "done";

                    }

                }
                //return RedirectToAction("UCViewBudgedted", new { orderby = "severe", cat = "all" });
            }
            public ActionResult UCViewRejected(String orderby, String cat)
            {

                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;

                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(c => c.stage_id == 14).ToList();

                vm.categories = db.Categories.ToList();

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

                ///////////////
                ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 2).ToList().Count;
                ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 23).ToList().Count;
                ViewBag.Resolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 6).ToList().Count;
                ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                   .Count();
                ViewBag.Budgeted = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 18).ToList().Count;
                ViewBag.Rejected = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 14).ToList().Count;
                ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 10).ToList().Count;
            int BudgetBasketCount;
            BudgetBasketCount = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 25).ToList().Count;
            BudgetBasketCount = BudgetBasketCount + (db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 26).ToList()).Count;
            ViewBag.BudgetBasket = BudgetBasketCount;
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
            public void UCViewRejectedAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                        ccomp.stage_id = 15;
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
                        cdr.Remarks_Member = it.Remarks;

                        db.Complaint_Det_Remarks.Attach(cdr);
                        var entry2 = db.Entry(cdr);
                        entry2.Property(a => a.Remarks_Member).IsModified = true;
                        db.SaveChanges();

                        TempData["msg"] = "done";

                    }

                }
                //return RedirectToAction("UCViewRejected", new { orderby = "severe", cat = "all" });
            }

            public ActionResult UCViewAlreadyResolved(String orderby, String cat)
            {

                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;

                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(c => c.stage_id == 10).ToList();
                vm.categories = db.Categories.ToList();
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


                ///////////////
                ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 2).ToList().Count;
                ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 23).ToList().Count;
                ViewBag.Resolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 6).ToList().Count;
                ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                   .Count();
                ViewBag.Budgeted = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 18).ToList().Count;
                ViewBag.Rejected = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 14).ToList().Count;
                ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 10).ToList().Count;
            int BudgetBasketCount;
            BudgetBasketCount = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 25).ToList().Count;
            BudgetBasketCount = BudgetBasketCount + (db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 26).ToList()).Count;
            ViewBag.BudgetBasket = BudgetBasketCount;
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
            public void UCViewAlreadyResolvedAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                        ccomp.stage_id = 11;
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
                        cdr.Remarks_Member = it.Remarks;

                        db.Complaint_Det_Remarks.Attach(cdr);
                        var entry2 = db.Entry(cdr);
                        entry2.Property(a => a.Remarks_Member).IsModified = true;
                        db.SaveChanges();

                        TempData["msg"] = "done";

                    }

                }
                //return RedirectToAction("UCViewAlreadyResolved", new { orderby = "severe", cat = "all" });
            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="Complaint_ID"></param>
            /// <returns></returns>
            public ActionResult UCApproveAction(int Complaint_ID)
            {

                using (PCP5Entities db = new PCP5Entities())
                {

                    Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();
                    ccomp.stage_id = 3;
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.date_stage_3 = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_3).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();
                    TempData["msg"] = "Complaint sent to field officer";

                }
                return RedirectToAction("UCManageAreaComplaints");
            }

            public ActionResult UCRejectAction(int Complaint_ID)
            {
                using (PCP5Entities db = new PCP5Entities())
                {

                    Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();
                    ccomp.stage_id = 10;
                    ccomp.date_stage_6 = DateTime.Now;
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.admin_confirm = false;

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_6).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();
                    TempData["msg"] = "Complaint rejected";

                }
                return RedirectToAction("UCManageAreaComplaints");
            }



            public ActionResult ApproveIncomingAction(int Complaint_ID)
            {
                using (PCP5Entities db = new PCP5Entities())
                {

                    Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();

                    if (ccomp.stage_id == 10)
                    {
                        ccomp.stage_id = 11;

                    }
                    else if (ccomp.stage_id == 14)
                    {
                        ccomp.stage_id = 15;

                    }

                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();
                    TempData["msg"] = "Work approved sent to coordinator for verification";

                }

                return RedirectToAction("UCIncomingComplaints");
            }

            public ActionResult RejectIncomingAction(int Complaint_ID)
            {
                return RedirectToAction("UCIncomingComplaints");

            }

            public ActionResult UCResolveComplaint()
            {

                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;

                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(c => c.stage_id == 6).ToList();

                ViewBag.msg = TempData["msg"];

                return View(vm);
            }
            public ActionResult UCResolveComplaintAction(int Complaint_ID)
            {
                using (PCP5Entities db = new PCP5Entities())
                {

                    Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();
                    ccomp.stage_id = 7;
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();
                    TempData["msg"] = "Complaint resolved sent to coordinator for verification";

                }

                return RedirectToAction("UCResolveComplaint");
            }


            public ActionResult UCBudgetComplaints()
            {
                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;

                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

                vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                        .Where(c => c.stage_id == 6).ToList();

                ViewBag.msg = TempData["msg"];

                return View(vm);
            }





        }
    }