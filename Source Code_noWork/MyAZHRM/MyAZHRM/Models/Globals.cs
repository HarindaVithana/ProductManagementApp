using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public static class Globals
    {
        public static string DEFAULT_DATE = "1900-01-01";
        public static string SUCCESS_OK = "OK";
        public static int EXPTIMER = 5;

        // Customer Product Upgrading Status 
        public static string CUS_PRD_PENDING = "PENDING";
        public static string CUS_PRD_CANCEL_REQ_PENDING = "PENDING CANCEL REQUEST";   // AddProduct.cshtml -> disableButtons();

        // Activity 
        public static string ACT_CUS_CAN_REQ = "Request to cancel package";

        // 2023/Dec/12 - Remember Me
        public static int REMEMBER_ME_MINUTES = 3;

    }
}