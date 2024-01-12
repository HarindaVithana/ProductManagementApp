using MyAZHRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using MyAZHRM.Helpers;

namespace MyAZHRM.Helpers
{
    public class ForgotPassword
    {
        Encryption encryption = new Encryption();
        
        public string SendEmailNotification(string strEmail)
        {
            // Get Customer Name
            CustomerModel customer = GetCustomerName(strEmail);
            string retMsg = string.Empty;
            try
            {
                if (customer.Name == "" || customer.Name == null)
                {
                    throw new Exception("User not found");
                }
                else
                {
                    // Web Config set URL 
                    string strURL = string.Empty;
                    var Request = HttpContext.Current.Request;

                    string strEndTime = DateTime.Now.AddMinutes(Globals.EXPTIMER).ToString("yyyy/MM/dd HH:mm:ss");
                    // Expire time to DB
                    UpdateExpireTimer(strEndTime, customer.Id);

                    // Set Expire Time
                    strEndTime = encryption.EncryptedKey(DateTime.Now.AddMinutes(Globals.EXPTIMER).ToString("yyyy/MM/dd HH:mm:ss"));

                    string strSubject = string.Empty;
                    string strBody = string.Empty;
                    string strToMail = string.Empty;
                    string strCCMail = string.Empty;

                    strSubject = "MyAZHRM - Change User Credential...!";
                    strURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/Account/ResetPassword?T=" + strEndTime + "&id=" + encryption.EncryptedKey(customer.Id.ToString());

                    strBody = "Dear " + customer.Name.ToString() + ", <br /> <br /> Please <a href='" + strURL + "'>click here </a> to reset password. <br /> <br /> Thank You. <br /> <br />";
                    strToMail = strEmail;
                    strCCMail = string.Empty;

                    EmailNotification objEmailNotify = new EmailNotification();
                    var thread1 = new Thread(() => objEmailNotify.SendingEmail(strToMail, strBody, strSubject, strCCMail));
                    thread1.Start();

                    
                    retMsg = "Success";
                }
            }
            catch (Exception e)
            {
                retMsg = e.Message;
            }

            return retMsg;
        }

        public CustomerModel GetCustomerName(string email)
        {
            CustomerModel customer = new CustomerModel();
            Common objCommon = new Common();

            string strProcName = "SEL_CUSTOMER_NAME";
            QueryParams qryPrmCustEmail = new QueryParams() { Name = "@Email", Value = email };

            List<QueryParams> lstPrms = new List<QueryParams>()
            {
                qryPrmCustEmail
            };

            string strReturnCode = string.Empty;
            string strMessage = string.Empty;
            bool IsSuccess = false;

            System.Data.DataSet dtSet = objCommon.GetData(strProcName, lstPrms, ref strReturnCode, ref strMessage, ref IsSuccess);
            if (IsSuccess == true)
            {
                System.Data.DataTable dtTbl = dtSet.Tables[0];
                if (dtTbl.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow dtRw in dtTbl.Rows)
                    {
                        customer.Name = dtRw["Name"].ToString();
                        customer.Id = (int)dtRw["Id"];
                    }
                }
            }

            return customer;
        }

        public void UpdateExpireTimer(string ExpTime, int UserId)
        {
            Common objCommon = new Common();
            string Procedure = "UPD_CUSTOMER_EXPTIME";
            QueryParams idparam = new QueryParams() { Name = "@Id", Value = UserId.ToString() };
            QueryParams exparam = new QueryParams() { Name = "@ExpTime", Value = ExpTime.ToString() };

            List<QueryParams> paramList = new List<QueryParams>()
            {
                idparam,
                exparam
            };

            string strReturnCode = string.Empty;
            string strMessage = string.Empty;
            int intLastRecord = 0;

            // Instert to db
            bool upsert = objCommon.UPSERT(Procedure, paramList, ref strReturnCode, ref strMessage, ref intLastRecord);

        }

    }
}