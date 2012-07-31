using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.Health;
using Microsoft.Health.Web;
using Microsoft.Health.Web.Authentication;
using Samples.HvMvc.Models;

namespace Samples.HvMvc.Areas.api.Controllers
{
    /// <summary>
    /// Account controller for doctors, primarly for win 8 app so they can access all users in the system
    /// </summary>
    public class DoctorAccountController : Controller
    {
        /// <summary>
        /// Logins a user via a http post
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>status</returns>
        [HttpPost]
        public ActionResult Login(string name, string password)
        {
            var status = "The user name or password provided is incorrect.";
            var key = "";
            if (Membership.ValidateUser(name, password))
            {
                FormsAuthentication.SetAuthCookie(name, true);
                status = "ok";
            }

            // If we got this far, something failed, redisplay form
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Helper method for WinJS app to check authorization
        /// </summary>
        /// <param name="val"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [AuthorizeRole(Roles = "Doctor")]
        [HttpPost]
        public ActionResult Ping()
        {
            return Json(new { status = "ok" }, JsonRequestBehavior.AllowGet);
        }



        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

#if DEBUG
        /// <summary>
        /// If you deploy this to production you should use ssl as passwords will be plain text
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult CreateDoctorUser(string name, string password, string email)
        {
            MembershipCreateStatus status;
            var user = Membership.CreateUser(name, password, email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out status);

            if (status == MembershipCreateStatus.Success)
            {
                return Json(new { status = status.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = ErrorCodeToString(status) }, JsonRequestBehavior.AllowGet);
            }
        }

        private const string DOCTOR_ROLE = "Doctor";
        public ActionResult AddUserToDoctorRole(string username)
        {
            return null;
            // enable rolemanager to use this
            //if (!Roles.RoleExists("Doctor"))
            //    Roles.CreateRole("Doctor");

            //try
            //{
            //    Roles.AddUserToRole(username, DOCTOR_ROLE);
            //    return Json(new { status = "ok" }, JsonRequestBehavior.AllowGet);
            //}
            //catch(Exception e)
            //{
            //    return Json(new { status = e.Message }, JsonRequestBehavior.AllowGet);
            //}
        }
#endif
    }
}
