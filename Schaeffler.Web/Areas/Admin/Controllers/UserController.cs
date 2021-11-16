using Schaeffler.Core.Encryption;
using Schaeffler.Domain;
using Schaeffler.Filters;
using Schaeffler.Helpers;
using Schaeffler.Persistence.Interface;
using Schaeffler.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schaeffler.Web.Areas.Admin.Controllers
{
    [AuthenticationFilter]
    [HandleError]
    public class UserController : Controller
    {
        #region Private variables and constructors

        private IUserDao _userDao;
        private IUtilityService _utilityService;


        public UserController(IUserDao userDao, IUtilityService utilityService)
        {
            _userDao = userDao;
            _utilityService = utilityService;
        }
        #endregion
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(User UA)
        {
            if (UA != null)
            {
                #region Validation

                List<ValidationParam> profileParam = new List<ValidationParam>{
                        new ValidationParam{
                            PropertyName="Password",
                            Value =UA.Password,
                            Type=typeof(string),
                            Methodologies = new Dictionary<ValidationMethodology,string> {
                                {ValidationMethodology.Required,null}
                            }
                        },
                        new ValidationParam{
                            PropertyName="NewPassword",
                            Value=UA.NewPassword,
                            Type=typeof(string),
                            Methodologies = new Dictionary<ValidationMethodology,string> {
                                {ValidationMethodology.Required,null},
                                {ValidationMethodology.MinLength,"password must be at least 8 characters length"},
                                {ValidationMethodology.RegularExpression,"password should contain atleast one number and one special character" }
                            },
                            Params=new Dictionary<ValidationMethodology,dynamic>
                            {
                                { ValidationMethodology.MinLength,8},
                                { ValidationMethodology.RegularExpression,@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$"}
                            }
                        },
                        new ValidationParam{
                            PropertyName="ConfirmPassword",
                            Value=UA.ConfirmPassword,
                            Type=typeof(string),
                            Methodologies = new Dictionary<ValidationMethodology,string> {
                                {ValidationMethodology.Required,null},
                                {ValidationMethodology.EqualTo,"Please enter the same value as NewPassword"}
                            },
                            Params=new Dictionary<ValidationMethodology, dynamic>
                            {
                                { ValidationMethodology.EqualTo,UA.NewPassword}
                            }
                        }
                };

                Validator.Validate(profileParam, ModelState);

                #endregion

                if (ModelState.IsValid)
                {
                    if (UA != null && UA.Password != null && UA.Password.Length >= 8 && UA.NewPassword != null && UA.NewPassword.Length >= 8)
                    {
                        UA.SystemIp = GetRemoteIp.GetIPAddress(HttpContext);

                        UA.GUID = ((User)Session["UserAccount"]).GUID;

                        UA.Password = AESEncryption.EncryptString(UA.Password);

                        UA.NewPassword = AESEncryption.EncryptString(UA.NewPassword);

                        ViewBag.Success = _userDao.ChangePassword(UA) > 0;
                    }
                }
            }
            return View();
        }
    }
}