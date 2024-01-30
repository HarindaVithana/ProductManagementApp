using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MyAZHRM.Models;
using MyAZHRM.Helpers;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.SessionState;


namespace MyAZHRM.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private Common objCommon = null;

        //Forget Password in login
        //2023/11/15--
        [AllowAnonymous]
        public ActionResult forgetpassword()
        {            
            return View();
        }
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            // 2023/Dec/12 - Remember Me
            LoginModel model = new LoginModel();
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            { 
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket.Expired == false)
                {
                    model.logEmail = ticket.Name.ToString().Trim();
                    model.Password = ticket.UserData.ToString().Trim();
                }
            }

            return View(model);
        }

      

        
        // POST: /Account/Login
        //2023/11/16--
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            objCommon = new Common();
            if (ModelState.IsValid)
            {
                string strProcName = "SEL_CUSTOMER_LOGIN_DETAILS";
                QueryParams qryPrmEmail = new QueryParams() { Name = "@Email", Value = model.logEmail };
                QueryParams qryPrmPwd = new QueryParams() { Name = "@Password", Value = objCommon.EncodePassword(model.Password) };

                List<QueryParams> lstPrms = new List<QueryParams>()
                {
                    qryPrmEmail,
                    qryPrmPwd
                };

                string strReturnCode = string.Empty;
                string strMessage = string.Empty;
                bool IsSuccess = false;

                try
                {
                    System.Data.DataSet dtSet = objCommon.GetData(strProcName, lstPrms, ref strReturnCode, ref strMessage, ref IsSuccess);
                    if (IsSuccess == true)
                    {
                        System.Data.DataTable dtTbl = dtSet.Tables[0];
                        Session["LoggedUsr"] = null;

                        CustomerModel objCustomer = new CustomerModel();
                        if (dtTbl.Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow dtRw in dtTbl.Rows)
                            {
                                objCustomer.Id = (int)dtRw["Id"];
                                objCustomer.Name = dtRw["Name"].ToString().Trim();
                                objCustomer.Email = dtRw["Email"].ToString().Trim();
                                objCustomer.Phone = dtRw["Phone"].ToString().Trim();
                                objCustomer.Address = dtRw["Address"].ToString().Trim();
                                objCustomer.Country = dtRw["Country"].ToString().Trim();
                                objCustomer.Currency = dtRw["Currency"].ToString().Trim();
                                objCustomer.Password = dtRw["Password"].ToString().Trim();
                                objCustomer.RegDate = Convert.ToDateTime(dtRw["RegDate"]);
                                objCustomer.DeactiveDate = Convert.ToDateTime(dtRw["DeactiveDate"]);
                                objCustomer.Status = dtRw["Status"].ToString().Trim();
                            }
                        }

                        Session["LoggedUsr"] = objCustomer;

                        if (model.RememberMe)
                        {
                            //create the authentication ticket
                            var authTicket = new FormsAuthenticationTicket(1, model.logEmail.ToString().Trim(), DateTime.Now, DateTime.Now.AddMinutes(Globals.REMEMBER_ME_MINUTES), false, model.Password, "/");

                            //encrypt the ticket and add it to a cookie
                            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                            Response.Cookies.Add(cookie);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new MembershipCreateUserException(strMessage);
                    }
                }
                catch (MembershipCreateUserException ex)
                {
                    ModelState.AddModelError("exception", ex.Message.ToString().Trim());
                }
            }

            // If we got this far, something failed, redisplay form
            // ModelState.AddModelError("", "Login Failed. Please try again.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [AllowAnonymous]
        //[HttpGet]
        public ActionResult LogOff()
        {
            //FormsAuthentication.SignOut();
            //WebSecurity.Logout();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterModel model = new RegisterModel();
            CountryInfo objCountry = new CountryInfo();
            List<string> lstCountry = objCountry.getCountries();
            Session["CountryList"] = null;
            Session["CountryList"] = lstCountry;

            CurrencyInfo objCurrency = new CurrencyInfo();
            List<string> lstCurrency = objCurrency.getCurrencies();
            Session["CurrencyList"] = null;
            Session["CurrencyList"] = lstCurrency;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult RegisterCountryList(string Prefix)
       {
            CountryInfo countries = new CountryInfo();
            //15/11/2023
            List<string> countryList = countries.getCountries();
            List<string> LstMatch = new List<string>();

            if (string.IsNullOrEmpty(Prefix) == false && string.IsNullOrWhiteSpace(Prefix) == false)
            {
                LstMatch = countryList.Where(x => x.ToLower().Contains(Prefix.ToLower())).ToList();
            }

            // List<string> Loc = cList.Where(x => x.StartsWith(Prefix.ToLower())).Select(x => x.ToString()).ToList();
            return Json(LstMatch, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult RegisterCurrencyList(string Prefix)
        {
            CurrencyInfo currencies = new CurrencyInfo();
            //15/11/2023
            List<string> currencyList = currencies.getCurrencies();
            List<string> LstMatch = new List<string>();

            if (string.IsNullOrEmpty(Prefix) == false && string.IsNullOrWhiteSpace(Prefix) == false)
            {
                LstMatch = currencyList.Where(x => x.ToLower().Contains(Prefix.ToLower())).ToList();
            }

            return Json(LstMatch, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            Common common = new Common();

            if (ModelState.IsValid)
            {
                // preconditions
                string Procedure = "INS_CUSTOMER_DETAILS";
                QueryParams nameparam = new QueryParams() { Name = "@Name", Value = model.UserName };
                QueryParams emailparam = new QueryParams() { Name = "@Email", Value = model.Email };
                QueryParams phoneparam = new QueryParams() { Name = "@Phone", Value = model.Telephone };
                QueryParams addrparam = new QueryParams() { Name = "@Address", Value = model.CompanyAddress };
                QueryParams countparam = new QueryParams() { Name = "@Country", Value = model.Country };
                QueryParams currencyparam = new QueryParams() { Name = "@Currency", Value = model.Currency };
                QueryParams passparam = new QueryParams() { Name = "@Password", Value = common.EncodePassword(model.Password) };
                QueryParams regdateparam = new QueryParams() { Name = "@RegDate", Value = DateTime.Now };
                QueryParams defdateparam = new QueryParams() { Name = "@DeactiveDate", Value = Globals.DEFAULT_DATE };
                //QueryParams insidparam = new QueryParams() { Name = "@InsertedID", Value = 0};

                List<QueryParams> paramList = new List<QueryParams>()
                {
                    nameparam,
                    emailparam,
                    phoneparam,
                    addrparam,
                    countparam,
                    currencyparam,
                    passparam,
                    regdateparam,
                    defdateparam
                    //,insidparam
                };

                string strReturnCode = string.Empty;
                string strMessage = string.Empty;
                int intLastRecord = 0;
                // Attempt to register the user
                try
                {
                    CountryInfo cInfo = new CountryInfo();
                    CurrencyInfo curInfo = new CurrencyInfo();

                    List<string> Countries = cInfo.getCountries();
                    List<string> Currencies = curInfo.getCurrencies();

                    if (!Countries.Contains(countparam.Value.ToString()))
                    {
                        throw new Exception("Invalid Country");
                    }
                    else if (!Currencies.Contains(currencyparam.Value.ToString()))
                    {
                        throw new Exception("Invalid Currency");
                    }
                    else
                    {
                        // Instert to db
                        bool upsert = common.UPSERT(Procedure, paramList, ref strReturnCode, ref strMessage, ref intLastRecord);

                        Session["ReturnCode"] = null;
                        Session["ErrorMsg"] = null;

                        if (upsert == true)
                        {
                            model.Message = "Success";
                            Session["ReturnCode"] = "Success";
                        }
                        else
                        {
                            model.Message = "Error";
                            Session["ReturnCode"] = "Error";
                            Session["ErrorMsg"] = strMessage;
                            if (strMessage == "Record already Exist")
                            {
                                Session["ErrorMsg"] = "Another User already registered using this email";
                            }
                        }
                                                                     
                        // return RedirectToAction("Login", "Account");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("exception", e.Message.ToString());
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        //password reset email
        public ActionResult SendEmailNotification(string Email)
        {
            ForgotPassword objFgtPwd = new ForgotPassword();
            var retmsg = objFgtPwd.SendEmailNotification(Email);

            if (retmsg == "User not found")
            {
                ModelState.AddModelError("email", retmsg);
            }

            return Json(new { returnMsg = retmsg });
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

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
    
        // Forgot password Reset page
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View("_ChangePasswordPartial");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ResetPassword(string Password, string UserId, string Exptime)
        {
            string returnMsg = "";
            Encryption encryption = new Encryption();
            objCommon = new Common();

            try
            {
                if (ValidateReset(UserId, Exptime, ref returnMsg))
                {
                    Exptime = encryption.DecryptedKey(Exptime);
                    UserId = encryption.DecryptedKey(UserId);

                    string Procedure = "UPD_CUSTOMER_PASSWORD";
                    QueryParams idparam = new QueryParams() { Name = "@Id", Value = UserId };
                    QueryParams pwparam = new QueryParams() { Name = "@Password", Value = objCommon.EncodePassword(Password) };

                    List<QueryParams> paramList = new List<QueryParams>()
                    {
                        idparam,
                        pwparam
                    };

                    string strReturnCode = string.Empty;
                    string strMessage = string.Empty;
                    int intLastRecord = 0;

                    // Instert to db
                    bool upsert = objCommon.UPSERT(Procedure, paramList, ref strReturnCode, ref strMessage, ref intLastRecord);
                    if (upsert)
                    {
                        returnMsg = "success";
                    }
                } 
            }
            catch (Exception e)
            {
                returnMsg = e.Message;
            }

            return Json(new { returnMsg = returnMsg });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckValidity(string UserId, string Exptime)
        {
            string returnMsg = "";
            ValidateReset(UserId, Exptime, ref returnMsg);
            
            return Json(new { returnMsg = returnMsg });
        }

        public bool ValidateReset(string UserId, string Exptime, ref string returnMsg)
        {
            Encryption encryption = new Encryption();
            bool isvalid = true;
            string Message = "";
            DateTime dateTime;


            if (UserId == null || UserId == "" )
            {
                Message = "Invalid User Id";
                isvalid = false;
            }
            else if(Exptime == null || Exptime == "")
            {
                Message = "Invalid Expiration Date";
                isvalid = false;
            }
            else
            {
                try
                {
                    Exptime = encryption.DecryptedKey(Exptime);
                    UserId = encryption.DecryptedKey(UserId);
                }
                catch(Exception e)
                {
                    Message = "Invalid Id or Expiration Date";
                }
                if (DateTime.TryParse(Exptime, out dateTime))
                {
                    if (dateTime < DateTime.Now)
                    {
                        Message = "Link Expired";
                        isvalid = false;
                    }
                }
                else
                {
                    Message = "Invalid Expiration Date";
                    isvalid = false;
                }
            }
            returnMsg = Message;

            return isvalid;
        }
    }
}
