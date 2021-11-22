using Schaeffler.Core.Encryption;
using Schaeffler.Domain;
using Schaeffler.Helpers;
using Schaeffler.Persistence.Interface;
using Schaeffler.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Schaeffler.Web.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        #region Constructor and Private Members

        private IUserAccountDao _userAccountDao;
        private IUtilityService _utilityService;
        public LoginController(IUserAccountDao userAccountDao, IUtilityService utilityService)
        {
            _userAccountDao = userAccountDao;
            _utilityService = utilityService;
        }


        #endregion
        // GET: Admin/Login
        public ActionResult Index()
        {
            //string Password = AESEncryption.DecryptString("of2jMzLE9R5VGsr2Taj36XlPCdXhn3OUTUMoPqYQWBk=");
            if (Session["UserAccount"] != null)
            {
                User U = ((User)Session["UserAccount"]);

                if (U != null)
                {
                    U.SystemIp = GetIp();

                    _userAccountDao.UpdateUserLoginLog(U);
                }
            }

            Session.Abandon();
            Response.Cookies.Clear();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User UA)
        {
            if (UA != null)
            {
                #region Validation

                List<ValidationParam> loginParam = new List<ValidationParam>{
                        new ValidationParam{
                            PropertyName="Email",
                            Value=UA.Email,
                            Type=typeof(string),
                            Methodologies=new Dictionary<ValidationMethodology,string>{
                               {ValidationMethodology.Required,null}
                            }
                        },
                        new ValidationParam{
                            PropertyName="Password",
                            Value=UA.Password,
                            Type=typeof(string),
                            Methodologies=new Dictionary<ValidationMethodology,string>{
                               {ValidationMethodology.Required,null}
                            }
                        }
                };

                Validator.Validate(loginParam, ModelState);
                #endregion

                if (Convert.ToString(Session["Captcha"]) == UA.Captcha)
                {
                    if (ModelState.IsValid)
                    {
                        UA.SecurityCode = string.Empty;

                        UA.Password = AESEncryption.EncryptString(UA.Password);

                        UA.SystemIp = GetRemoteIp.GetIPAddress(HttpContext);

                        string EncDetail = AESEncryption.EncryptString(UA.Email + "|" + UA.Password + "|" + new Guid().ToString());

                        UA = _userAccountDao.AuthenticateUser(UA);

                        if (UA != null && UA.SecurityCode == "BLOCKED")
                        {
                            ModelState.AddModelError("Id", "Account is locked");
                        }
                        else if (UA == null)
                        {
                            ModelState.AddModelError("Id", "Invalid username or password");
                        }
                        else if (UA != null)
                        {

                            UA.SystemIp = GetRemoteIp.GetIPAddress(HttpContext);

                            UA.LastLoginStatus = _userAccountDao.CheckLoginStatus(Guid.Empty, UA.GUID);

                            if (UA.LastLoginStatus)
                            {
                                ModelState.AddModelError("Id", "User Already having an open session.");
                                ViewBag.LogDetails = EncDetail;
                            }
                            else
                            {
                                Guid SessionId;

                                if (_userAccountDao.SaveUserLoginLog(UA, out SessionId) > 0)
                                {
                                    UA.SessionId = SessionId;
                                    Session["UserAccount"] = UA;
                                    return RedirectToRoute("Home");
                                }
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("Id", "Invalid username or password");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Id", "Invalid username or password");
                    }
                }
                else
                {
                    ModelState.AddModelError("Id", "Enter Valid Captacha");
                }
            }
            return View(UA);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForceLogin(string EncDetail)
        {

            if (!string.IsNullOrEmpty(EncDetail))
            {

                string[] encDetailSplitUps = AESEncryption.DecryptString(EncDetail).Split('|');

                User UA = new User
                {
                    Email = encDetailSplitUps[0],
                    Password = encDetailSplitUps[1],
                    SystemIp = GetRemoteIp.GetIPAddress(HttpContext)
                };

                UA = _userAccountDao.AuthenticateUser(UA);

                UA.LastLoginStatus = true;

                _userAccountDao.UpdateUserLoginLog(UA);

                if (UA != null && UA.SecurityCode == "BLOCKED")
                {
                    ModelState.AddModelError("Id", "Account is locked");
                }
                else if (UA == null)
                {
                    ModelState.AddModelError("Id", "Invalid username or password");
                }
                else if (UA != null)
                {
                    UA.SystemIp = GetRemoteIp.GetIPAddress(HttpContext);

                    UA.LastLoginStatus = _userAccountDao.CheckLoginStatus(Guid.Empty, UA.GUID);

                    if (UA.LastLoginStatus)
                    {
                        ModelState.AddModelError("Id", "User Already having an open session.");
                        ViewBag.LogDetails = EncDetail;
                    }
                    else
                    {
                        Guid SessionId;

                        if (_userAccountDao.SaveUserLoginLog(UA, out SessionId) > 0)
                        {
                            UA.SessionId = SessionId;
                            Session["UserAccount"] = UA;
                            return RedirectToRoute("Home");
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("Id", "Invalid username or password");
                }
            }

            return View("Index");
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword FP)
        {
            dynamic error = null; bool result = false;

            if (FP != null && FP.user != null && !string.IsNullOrEmpty(FP.user.Email))
            {
                FP.SystemIp = GetRemoteIp.GetIPAddress(HttpContext);

                FP = _userAccountDao.SaveForgotPassword(FP);

                if (FP == null)
                {
                    error = "Please enter valid email.";
                }
                else
                {
                    StringBuilder contents = new StringBuilder();
                    FP.Key = AESEncryption.EncryptandEncodeUrl(FP.UniqueId + "_" + FP.user.GUID);
                    contents.Append(RenderRazorViewToString("_EDMForgotPassword", FP));

                    if (_utilityService.SendEmail("SCHAEFFLER - Reset Forgot Password", contents.ToString(), FP.user.Email, true, null) == "success")
                    {
                        result = true;
                    }
                    else
                    {
                        error = "Error occured. Please try again later";
                    }
                }
            }
            // return Json(new { Valid = (ModelState.IsValid), Success = result, Error = error }, JsonRequestBehavior.DenyGet);
            return Json(new { Valid = (ModelState.IsValid), Success = result, Error = error });
        }

        public ActionResult Logout()
        {
            if (Session["UserAccount"] != null)
            {
                User U = ((User)Session["UserAccount"]);

                if (U != null)
                {
                    U.SystemIp = GetRemoteIp.GetIPAddress(HttpContext);

                    _userAccountDao.UpdateUserLoginLog(U);
                }
            }

            Session.Abandon();

            Response.Cookies.Clear();

            return RedirectToRoute("Login");
        }

        #region Reset Password

        [HttpGet]
        public ActionResult ResetPassword(string EncDetail)
        {
            ForgotPassword FP = null;

            string errorMessage = "Invalid reset password link";

            if (!string.IsNullOrEmpty(EncDetail))
            {
                string data = AESEncryption.DecodeUrlandDecrypt(EncDetail);

                string[] verificationParams = data.Split('_');

                FP = _userAccountDao.VerifyForgotPasswordUniqueId(new ForgotPassword
                {
                    UniqueId = new Guid(verificationParams[0]),
                    user = new User
                    {
                        GUID = new Guid(verificationParams[1])
                    }
                });

                if (FP != null)
                {
                    errorMessage = (FP.IsDeleted) ? "Reset password link expired." : ((FP.IsChanged) ? "Reset password link used already." : null);

                    FP.Key = EncDetail;
                }
            }

            ViewBag.Error = errorMessage;

            return View(FP);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ForgotPassword FP)
        {
            if (FP != null && FP.user != null && (FP.user.NewPassword == FP.user.ConfirmPassword))
            {
                string data = AESEncryption.DecodeUrlandDecrypt(FP.Key);

                string[] verificationParams = data.Split('_');

                FP.UniqueId = new Guid(verificationParams[0]);

                FP.user.GUID = new Guid(verificationParams[1]);

                FP.user.Password = AESEncryption.EncryptString(FP.user.NewPassword);

                FP.SystemIp = GetRemoteIp.GetIPAddress(HttpContext);

                FP.user = _userAccountDao.UpdatePassword(FP);

                if (FP.user != null && !string.IsNullOrEmpty(FP.user.Email))
                {
                    StringBuilder contents = new StringBuilder();
                    contents.Append(RenderRazorViewToString("_EDMPasswordChangedAcknowledge", FP.user));

                    if (_utilityService.SendEmail("SCHAEFFLER- Reset Forgot Password", contents.ToString(), FP.user.Email, true, null) == "success")
                    {
                        ViewBag.Success = "Password updated successfully";
                    }
                    else
                    {
                        ModelState.AddModelError("Id", "Error occured. Please try again later");
                    }
                }
                else
                {
                    ViewBag.Error = "Error Occured";
                    FP = null;
                }
            }
            return View(FP);
        }

        #endregion

        #region Private Methods
        public JsonResult GetCaptcha()
        {
            return Json(new { captchaImage = Captcha.GetBase64(HttpContext), code = Convert.ToString(Session["Captcha"]) }, JsonRequestBehavior.AllowGet);
        }
        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

                viewResult.View.Render(viewContext, sw);

                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }
        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        #endregion
    }
}