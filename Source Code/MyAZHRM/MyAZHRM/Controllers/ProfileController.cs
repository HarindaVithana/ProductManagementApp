using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAZHRM.Helpers;
using MyAZHRM.Models;
using System.Web.Mvc;

namespace MyAZHRM.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            // 2023/Dec/07 
            CountryInfo objCountry = new CountryInfo();
            List<string> lstCountry = objCountry.getCountries();
            Session["CountryList"] = null;
            Session["CountryList"] = lstCountry;

            CurrencyInfo objCurrency = new CurrencyInfo();
            List<string> lstCurrency = objCurrency.getCurrencies();
            Session["CurrencyList"] = null;
            Session["CurrencyList"] = lstCurrency;

            ViewBag.Message = "Profile Controller";
            var user = Session["LoggedUsr"];
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public string UpdateProfile(int Id, string Name, string Phone, string Address, string Country, string Currency)
        {
            string returnMsg = "";
            Common common = new Common();
            CustomerModel userData = (CustomerModel)Session["LoggedUsr"];

            string Procedure = "UPD_CUSTOMER_DETAILS";
            QueryParams idparam = new QueryParams() { Name = "@Id", Value = Id };
            QueryParams nameparam = new QueryParams() { Name = "@Name", Value = Name };
            QueryParams phoneparam = new QueryParams() { Name = "@Phone", Value = Phone };
            QueryParams addrparam = new QueryParams() { Name = "@Address", Value = Address };
            QueryParams countparam = new QueryParams() { Name = "@Country", Value = Country };
            QueryParams currencyparam = new QueryParams() { Name = "@Currency", Value = Currency };

            List<QueryParams> paramList = new List<QueryParams>()
            {
                idparam,
                nameparam,
                phoneparam,
                addrparam,
                countparam,
                currencyparam
            };

            string strReturnCode = string.Empty;
            string strMessage = string.Empty;
            int intLastRecord = 0;
            // Attempt to register the user
            try
            {
                foreach (QueryParams param in paramList)
                {
                    if (param.Value == null || param.Value.ToString() == "")
                    {
                        throw new Exception("Invalid");           
                    }
                }
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
                    if (upsert)
                    {
                        GetCustomer(userData.Email, userData.Password);
                        returnMsg = "success";
                        RedirectToAction("Index", "Profile");
                    }
                }           
            }
            catch (Exception e)
            {
                returnMsg =  e.Message.ToString();
            }

            return returnMsg;
        }

        public void GetCustomer(string Email, string password)
        {
            var objCommon = new Common();
            if (ModelState.IsValid)
            {
                string strProcName = "SEL_CUSTOMER_LOGIN_DETAILS";
                QueryParams qryPrmEmail = new QueryParams() { Name = "@Email", Value = Email };
                QueryParams qryPrmPwd = new QueryParams() { Name = "@Password", Value = password };

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
                    }
                    else
                    {
                        throw new Exception(strMessage);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message.ToString().Trim());
                }
            }
        }

        public string UpdatePassword(string Current, string NewPass, string ConfPass)
        {
            var objCommon = new Common();
            LocalPasswordModel model = new LocalPasswordModel();
            string Message = "";
            CustomerModel userData = (CustomerModel)Session["LoggedUsr"];

            if (objCommon.EncodePassword(Current) != userData.Password)
            {
                Message = "Error";
            }
            else if (objCommon.EncodePassword(NewPass) == userData.Password)
            {
                Message = "invalid";
            }
            else
            {
                string Procedure = "UPD_CUSTOMER_PASSWORD";
                QueryParams idparam = new QueryParams() { Name = "@Id", Value = userData.Id };
                QueryParams pwparam = new QueryParams() { Name = "@Password", Value = objCommon.EncodePassword(NewPass) };

                List<QueryParams> paramList = new List<QueryParams>()
                {
                    idparam,
                    pwparam
                };

                string strReturnCode = string.Empty;
                string strMessage = string.Empty;
                int intLastRecord = 0;
                // Attempt to register the user
                try
                {
                    // Instert to db
                    bool upsert = objCommon.UPSERT(Procedure, paramList, ref strReturnCode, ref strMessage, ref intLastRecord);
                    if (upsert)
                    {
                        GetCustomer(userData.Email, userData.Password);
                        RedirectToAction("Index", "Profile");
                        Message = "Password updated successfullly";
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message.ToString());
                }

            }

            return Message;
        }
    }
}
