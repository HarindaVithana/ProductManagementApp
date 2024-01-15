using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using MyAZHRM.Models;
using System.Net.Http;
using System.Web;

namespace MyAZHRM.Helpers
{
    public class EmailNotification
    {

        public void SendingEmail(string strToMail, string strBody, string strSubject, string strCCMail = "")
        {
            if (string.IsNullOrEmpty(strToMail) == true)
            {
                return;
            }

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["EMAIL_FROM"].ToString().Trim(), ConfigurationManager.AppSettings["EMAIL_DISPLAY_NAME"].ToString().Trim());
            mailMsg.To.Add(new MailAddress(strToMail.ToString().Trim()));
            mailMsg.Subject = strSubject;
            mailMsg.IsBodyHtml = true;
            mailMsg.Body = strBody;
            if (string.IsNullOrEmpty(strCCMail) == false)
            {
                mailMsg.CC.Add(strCCMail);
            }

            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EMAIL_FROM"].ToString().Trim(), ConfigurationManager.AppSettings["EMAIL_FROM_PWD"].ToString().Trim());

            // 2023/Oct/19
            // smtp.Port = 587;
            // smtp.Host = "smtp.gmail.com";
            // smtp.EnableSsl = true;
            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["EMAIL_PORT"].ToString().Trim());
            smtp.Host = ConfigurationManager.AppSettings["EMAIL_HOST"].ToString().Trim();
            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EMAIL_ENABLE_SSL"].ToString().Trim());

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;           
            smtp.Send(mailMsg);
        }

    }
}
