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
    [Authorize(Roles = "4")]
    public class FOAccountController : Controller
    {
        PCP5Entities db = new PCP5Entities();

        public ActionResult FOPanelOverview()
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


        public ActionResult FOManageAreaComplaints(String orderby)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                    
                                    .Where(c => c.stage_id == 3)
                                    .Where(d=>d.category_id==vm.acc.Category.category_id)
                                    .ToList();


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


            //////////////// ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 3)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 24)
                                  .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                    .Where(d => d.category_id == vm.acc.Category.category_id)   .Count();
            ViewBag.Update = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 4)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Resolve = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 5)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 27)
                                   .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;

            //////////////////////////


            ViewBag.msg = TempData["msg"];

            return View(vm);
        }

        public ActionResult FOViewReviewed(String orderby)
        {
            
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                    .Where(c => c.stage_id == 24)
                                    .Where(d => d.category_id == vm.acc.Category.category_id)
                                    .ToList();


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

            //////////////// ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 3)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 24)
                                  .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).Count();
            ViewBag.Update = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 4)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Resolve = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 5)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 27)
                                   .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;

            //////////////////////////

            ViewBag.msg = TempData["msg"];


            return View(vm);
        }

        public ActionResult FOUpdateComplaintProgress(String orderby)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                    .Where(c => c.stage_id == 4)
                                    .Where(d => d.category_id == vm.acc.Category.category_id)
                                    .ToList();


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

            //////////////// ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 3)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 24)
                                  .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).Count();
            ViewBag.Update = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 4)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Resolve = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 5)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 27)
                                   .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;

            //////////////////////////


            ViewBag.msg = TempData["msg"];


            return View(vm);
        }

        public ActionResult ResolvedComplaints(String orderby)
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)

                                    .Where(d => d.category_id == vm.acc.Category.category_id)
                                    .Where(c => c.stage_id == 5).ToList();
                            


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
            //////////////// ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 3)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 24)
                                  .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).Count();
            ViewBag.Update = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 4)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Resolve = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 5)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 27)
                                   .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;

            //////////////////////////


            ViewBag.msg = TempData["msg"];


            return View(vm);
        }



        public ActionResult FirstComplaintAction(int Complaint_ID, vmPCP vm)
        {

            try
            {
                string fileName = string.Empty;
                string extension = string.Empty;
                Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();
                ccomp.stage_id = 4;
                ccomp.date_stage_4 = DateTime.Now;
                ccomp.date_last_modified = DateTime.Now;
                ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                ccomp.complaint_Type = "first";
                ccomp.is_Notified = false;

                db.Complaints.Attach(ccomp);

                var entry = db.Entry(ccomp);

                entry.Property(e => e.date_last_modified).IsModified = true;
                entry.Property(e => e.is_Notified).IsModified = true;
                entry.Property(e => e.stage_id).IsModified = true;
                entry.Property(e => e.date_stage_4).IsModified = true;
                entry.Property(e => e.complaint_Type).IsModified = true;

                entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;


                db.SaveChanges();

                Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                var check = db.Complaint_Det_Remarks.Where(c => c.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                if (check == null)
                {
                    cdr.Complaint_id = Complaint_ID;
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;
                    db.Complaint_Det_Remarks.Add(cdr);
                    db.SaveChanges();
                }
                else
                {
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;
                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    entry2.Property(e => e.Remarks_Field_Off).IsModified = true;
                    db.SaveChanges();
                }


                TempData["msg"] = "Complaint work started";

                return RedirectToAction("FOManageAreaComplaints", new { orderby = "severe" });
            }
            catch (Exception er)
            {

                TempData["msg"] = "Error!";
                return RedirectToAction("FOManageAreaComplaints", new { orderby = "severe" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlreadyComplaintAction(int Complaint_ID, vmPCP vm)
        {

            try
            {
                string fileName = string.Empty;
                string extension = string.Empty;
                Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();
                ccomp.stage_id = 10;
                ccomp.date_stage_4 = DateTime.Now;
                ccomp.date_last_modified = DateTime.Now;
                ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                ccomp.complaint_Type = "already resolved";

                db.Complaints.Attach(ccomp);
                var entry = db.Entry(ccomp);

                entry.Property(e => e.date_last_modified).IsModified = true;
                entry.Property(e => e.stage_id).IsModified = true;
                entry.Property(e => e.date_stage_4).IsModified = true;
                entry.Property(e => e.complaint_Type).IsModified = true;
                entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;


                db.SaveChanges();


                Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                var check = db.Complaint_Det_Remarks.Where(c => c.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                if (check == null)
                {
                    cdr.Complaint_id = Complaint_ID;
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;
                }
                else
                {
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;

                }
                if (vm.Comp_det_Annotations.DetailsimageFile1 != null)
                {
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile1.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile1.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image1 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile1.SaveAs(fileName);


                    fileName = string.Empty;
                    extension = string.Empty;
                }
                if (vm.Comp_det_Annotations.DetailsimageFile2 != null)
                {
                    //adding image 2
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile2.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile2.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image2 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile2.SaveAs(fileName);


                    fileName = string.Empty;
                    extension = string.Empty;
                }
                if (vm.Comp_det_Annotations.DetailsimageFile3 != null)
                {
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile3.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile3.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image3 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile3.SaveAs(fileName);

                }
                if (check == null)
                {

                    db.Complaint_Det_Remarks.Add(cdr);
                    db.SaveChanges();
                }
                else
                {
                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    if (vm.Comp_det_Annotations.DetailsimageFile1 != null)
                    {
                        entry2.Property(e => e.updated_Image1).IsModified = true;
                    }

                    if (vm.Comp_det_Annotations.DetailsimageFile2 != null)
                    {

                        entry2.Property(e => e.updated_Image2).IsModified = true;
                    }

                    if (vm.Comp_det_Annotations.DetailsimageFile3 != null)
                    {
                        entry2.Property(e => e.updated_Image3).IsModified = true;
                    }

                    entry2.Property(e => e.Remarks_Field_Off).IsModified = true;
                    db.SaveChanges();
                }


                TempData["msg"] = "Complaint forwarded to UC nazim for review";

                return RedirectToAction("FOManageAreaComplaints", new { orderby = "severe" });
            }
            catch (Exception er)
            {

                TempData["msg"] = "Error!";
                return RedirectToAction("FOManageAreaComplaints", new { orderby = "severe" });
            }
        }
        public ActionResult FOBudgetBasket()
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();

            vm.comps = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id)
                                    .Where(d => d.category_id == vm.acc.Category.category_id)
                                    .Where(c => c.stage_id == 27).ToList();
            
            //////////////// ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 3)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 24)
                                  .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).Count();
            ViewBag.Update = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 4)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Resolve = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 5)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 27)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;

            //////////////////////////


            ViewBag.msg = TempData["msg"];


            return View(vm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BudgetComplaintAction(int Complaint_ID, vmPCP vm)
        {


            try
            {
                string fileName = string.Empty;
                string extension = string.Empty;
                Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();
                ccomp.stage_id = 18;
                ccomp.date_stage_4 = DateTime.Now;
                ccomp.date_last_modified = DateTime.Now;
                ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                ccomp.complaint_Type = "budget";
                //ccomp.Expected_amount = vm.Comp_det_Annotations.Expected_amount;

                db.Complaints.Attach(ccomp);
                var entry = db.Entry(ccomp);

                entry.Property(e => e.date_last_modified).IsModified = true;
                entry.Property(e => e.stage_id).IsModified = true;
                entry.Property(e => e.date_stage_4).IsModified = true;
                entry.Property(e => e.complaint_Type).IsModified = true;
                entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;
                //entry.Property(e => e.Expected_amount).IsModified = true;


                db.SaveChanges();


                Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                var check = db.Complaint_Det_Remarks.Where(c => c.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                if (check == null)
                {
                    cdr.Complaint_id = Complaint_ID;
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;
                }
                else
                {
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;

                }
                if (vm.Comp_det_Annotations.DetailsimageFile1 != null)
                {
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile1.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile1.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image1 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile1.SaveAs(fileName);


                    fileName = string.Empty;
                    extension = string.Empty;
                }
                if (vm.Comp_det_Annotations.DetailsimageFile2 != null)
                {
                    //adding image 2
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile2.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile2.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image2 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile2.SaveAs(fileName);


                    fileName = string.Empty;
                    extension = string.Empty;
                }
                if (vm.Comp_det_Annotations.DetailsimageFile3 != null)
                {
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile3.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile3.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image3 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile3.SaveAs(fileName);

                }
                if (check == null)
                {

                    db.Complaint_Det_Remarks.Add(cdr);
                    db.SaveChanges();
                }
                else
                {
                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    if (vm.Comp_det_Annotations.DetailsimageFile1 != null)
                    {
                        entry2.Property(e => e.updated_Image1).IsModified = true;
                    }

                    if (vm.Comp_det_Annotations.DetailsimageFile2 != null)
                    {

                        entry2.Property(e => e.updated_Image2).IsModified = true;
                    }

                    if (vm.Comp_det_Annotations.DetailsimageFile3 != null)
                    {
                        entry2.Property(e => e.updated_Image3).IsModified = true;
                    }

                    entry2.Property(e => e.Remarks_Field_Off).IsModified = true;
                    db.SaveChanges();
                }



                TempData["msg"] = "Complaint forwarded to UC nazim for review";

                return RedirectToAction("FOManageAreaComplaints", new { orderby = "severe" });
            }
            catch (Exception er)
            {

                TempData["msg"] = "Error!";
                return RedirectToAction("FOManageAreaComplaints", new { orderby = "severe" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectBogusAction(int Complaint_ID, vmPCP vm)
        {
            try
            {
                string fileName = string.Empty;
                string extension = string.Empty;
                Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();
                ccomp.stage_id = 14;
                ccomp.date_stage_4 = DateTime.Now;
                ccomp.date_last_modified = DateTime.Now;
                ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;
                ccomp.complaint_Type = "bogus";



                db.Complaints.Attach(ccomp);
                var entry = db.Entry(ccomp);

                entry.Property(e => e.date_last_modified).IsModified = true;
                entry.Property(e => e.stage_id).IsModified = true;
                entry.Property(e => e.date_stage_4).IsModified = true;
                entry.Property(e => e.complaint_Type).IsModified = true;

                entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;


                db.SaveChanges();

                Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                var check = db.Complaint_Det_Remarks.Where(c => c.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                if (check == null)
                {
                    cdr.Complaint_id = Complaint_ID;
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;
                }
                else
                {
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;

                }
                fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile1.FileName);
                extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile1.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                cdr.updated_Image1 = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                vm.Comp_det_Annotations.DetailsimageFile1.SaveAs(fileName);


                fileName = string.Empty;
                extension = string.Empty;
                if (vm.Comp_det_Annotations.DetailsimageFile2 != null)
                {
                    //adding image 2
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile2.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile2.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image2 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile2.SaveAs(fileName);


                    fileName = string.Empty;
                    extension = string.Empty;
                }
                if (vm.Comp_det_Annotations.DetailsimageFile3 != null)
                {
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile3.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile3.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image3 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile3.SaveAs(fileName);


                }
                if (check == null)
                {

                    db.Complaint_Det_Remarks.Add(cdr);
                    db.SaveChanges();
                }
                else
                {
                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    if (vm.Comp_det_Annotations.DetailsimageFile1 != null)
                    {
                        entry2.Property(e => e.updated_Image1).IsModified = true;
                    }

                    if (vm.Comp_det_Annotations.DetailsimageFile2 != null)
                    {

                        entry2.Property(e => e.updated_Image2).IsModified = true;
                    }

                    if (vm.Comp_det_Annotations.DetailsimageFile3 != null)
                    {
                        entry2.Property(e => e.updated_Image3).IsModified = true;
                    }

                    entry2.Property(e => e.Remarks_Field_Off).IsModified = true;
                    db.SaveChanges();
                }


                TempData["msg"] = "Complaint forwarded to UC nazim for review";

                return RedirectToAction("FOManageAreaComplaints", new { orderby = "severe" });
            }
            catch (Exception er)
            {

                TempData["msg"] = "Error!";
                return RedirectToAction("FOManageAreaComplaints", new { orderby = "severe" });
            }



        }



        public ActionResult UpdateProgressAction(int Complaint_ID)
        {
            try
            {
                string fileName = string.Empty;
                string extension = string.Empty;
                Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();
                ccomp.stage_id = 5;
                ccomp.date_stage_5 = DateTime.Now;
                ccomp.date_last_modified = DateTime.Now;
                ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;

                ccomp.is_Notified = false;

                db.Complaints.Attach(ccomp);
                var entry = db.Entry(ccomp);

                entry.Property(e => e.date_last_modified).IsModified = true;
                entry.Property(e => e.stage_id).IsModified = true;
                entry.Property(e => e.date_stage_5).IsModified = true;
                entry.Property(e => e.complaint_Type).IsModified = true;
                entry.Property(e => e.is_Notified).IsModified = true;
                entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;

                db.SaveChanges();

                TempData["msg"] = "Complaint progress updated";

                return RedirectToAction("FOUpdateComplaintProgress", new { orderby = "severe" });
            }
            catch (Exception er)
            {

                TempData["msg"] = "Error!";
                return RedirectToAction("FOUpdateComplaintProgress", new { orderby = "severe" });
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResolveProgressAction(int Complaint_ID, vmPCP vm)
        {

            try
            {
                string fileName = string.Empty;
                string extension = string.Empty;
                Complaint ccomp = db.Complaints.Where(a => a.complaint_Id == Complaint_ID).FirstOrDefault();
                ccomp.stage_id = 6;
                ccomp.date_last_modified = DateTime.Now;
                ccomp.Forwarded_By_Account_id = db.Accounts.Where(a => a.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).FirstOrDefault().Account_id;

                db.Complaints.Attach(ccomp);
                var entry = db.Entry(ccomp);

                entry.Property(e => e.date_last_modified).IsModified = true;
                entry.Property(e => e.stage_id).IsModified = true;

                entry.Property(e => e.Forwarded_By_Account_id).IsModified = true;


                db.SaveChanges();


                Complaint_Det_Remarks cdr = new Complaint_Det_Remarks();
                var check = db.Complaint_Det_Remarks.Where(c => c.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                if (check == null)
                {
                    cdr.Complaint_id = Complaint_ID;
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;
                }
                else
                {
                    cdr = db.Complaint_Det_Remarks.Where(a => a.Complaint_id.Equals(Complaint_ID)).FirstOrDefault();
                    cdr.Remarks_Field_Off = vm.Comp_det_Annotations.Remarks_Field_Off;

                }


                fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile1.FileName);
                extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile1.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                cdr.updated_Image1 = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                vm.Comp_det_Annotations.DetailsimageFile1.SaveAs(fileName);


                fileName = string.Empty;
                extension = string.Empty;
                if (vm.Comp_det_Annotations.DetailsimageFile2 != null)
                {
                    //adding image 2
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile2.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile2.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image2 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile2.SaveAs(fileName);


                    fileName = string.Empty;
                    extension = string.Empty;
                }
                if (vm.Comp_det_Annotations.DetailsimageFile3 != null)
                {
                    fileName = Path.GetFileNameWithoutExtension(vm.Comp_det_Annotations.DetailsimageFile3.FileName);
                    extension = Path.GetExtension(vm.Comp_det_Annotations.DetailsimageFile3.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff_updated") + extension;
                    cdr.updated_Image3 = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    vm.Comp_det_Annotations.DetailsimageFile3.SaveAs(fileName);
                }

                if (check == null)
                {

                    db.Complaint_Det_Remarks.Add(cdr);
                    db.SaveChanges();
                }
                else
                {
                    db.Complaint_Det_Remarks.Attach(cdr);
                    var entry2 = db.Entry(cdr);
                    if (vm.Comp_det_Annotations.DetailsimageFile1 != null)
                    {
                        entry2.Property(e => e.updated_Image1).IsModified = true;
                    }

                    if (vm.Comp_det_Annotations.DetailsimageFile2 != null)
                    {

                        entry2.Property(e => e.updated_Image2).IsModified = true;
                    }

                    if (vm.Comp_det_Annotations.DetailsimageFile3 != null)
                    {
                        entry2.Property(e => e.updated_Image3).IsModified = true;
                    }

                    entry2.Property(e => e.Remarks_Field_Off).IsModified = true;
                    db.SaveChanges();
                }

                TempData["msg"] = "Complaint forwarded to UC nazim for review";

                return RedirectToAction("ResolvedComplaints", new { orderby = "severe" });
            }
            catch (Exception er)
            {

                TempData["msg"] = "Error!";
                return RedirectToAction("ResolvedComplaints", new { orderby = "severe" });
            }

        }


        public ActionResult FOViewForwardedComplaints()
        {
            vmPCP vm = new vmPCP();
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            vm.comps = db.Complaints.Where(c => c.Forwarded_By_Account_id == vm.acc.Account_id).ToList();

            //////////////// ///////////////
            ViewBag.Recieved = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 3)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Reviewed = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 24)
                                  .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Forwarded = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(d => d.Forwarded_By_Account_id == vm.acc.Account_id)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).Count();
            ViewBag.Update = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 4)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.Resolve = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 5)
                                    .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;
            ViewBag.BudgetBasket = db.Complaints.Where(b => b.UC_area_id == vm.acc.UC_Area_id).Where(c => c.stage_id == 27)
                                   .Where(d => d.category_id == vm.acc.Category.category_id).ToList().Count;

            //////////////////////////



            return View(vm);
        }
    }
}