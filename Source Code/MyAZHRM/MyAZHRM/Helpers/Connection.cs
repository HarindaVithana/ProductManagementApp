using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace MyAZHRM.Helpers
{
    public class Connection
    {

        public static SqlConnection GetConnection()
        {
            SqlConnection SqlCon = null;
            string strErrorMsg = string.Empty;
            string strConString = string.Empty;           
            try
            {
                strConString = ConfigurationManager.ConnectionStrings["MyAZHRM"].ConnectionString;
                if (string.IsNullOrEmpty(strConString) == true)
                {
                    throw new Exception("Connection String is null or empty. Cannot create database access.");
                }
                else
                {
                    SqlCon = new SqlConnection(strConString);
                    if (SqlCon == null)
                    {
                        throw new Exception("Connection not found. Cannot create database access.");
                    }
                    else
                    {
                        if (SqlCon.State != ConnectionState.Open)
                        {
                            SqlCon.Open();
                        }
                    }
                }        
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString().Trim();
            }

            return SqlCon;
        }

        public static void CloseConnection(SqlConnection SqlCon)
        {
            if (SqlCon.State == ConnectionState.Open)
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }

    }
}