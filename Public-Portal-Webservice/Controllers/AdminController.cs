using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Public_Portal_Webservice.Models;
using Public_Portal_Webservice.Models.viewModel;

namespace Public_Portal_Webservice.Controllers
{
    public class AdminController : Controller
    {
        PCP5Entities db = new PCP5Entities();
        public ActionResult LoginAdmin()
        {
            if (User.IsInRole("0"))
            {
                return RedirectToAction("AdminPanel");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult LoginAdmin(LoginAcc acc, string ReturnUrl = "")
        {

            string message = "";
            using (PCP5Entities pc = new PCP5Entities())
            {
                var v = pc.Accounts.Where(a => a.username == acc.username).FirstOrDefault();
                if (v != null)
                {
                    if ("0".Equals(v.Role.ToString()))
                    {
                        if (String.Compare(crypto.Hash(acc.password), v.password) == 0)
                        {

                            String accountType = "Moderator;0";
                            int timeout = true ? 525600 : 1;
                            var ticket = new FormsAuthenticationTicket(1, acc.username, DateTime.Now, DateTime.Now.AddMinutes(timeout), true, accountType);

                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("AdminPanel");
                            }
                        }
                        else
                        {
                            message = "invalid password provided";
                        }

                    }
                    else
                    {
                        message = "invalid credentials provided";
                    }
                }
                else
                {
                    message = "no account associated with this username";
                }
            }

            ViewBag.Message = message;
            return View();
        }


        public ActionResult AdminPanel()
        {
            if (User.IsInRole("0"))
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
            else
            {

                return View("LoginAdmin");
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LoginAdmin");
        }

        [Authorize(Roles = "0")]
        public ActionResult ManageTownAccounts()
        {
            vmPCP vm = new vmPCP();
            vm.towns = db.Towns.ToList();
            vm.accounts = db.Accounts.Where(a => a.Role.Equals(1)).ToList();

            return View(vm);

        }

        [Authorize(Roles = "0")]
        public ActionResult ManageTownAccountSelected(int T_id)
        {
            vmPCP vm = new vmPCP();

            vm.accounts = db.Accounts.Where(d => d.Role.Equals(1))
                                     .Where(c => c.Town_id == T_id)
                                     .ToList();

            vm.towns = db.Towns.ToList();
            vm.town = db.Towns.Where(a => a.town_id == T_id).FirstOrDefault();

            ViewBag.TownSelected = db.Towns.Where(b => b.town_id == T_id).FirstOrDefault().town_name;

            ViewBag.msg = TempData["msg"];

            return View(vm);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTownAccount(int T_id, vmPCP vm)
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

                        TempData["msg"] = "email exists";
                        return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
                    }
                    else if (isUserExist)
                    {
                        ModelState.AddModelError("UserExist", "Username already exists, try another name");

                        TempData["msg"] = "Username exists";
                        return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
                    }


                    using (PCP5Entities pcc = new PCP5Entities())
                    {

                        String username = System.Web.HttpContext.Current.User.Identity.Name;

                        Account acc = new Account();
                        acc.Role = 1;
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
                        acc.Town_id = T_id;

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

                    return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
                }
                TempData["msg"] = "Added";
                return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id, addor = 1 });
            }
            catch (Exception er)
            {
                TempData["msg"] = "Error";
                return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
            }
        }

        private void SendAccountDetailsToMember(string email_address, string username, string password)
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



        [Authorize(Roles = "0")]
        public ActionResult ManageCoordinatorAccounts()
        {
            vmPCP vm = new vmPCP();
            vm.towns = db.Towns.ToList();
            vm.accounts = db.Accounts.Where(a => a.Role.Equals(2)).ToList();

            return View(vm);

        }

        [Authorize(Roles = "0")]
        public ActionResult ManageCoordinatorAccountSelected(int T_id)
        {
            vmPCP vm = new vmPCP();

            vm.accounts = db.Accounts.Where(d => d.Role.Equals(2))
                                     .Where(c => c.Town_id == T_id)
                                     .ToList();

            vm.towns = db.Towns.ToList();
            vm.town = db.Towns.Where(a => a.town_id == T_id).FirstOrDefault();

            ViewBag.TownSelected = db.Towns.Where(b => b.town_id == T_id).FirstOrDefault().town_name;

            ViewBag.msg = TempData["msg"];

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCoordinatorAccount(int T_id, vmPCP vm)
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

                        TempData["msg"] = "email exists";
                        return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
                    }
                    else if (isUserExist)
                    {
                        ModelState.AddModelError("UserExist", "Username already exists, try another name");

                        TempData["msg"] = "Username exists";
                        return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
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
                        acc.Town_id = T_id;

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

                    return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
                }
                TempData["msg"] = "Added";
                return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id, addor = 1 });
            }
            catch (Exception er)
            {
                TempData["msg"] = "Error";
                return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
            }
        }


        [Authorize(Roles = "0")]
        public ActionResult AccountDelete(int account_id, String type, int T_id)
        {
            try
            {
                Account ac2 = db.Accounts.Where(a => a.Account_id.Equals(account_id)).FirstOrDefault();
                db.Accounts.Remove(ac2);
                db.SaveChanges();
                TempData["msg"] = "Deleted";

                if (type.Equals("nazim"))
                {
                    return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
                }
                else if (type.Equals("ch"))
                {
                    return RedirectToAction("ManageCoordinatorAccountSelected", new { T_id = T_id });
                }
                else
                    return RedirectToAction("AdminPanel");
            }
            catch (Exception er)
            {
                TempData["msg"] = "Error";
                if (type.Equals("nazim"))
                {
                    return RedirectToAction("ManageTownAccountSelected", new { T_id = T_id });
                }
                else if (type.Equals("ch"))
                {
                    return RedirectToAction("ManageCoordinatorAccountSelected", new { T_id = T_id });
                }
                else
                    return RedirectToAction("AdminPanel");
            }
        }

        public ActionResult ManageCategories()
        {
            if (User.IsInRole("0"))
            {
                return View();
            }
            else
            {

                return View("LoginAdmin");
            }
        }
    }
}