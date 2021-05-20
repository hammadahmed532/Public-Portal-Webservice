using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Public_Portal_Webservice.Models;
using Public_Portal_Webservice.Models.viewModel;

namespace Public_Portal_Webservice.Controllers
{
    public class AccountController : Controller
    {

        PCP5Entities db = new PCP5Entities();
        //Registration action
        [HttpGet]
        public ActionResult Registration()
        {
            PCP5Entities db = new PCP5Entities();
            vmPCP vm = new vmPCP();
            vm.towns = db.Towns.ToList();

            return View(vm);
        }
        [HttpGet]
        public ActionResult RegistrationUCSelect(String TownSelected)
        {
            Session["TownRegg"] = TownSelected;

            int town_id_int = Convert.ToInt32(TownSelected);
            vmPCP vm = new vmPCP();
            PCP5Entities db = new PCP5Entities();
            //vm.ucs = db.UCAreas.Where(c => c.TownID == town_id_int).ToList();
            return View(vm);
        }

        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult RegistrationUCSelect([Bind(Exclude = "isEmailVerified,ActivationCode")] vmPCP user)
        {
            try
            {
                bool Status = false;
                string Message = "";
                int Town_id_int = Convert.ToInt32(Session["TownRegg"]);
                //model validataion
                if (ModelState.IsValid)
                {

                    #region email exist
                    var isExist = isEmailExist(user.accountReg.email_address);
                    var isUserExist = isUsernameExist(user.accountReg.username);
                    if (isExist)
                    {
                        ModelState.AddModelError("EmailExist", "Email already exists");
                        user.ucs = db.UCAreas.Where(c => c.TownID == Town_id_int).ToList();
                        return View(user);
                    }
                    else if (isUserExist)
                    {
                        ModelState.AddModelError("UserExist", "Username already exists, try another name");
                        user.ucs = db.UCAreas.Where(c => c.TownID == Town_id_int).ToList();
                        return View(user);
                    }
                    else if (!user.accountReg.password.Equals(user.accountReg.confirmPassword))
                    {
                        ModelState.AddModelError("passwordNotSame", "Password and confirm password does not match");
                        user.ucs = db.UCAreas.Where(c => c.TownID == Town_id_int).ToList();
                        return View(user);
                    }
                    #endregion

                    //generate activation code

                    #region Save to Database
                    using (PCP5Entities pcc = new PCP5Entities())
                    {
                        Account acc = new Account();
                        acc.Role = 5;
                        acc.Full_name = user.accountReg.Full_name;
                        acc.date_created = DateTime.Now;
                        acc.ActivationCode = Guid.NewGuid();
                        acc.address = user.accountReg.address;
                        acc.username = user.accountReg.username;
                        acc.password = crypto.Hash(user.accountReg.password);
                        acc.isEmailVerified = false;
                        acc.gender = user.accountReg.gender;
                        acc.email_address = user.accountReg.email_address;
                        acc.phone_number = user.accountReg.phone_number;
                        acc.Town_id = Town_id_int;
                        acc.UC_Area_id = user.acc.UC_Area_id;


                        pcc.Accounts.Add(acc);
                        pcc.SaveChanges();

                        //send email to user
                        SendVerificationLinkEmail(acc.email_address, acc.ActivationCode.ToString(), acc.Full_name);
                        Message = "Registration successfully done!. Account activation link has been sent to " +
                            "your email id: " + acc.email_address;
                        Status = true;
                    }
                    #endregion

                }
                else
                {
                    Message = "Invalid Request";
                    user.ucs = db.UCAreas.Where(c => c.TownID == Town_id_int).ToList();
                    return View(user);
                }


                TempData["Status"] = Message;

                return RedirectToAction("Login");
            }
            catch (Exception er)
            {
                int town_id_int = Convert.ToInt32(Session["TownRegg"]);
                vmPCP vm = new vmPCP();
                vm.ucs = db.UCAreas.Where(c => c.TownID == town_id_int).ToList();

                ViewBag.msg = "Please enter all fields";

                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult VerifyAccount(String id)
        {
            bool Status = false;
            using (PCP5Entities pcd = new PCP5Entities())
            {
                pcd.Configuration.ValidateOnSaveEnabled = false;

                var v = pcd.Accounts.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.isEmailVerified = true;
                    pcd.SaveChanges();
                    Status = true;

                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {

            if (!Request.IsAuthenticated)
            {
                ViewBag.msg = TempData["Status"];
                return View();
            }
            else
            {

                return RedirectToAction("Index", "Complaint");
            }

        }
        [HttpPost]
       // [ValidateAntiForgeryToken]
        //[OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.None)]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login(vmPCP acc2, string ReturnUrl = "")
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon();

                }
                string message = "";
                using (PCP5Entities pc = new PCP5Entities())
                {
                    var v = pc.Accounts.Where(a => a.username == acc2.login.username).FirstOrDefault();
                    if (v != null)
                    {
                        if (String.Compare(crypto.Hash(acc2.login.password), v.password) == 0)
                        {
                            String accountType = "Moderator;" + String.Concat(pc.Accounts.Where(u => u.username.Equals(acc2.login.username)).FirstOrDefault().Role).ToString();
                            int timeout = true ? 525600 : 1;
                            var ticket = new FormsAuthenticationTicket(1, acc2.login.username, DateTime.Now, DateTime.Now.AddMinutes(timeout), true, accountType);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                           

                                if (1.Equals(v.Role))
                                {
                                    return RedirectToAction("TownPanelOverview", "TownAccount");
                                }
                                else if (2.Equals(v.Role))
                                {

                                    return RedirectToAction("CHPanelOverview", "CHAccount");
                                }
                                else if (3.Equals(v.Role))
                                {
                                    return RedirectToAction("UCPanelOverview", "UCAccount");
                                }

                                else if (4.Equals(v.Role))
                                {
                                    return RedirectToAction("FOPanelOverview", "FOAccount");
                                }
                                else if (5.Equals(v.Role))
                                {
                                    return RedirectToAction("AccountProfile", "PublicAccount");
                                }
                                else if (0.Equals(v.Role))
                                {

                                    FormsAuthentication.SignOut();
                                    Session.Abandon();
                                    message = "no account found";
                                }
                                else
                                {
                                    FormsAuthentication.SignOut();
                                    Session.Abandon();
                                    message = "no account found";
                                }
                            
                        }
                        else
                        {
                            message = "invalid password provided";
                        }
                    }
                    else
                    {
                        message = "no account associated with this username!";
                    }
                }
                //TempData["Status"] = message;
                ViewBag.msg = message;
                return View();
            }
            catch (Exception er)
            {
                ViewBag.msg = "Error";
                return View();

            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Complaint");
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

        [NonAction]
        public void SendVerificationLinkEmail(String emailId, String activationCode, String fullName)
        {
            var verifyUrl = "/Account/VerifyAccount?id=" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("publiccomplaintportal@gmail.com", "PCP Karachi");
            var toEmail = new MailAddress(emailId);

            var fromEmailPassword = "szabist123";
            String subject = "Your account is successfully created!";

            String body = "<br/><br/> Dear " + fullName + "," +
                " your account has been created, in order to continue using this account you need to verify it. We have sent you the link to activate your account for further use " +
                "<br/><br/><a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })
                smtp.Send(message);

        }

        [Authorize]
        public ActionResult AccountCheckAndRedirect()
        {
            if (User.IsInRole("5"))
            {
                return RedirectToAction("AccountProfile", "PublicAccount");
            }
            else if (User.IsInRole("1"))
            {
                return RedirectToAction("TNManageAreaComplaints", "TownAccount", new { orderby = "severe", cat = "all" });
            }
            else if (User.IsInRole("3"))
            {
                return RedirectToAction("UCManageAreaComplaints", "UCAccount",new {orderby= "severe",cat="all" });
            }
            else if (User.IsInRole("2"))
            {
                return RedirectToAction("CHManageAreaComplaints", "CHAccount", new { orderby = "severe", cat = "all" });
            }
            else if (User.IsInRole("4"))
            {
                return RedirectToAction("FOManageAreaComplaints", "FOAccount", new { orderby = "severe"});
            }
            else if (User.IsInRole("0"))
            {
                return RedirectToAction("AdminPanel", "Admin");
            }

            else
            {
                return RedirectToAction("Index", "Complaint");
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ForgotPassword(vmPCP User)
        {
            string message = "";
            using (PCP5Entities pc = new PCP5Entities())
            {
                var v = pc.Accounts.Where(a => a.username == User.acc.username).FirstOrDefault();
                if (v == null)
                {
                    ModelState.AddModelError("AccountUserNotFound", "No Account found!");
                    return View();

                }
                else
                {

                    sendPasswordResetLink(v.email_address, v.ActivationCode.ToString(), v.Full_name);
                    String usern = v.username;
                    return RedirectToAction("ForgotPasswordMailSent",new { username = usern });

                }
            }


        }
        public ActionResult ForgotPasswordMailSent(String username) {

            vmPCP vm = new vmPCP();
             vm.acc = db.Accounts.Where(a=>a.username.Equals(username)).FirstOrDefault();
            return View(vm);

        }
        [HttpGet]
        public ActionResult AccountPasswordReset(String activationCode)
        {

            bool Status = false;
            using (PCP5Entities pcd = new PCP5Entities())
            {
                pcd.Configuration.ValidateOnSaveEnabled = false;
                vmPCP vm = new vmPCP();
                vm.acc= pcd.Accounts.Where(a => a.ActivationCode == new Guid(activationCode)).FirstOrDefault();
                if (vm.acc != null)
                {
                    Status = true;
                    return View(vm);

                }
                else
                {

                    ViewBag.Message = "Invalid Request";
                    return View();
                }
            }
            
        }
        [HttpPost]
        public ActionResult AccountPasswordReset(vmPCP vm)
        {
            try
            {
                using (PCP5Entities pcc = new PCP5Entities())
                {
                    Account acc = new Account();
                    acc = pcc.Accounts.Where(a => a.email_address.Equals(vm.acc.email_address)).FirstOrDefault();
                    acc.password = crypto.Hash(vm.accountReg.password);

                    pcc.Accounts.Attach(acc);
                    var entry = pcc.Entry(acc);

                    entry.Property(e => e.password).IsModified = true;
                    
                    pcc.SaveChanges();

                    //send email to user
                    SendEmailPasswordResetProcessComplete(acc.email_address, acc.Full_name);

                    TempData["Status"] = "Password Reset Success!";
                }
            }
            catch (Exception ec)
            {

                TempData["Status"] = "Error!";
            }

                return RedirectToAction("Login");
        }

        [NonAction]
        public void SendEmailPasswordResetProcessComplete(String emailId, String fullName)
        {
            var verifyUrl = "/Account/Login";
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("publiccomplaintportal@gmail.com", "PCP Pakistan");
            var toEmail = new MailAddress(emailId);

            var fromEmailPassword = "szabist123";
            String subject = "Password Changed!";

            String body = "<br/><br/> Dear " + fullName + "," +
                " Your password has been changed, if this action is not done by you please report immediately and change your email account password" +
                "<br/><br/><a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })
                smtp.Send(message);

        }
    
        [NonAction]
        public void sendPasswordResetLink(String emailId, String activationCode, String fullName)
        {
            var verifyUrl = "/Account/AccountPasswordReset?activationCode=" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("publiccomplaintportal@gmail.com", "PCP Pakistan");
            var toEmail = new MailAddress(emailId);

            var fromEmailPassword = "szabist123";
            String subject = "Password Reset!";

            String body = "<br/><br/> Dear " + fullName + "," +
                " We have recieved password reset request for your account, if you wish to continue with the reset request, go to this url " +
                "<br/><br/><a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })
                smtp.Send(message);

        }
    }
}