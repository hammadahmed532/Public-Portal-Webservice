using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Public_Portal_Webservice.Models;
using Public_Portal_Webservice.Models.viewModel;

namespace Public_Portal_Webservice.Controllers
{

    [Authorize(Roles = "1")]
    public class TownAccountController : Controller
    {
        PCP5Entities db = new PCP5Entities();


        public ActionResult TownPanelOverview()
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



            //counting of total baldia members
            vm.accounts = db.Accounts.Where(d => d.Role.Equals(3))
                                     .Where(c => c.Town_id == vm.acc.Town_id).OrderBy(a => a.UCArea.UCName.ToString())
                                    .ToList();
            ViewBag.TotalMembers = vm.accounts.Count();

            vm.accounts = db.Accounts.Where(d => d.Role.Equals(3))
                                             .Where(c => c.Town_id == vm.acc.Town_id).OrderBy(a => a.UCArea.UCName.ToString())
                                            .Take(5).ToList();


            return View(vm);

        }

        public ActionResult TNManageAreaComplaints(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 8)
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
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 8).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 20).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 16).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 12).ToList().Count;
            ViewBag.ReviewActivites = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.admin_confirm == false)
                                    .Where(e => e.stage_id != 1).Count();
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
        public void TNManageAreaComplaintsAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                    ccomp.stage_id = 9;
                    ccomp.date_stage_6 = DateTime.Now;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_6).IsModified = true;
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
                    cdr.Remarks_Nazim = it.Remarks;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";

                }

            }
            //return RedirectToAction("TNManageAreaComplaints", new { orderby = "severe", cat = "all" });
        }
        public ActionResult TNBudgeted(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 20)
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
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 8).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 20).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 16).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 12).ToList().Count;
            ViewBag.ReviewActivites = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.admin_confirm == false)
                                    .Where(e => e.stage_id != 1).Count();
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
        public void TNBudgetedAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                    ccomp.stage_id = 21;
                    ccomp.date_stage_6 = DateTime.Now;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_6).IsModified = true;
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
                    cdr.Remarks_Nazim = it.Remarks;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();
                    TempData["msg"] = "done";

                }

            }
            //return RedirectToAction("TNBudgeted", new { orderby = "severe", cat = "all" });
        }
        public ActionResult TNRejected(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 16)
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
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 8).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 20).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 16).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 12).ToList().Count;
            ViewBag.ReviewActivites = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.admin_confirm == false)
                                    .Where(e => e.stage_id != 1).Count();
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
        public void TNRejectedAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                    ccomp.stage_id = 17;
                    ccomp.date_stage_6 = DateTime.Now;

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_6).IsModified = true;
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
                    cdr.Remarks_Nazim = it.Remarks;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";

                }

            }
            //return RedirectToAction("TNRejected", new { orderby = "severe", cat = "all" });
        }
        public ActionResult TNAlreadyResolved(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 12)
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
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 8).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 20).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 16).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 12).ToList().Count;
            ViewBag.ReviewActivites = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.admin_confirm == false)
                                    .Where(e => e.stage_id != 1).Count();
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
        public void TNAlreadyResolvedAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                    ccomp.stage_id = 13;
                    ccomp.date_stage_6 = DateTime.Now;

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_6).IsModified = true;
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
                    cdr.Remarks_Nazim = it.Remarks;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";

                }

            }
            //return RedirectToAction("TNAlreadyResolved", new { orderby = "severe", cat = "all" });
        }

        public ActionResult BudgetBasket(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(c => c.stage_id == 21)
                                    .ToList();

            vm.comps = vm.comps.Union(db.Complaints.Where(f => f.stage_id == 25).Where(c => c.UCArea.TownID == vm.acc.Town_id).ToList());
            vm.comps = vm.comps.Union(db.Complaints.Where(f => f.stage_id == 26).Where(a => a.UCArea.TownID == vm.acc.Town_id).ToList());
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
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 8).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 20).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 16).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 12).ToList().Count;
            ViewBag.ReviewActivites = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.admin_confirm == false)
                                    .Where(e => e.stage_id != 1).Count();
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
        public void BudgetBasketAction(List<ComplaintsWithRadioBtn> RadioArray)
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
                    ccomp.stage_id = 25;
                    ccomp.date_stage_6 = DateTime.Now;
                    

                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.date_stage_6).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                    cdr.budgetYear = it.budgetYear;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.budgetYear).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";
                }
                else if ("R".Equals(selected))
                {
                    Complaint ccomp = db.Complaints.Find(comp_id);
                    ccomp.date_last_modified = DateTime.Now;
                    ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                    ccomp.is_Notified = false;
                    ccomp.stage_id = 26;
                    db.Complaints.Attach(ccomp);
                    var entry = db.Entry(ccomp);

                    entry.Property(e => e.date_last_modified).IsModified = true;
                    entry.Property(e => e.is_Notified).IsModified = true;
                    entry.Property(e => e.stage_id).IsModified = true;
                    entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                    db.SaveChanges();

                    Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(comp_id)).FirstOrDefault();
                    cdr.Remarks_Nazim = it.Remarks;

                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(a => a.Remarks_Member).IsModified = true;
                    db.SaveChanges();

                    TempData["msg"] = "done";

                }

            }
            //return RedirectToAction("TNAlreadyResolved", new { orderby = "severe", cat = "all" });
        }

        public ActionResult TNForwarded(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
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
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 8).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 20).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 16).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 12).ToList().Count;
            ViewBag.ReviewActivites = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.admin_confirm == false)
                                    .Where(e => e.stage_id != 1).Count();
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
        public ActionResult ReviewComplaintActivities(String orderby, String cat)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            vm.comps = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id)
                                    .Where(d => d.admin_confirm == false)
                                    .Where(e => e.stage_id != 1)
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
            ViewBag.Resolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 8).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                               .Count();
            ViewBag.Budgeted = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 20).ToList().Count;
            ViewBag.Rejected = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 16).ToList().Count;
            ViewBag.AlreadyResolved = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 12).ToList().Count;
            ViewBag.ReviewActivites = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(d => d.admin_confirm == false)
                                    .Where(e => e.stage_id != 1).Count();
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UCArea.TownID == vm.acc.Town_id).Where(c => c.stage_id == 21).ToList().Count;
            
            ////////////

            return View(vm);

        }



        ///////////////////////////////////////////
        ///account configuration///////////////
        public ActionResult ManageUCAccounts()
        {


            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.accounts = db.Accounts.Where(d => d.Role.Equals(3))
                                     .Where(c => c.Town_id == vm.acc.Town_id).OrderBy(a => a.UCArea.UCName.ToString())
                                     .ToList();
            ViewBag.TotalMembers = vm.accounts.Count();


            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();


            return View(vm);



        }
        [HttpGet]
        public ActionResult ManageUCAccountSelected(int uc_id)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.accounts = db.Accounts.Where(d => d.Role.Equals(3))
                                     .Where(c => c.UC_Area_id == uc_id).OrderBy(a => a.Full_name.ToString())
                                     .ToList();
            ViewBag.TotalMembers = vm.accounts.Count();

            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();
            vm.ucSingle = db.UCAreas.Where(a => a.ID == uc_id).FirstOrDefault();
            vm.categories = db.Categories.ToList();
            ViewBag.UCSelected = db.UCAreas.Where(b => b.ID == uc_id).FirstOrDefault().UCName;


            ViewBag.msg = TempData["msg"];

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUCAccount(int uc_idd, vmPCP vm)
        {
            try
            {
                bool Status = false;
                string Message = "";
                //model validataion
                if (ModelState.IsValid)
                {

                    #region email exist
                    var isExist = isEmailExist(vm.accountReg.email_address);
                    var isUserExist = isUsernameExist(vm.accountReg.username);
                    if (isExist)
                    {
                        ModelState.AddModelError("EmailExist", "Email already exists");
                        TempData["msg"] = "Email exists";
                        return RedirectToAction("ManageUCAccountSelected", new { uc_id = uc_idd });
                    }
                    else if (isUserExist)
                    {
                        ModelState.AddModelError("UserExist", "Username already exists, try another name");
                        TempData["msg"] = "username exists";
                        return RedirectToAction("ManageUCAccountSelected", new { uc_id = uc_idd });
                    }


                    using (PCP5Entities pcc = new PCP5Entities())
                    {

                        String username = System.Web.HttpContext.Current.User.Identity.Name;

                        Account acc = new Account();
                        acc.Role = 3;
                        acc.Full_name = vm.accountReg.Full_name;
                        acc.date_created = DateTime.Now;
                        acc.ActivationCode = Guid.NewGuid();
                        acc.address = vm.accountReg.address;
                        acc.username = vm.accountReg.username;
                        acc.password = crypto.Hash(vm.accountReg.password);
                        acc.isEmailVerified = true;
                        acc.gender = vm.accountReg.gender;
                        acc.email_address = vm.accountReg.email_address;
                        acc.phone_number = vm.accountReg.phone_number;
                        acc.Town_id = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault().Town_id;
                        acc.UC_Area_id = uc_idd;

                        pcc.Accounts.Add(acc);
                        pcc.SaveChanges();

                        //send email to user
                        SendAccountDetailsToMember(acc.email_address, acc.username, acc.password);
                        Status = true;
                    }
                    #endregion

                }
                else
                {

                    TempData["msg"] = "Error";

                    return RedirectToAction("ManageUCAccountSelected", new { uc_id = uc_idd });
                }
                TempData["msg"] = "Account added";
                return RedirectToAction("ManageUCAccountSelected", new { uc_id = uc_idd });
            }
            catch (Exception er)
            {
                TempData["msg"] = "Error";
                return RedirectToAction("ManageUCAccountSelected", new { uc_id = uc_idd });
            }
        }

        [NonAction]
        public void SendAccountDetailsToMember(string username, string password, string password1)
        {
        }

        [NonAction]
        public bool isEmailExist(String emailID)
        {
            using (PCP5Entities pcdb = new PCP5Entities())
            {
                var v = pcdb.Accounts.Where(a => a.email_address == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool isUsernameExist(String usernam)
        {
            using (PCP5Entities pc = new PCP5Entities())
            {
                var v = pc.Accounts.Where(a => a.username == usernam).FirstOrDefault();
                return v != null;
            }
        }



        public ActionResult AccountDelete(int account_id, String type, int uc_idd)
        {
            try
            {
                Account ac2 = db.Accounts.Where(a => a.Account_id.Equals(account_id)).FirstOrDefault();               
                ac2.Role= 99;

                db.Accounts.Attach(ac2);
                var entry = db.Entry(ac2);
                entry.Property(e => e.Role).IsModified = true;
                db.SaveChanges();
            
                TempData["msg"] = "Deleted";

                if (type.Equals("uc"))
                {
                    return RedirectToAction("ManageUCAccountSelected", new { uc_id = uc_idd });
                }
                else if (type.Equals("ch"))
                {
                    return RedirectToAction("ManageCHAccountSelected", new { uc_id = uc_idd });
                }
                else if (type.Equals("fo"))
                {
                    return RedirectToAction("ManageFOAccountSelected", new { uc_id = uc_idd });
                }
                else
                    return RedirectToAction("TownPanelOverview");
            }
            catch (Exception er)
            {
                System.Diagnostics.Debug.Write(er);
                TempData["msg"] = "Error";
                if (type.Equals("uc"))
                {
                    return RedirectToAction("ManageUCAccountSelected", new { uc_id = uc_idd });
                }
                else if (type.Equals("ch"))
                {
                    return RedirectToAction("ManageCHAccountSelected", new { uc_id = uc_idd });
                }
                else if (type.Equals("fo"))
                {
                    return RedirectToAction("ManageFOAccountSelected", new { uc_id = uc_idd });
                }
                else
                    return RedirectToAction("TownPanelOverview");
            }
        }



        public ActionResult TownAccountSetting()
        {
            try
            {

                vmPCP vm = new vmPCP();
                String username = System.Web.HttpContext.Current.User.Identity.Name;
                vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();


                return View(vm);


            }
            catch (Exception er)
            {
                return RedirectToAction("TownPanelOverview");
            }
        }












        /// <summary>
        /// complaint handler accounts 
        /// </summary>
        /// <returns></returns>


        public ActionResult ManageCHAccounts()
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.accounts = db.Accounts.Where(d => d.Role.Equals(2))
                                     .Where(c => c.Town_id == vm.acc.Town_id).OrderBy(a => a.UCArea.UCName.ToString())
                                     .ToList();
            ViewBag.TotalMembers = vm.accounts.Count();


            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();

            return View(vm);

        }


        public ActionResult ManageCHAccountSelected(int uc_id)
        {


            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.accounts = db.Accounts.Where(d => d.Role.Equals(2))
                                     .Where(c => c.UC_Area_id == uc_id).OrderBy(a => a.Full_name.ToString())
                                     .ToList();
            ViewBag.TotalMembers = vm.accounts.Count();

            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();
            vm.ucSingle = db.UCAreas.Where(a => a.ID == uc_id).FirstOrDefault();
            vm.categories = db.Categories.ToList();
            ViewBag.UCSelected = db.UCAreas.Where(b => b.ID == uc_id).FirstOrDefault().UCName;

            if (TempData["msg"] == "Error")
            {
                ViewBag.msg = "Error creating account";
            }
            else if (TempData["msg"] == "Added")
            {
                ViewBag.msg = "Account added successfully";
            }
            else if (TempData["msg"] == "Deleted")
            {
                ViewBag.msg = "Account deleted successfully";
            }
            return View(vm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCHAccount(int uc_idd, vmPCP vm)
        {
            try
            {
                bool Status = false;
                string Message = "";
                //model validataion
                if (ModelState.IsValid)
                {

                    #region email exist
                    var isExist = isEmailExist(vm.accountReg.email_address);
                    var isUserExist = isUsernameExist(vm.accountReg.username);
                    if (isExist)
                    {
                        ModelState.AddModelError("EmailExist", "Email already exists");
                        return RedirectToAction("ManageCHAccountSelected", new { uc_id = uc_idd });
                    }
                    else if (isUserExist)
                    {
                        ModelState.AddModelError("UserExist", "Username already exists, try another name");
                        return RedirectToAction("ManageCHAccountSelected", new { uc_id = uc_idd });
                    }


                    using (PCP5Entities pcc = new PCP5Entities())
                    {

                        String username = System.Web.HttpContext.Current.User.Identity.Name;

                        Account acc = new Account();
                        acc.Role = 2;
                        acc.Full_name = vm.accountReg.Full_name;
                        acc.date_created = DateTime.Now;
                        acc.ActivationCode = Guid.NewGuid();
                        acc.address = vm.accountReg.address;
                        acc.username = vm.accountReg.username;
                        acc.password = crypto.Hash(vm.accountReg.password);
                        acc.isEmailVerified = true;
                        acc.gender = vm.accountReg.gender;
                        acc.email_address = vm.accountReg.email_address;
                        acc.phone_number = vm.accountReg.phone_number;
                        acc.Town_id = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault().Town_id;
                        acc.UC_Area_id = uc_idd;

                        pcc.Accounts.Add(acc);
                        pcc.SaveChanges();

                        //send email to user
                        SendAccountDetailsToMember(acc.email_address, acc.username, acc.password);
                        Status = true;
                    }
                    #endregion

                }
                else
                {

                    TempData["msg"] = "Error";

                    return RedirectToAction("ManageCHAccountSelected", new { uc_id = uc_idd });
                }
                TempData["msg"] = "Added";
                return RedirectToAction("ManageCHAccountSelected", new { uc_id = uc_idd });
            }
            catch (Exception er)
            {
                TempData["msg"] = "Error";
                return RedirectToAction("ManageCHAccountSelected", new { uc_id = uc_idd });
            }
        }



        public ActionResult ManageFOAccounts()
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.accounts = db.Accounts.Where(d => d.Role.Equals(4))
                                     .Where(c => c.Town_id == vm.acc.Town_id).OrderBy(a => a.UCArea.UCName.ToString())
                                     .ToList();
            ViewBag.TotalMembers = vm.accounts.Count();


            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();

            return View(vm);

        }
        public ActionResult ManageFOAccountSelected(int uc_id)
        {

            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.accounts = db.Accounts.Where(d => d.Role.Equals(4))
                                     .Where(c => c.UC_Area_id == uc_id).OrderBy(a => a.Full_name.ToString())
                                     .ToList();
            ViewBag.TotalMembers = vm.accounts.Count();

            vm.ucs = db.UCAreas.Where(a => a.TownID == vm.acc.Town_id).ToList();
            vm.ucSingle = db.UCAreas.Where(a => a.ID == uc_id).FirstOrDefault();
            vm.categories = db.Categories.ToList();
            ViewBag.UCSelected = db.UCAreas.Where(b => b.ID == uc_id).FirstOrDefault().UCName;

            if (TempData["msg"] == "Error")
            {
                ViewBag.msg = "Error creating account";
            }
            else if (TempData["msg"] == "Added")
            {
                ViewBag.msg = "Account added successfully";
            }
            else if (TempData["msg"] == "Deleted")
            {
                ViewBag.msg = "Account deleted successfully";
            }
            return View(vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFOAccount(int uc_idd, vmPCP vm)
        {
            try
            {
                bool Status = false;
                string Message = "";
                //model validataion
                if (ModelState.IsValid)
                {

                    #region email exist
                    var isExist = isEmailExist(vm.accountReg.email_address);
                    var isUserExist = isUsernameExist(vm.accountReg.username);
                    if (isExist)
                    {
                        ModelState.AddModelError("EmailExist", "Email already exists");
                        return RedirectToAction("ManageFOAccountSelected", new { uc_id = uc_idd });
                    }
                    else if (isUserExist)
                    {
                        ModelState.AddModelError("UserExist", "Username already exists, try another name");
                        return RedirectToAction("ManageFOAccountSelected", new { uc_id = uc_idd });
                    }


                    using (PCP5Entities pcc = new PCP5Entities())
                    {

                        String username = System.Web.HttpContext.Current.User.Identity.Name;

                        Account acc = new Account();
                        acc.Role = 4;
                        acc.Full_name = vm.accountReg.Full_name;
                        acc.date_created = DateTime.Now;
                        acc.ActivationCode = Guid.NewGuid();
                        acc.address = vm.accountReg.address;
                        acc.username = vm.accountReg.username;
                        acc.password = crypto.Hash(vm.accountReg.password);
                        acc.isEmailVerified = true;
                        acc.gender = vm.accountReg.gender;
                        acc.email_address = vm.accountReg.email_address;
                        acc.phone_number = vm.accountReg.phone_number;
                        acc.Town_id = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault().Town_id;
                        acc.department_id = vm.accountReg.department_id;
                        acc.UC_Area_id = uc_idd;

                        pcc.Accounts.Add(acc);
                        pcc.SaveChanges();

                        //send email to user
                        Status = true;
                    }
                    #endregion

                }
                else
                {
                    Message = "Invalid Request";

                    TempData["msg"] = "Error";

                    return RedirectToAction("ManageFOAccountSelected", new { uc_id = uc_idd });
                }

                TempData["msg"] = "Added";
                return RedirectToAction("ManageFOAccountSelected", new { uc_id = uc_idd });


            }
            catch (Exception er)
            {
                TempData["msg"] = "Error";
                return RedirectToAction("ManageFOAccountSelected", new { uc_id = uc_idd });
            }
        }

    }
}