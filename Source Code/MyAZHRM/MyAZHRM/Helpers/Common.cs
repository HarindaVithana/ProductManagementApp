using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using MyAZHRM.Models;
using System.Security.Cryptography;
using System.Text;

namespace MyAZHRM.Helpers
{
    public class Common
    {
        private SqlCommand SqlCmd = null;
        private SqlConnection SqlCon = null;
        private DataTable dtTbl;
        private DataSet dtSet;
        private SqlDataAdapter dtAdpt;
        private SqlTransaction SqlTrans = null;



        // INSERT & UPDATE
        public bool UPSERT(string strProcedureName, List<QueryParams> lstParams, ref string strReturnCode, ref string strMessage, ref int intLastRecord)
        {
            bool IsSuccess = false;
            try
            {
                SqlCon = new SqlConnection();
                SqlCon = Connection.GetConnection();

                if (SqlCon.State != ConnectionState.Open)
                {
                    SqlCon.Open();
                }

                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlTrans = SqlCon.BeginTransaction();
                }

                SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlCon;
                SqlCmd.CommandText = strProcedureName;
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Transaction = SqlTrans;

                if (lstParams.Count() > 0)
                {
                    foreach (QueryParams lstItm in lstParams)
                    {
                        SqlParameter SqlParam = new SqlParameter(lstItm.Name, lstItm.Value);
                        SqlCmd.Parameters.Add(SqlParam);
                    }
                }

                SqlCmd.Parameters.Add("@InsertedID", SqlDbType.Int).Direction = ParameterDirection.Output;
                SqlCmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;

                SqlCmd.ExecuteNonQuery();

                string strErMsg = SqlCmd.Parameters["@ErrorMessage"].Value.ToString().Trim();
                if (string.IsNullOrEmpty(strErMsg) == false)
                {
                    IsSuccess = false;
                    throw new Exception(strErMsg);
                }
                else
                {
                    intLastRecord = Convert.ToInt32(SqlCmd.Parameters["@InsertedID"].Value);
                    IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                strReturnCode = "Error";
                strMessage = ex.Message;
            }
            finally
            {
                if (IsSuccess == true)
                {
                    SqlTrans.Commit();
                    strReturnCode = "OK";
                    strMessage = "Action Successful.";
                }
                else
                {
                    SqlTrans.Rollback();
                    strReturnCode = "Error";
                    //strMessage = "Action Unsuccessful.";
                }

                if (SqlCon != null)
                {
                    if (SqlCon.State == ConnectionState.Open)
                    {
                        SqlCon.Close();
                    }
                }   
            }

            return IsSuccess;
        }



        // DELETE
        public bool DELETE(string strProcedureName, List<QueryParams> lstParams, ref string strReturnCode, ref string strMessage)
        {
            bool IsSuccess = false;
            try
            {
                SqlCon = new SqlConnection();
                SqlCon = Connection.GetConnection();

                if (SqlCon.State != ConnectionState.Open)
                {
                    SqlCon.Open();
                }

                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlTrans = SqlCon.BeginTransaction();
                }

                SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlCon;
                SqlCmd.CommandText = strProcedureName;
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Transaction = SqlTrans;

                if (lstParams.Count() > 0)
                {
                    foreach (QueryParams lstItm in lstParams)
                    {
                        SqlParameter SqlParam = new SqlParameter(lstItm.Name, lstItm.Value);
                        SqlCmd.Parameters.Add(SqlParam);
                    }
                }

                SqlCmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;

                SqlCmd.ExecuteNonQuery();

                string strErMsg = SqlCmd.Parameters["@ErrorMessage"].Value.ToString().Trim();
                if (string.IsNullOrEmpty(strErMsg) == false)
                {
                    IsSuccess = false;
                    throw new Exception(strErMsg);
                }
                else
                {
                    IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                strReturnCode = "Error";
                strMessage = ex.Message;
            }
            finally
            {
                if (IsSuccess == true)
                {
                    SqlTrans.Commit();
                    strReturnCode = "OK";
                    strMessage = "Action Successful.";
                }
                else
                {
                    SqlTrans.Rollback();
                    strReturnCode = "Error";
                    //strMessage = "Action Unsuccessful.";
                }

                if (SqlCon != null)
                {
                    if (SqlCon.State == ConnectionState.Open)
                    {
                        SqlCon.Close();
                    }
                }
            }

            return IsSuccess;
        }



        // GET DATA
        public DataSet GetData(string strProcedureName, List<QueryParams> lstParams, ref string strReturnCode, ref string strMessage, ref bool IsSuccess)
        {
            dtSet = new DataSet();
            try
            {
                SqlCon = new SqlConnection();
                SqlCon = Connection.GetConnection();

                if (SqlCon.State != ConnectionState.Open)
                {
                    SqlCon.Open();
                }

                SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlCon;
                SqlCmd.CommandText = strProcedureName;
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (lstParams.Count() > 0)
                {
                    foreach (QueryParams lstItm in lstParams)
                    {
                        SqlParameter SqlParam = new SqlParameter(lstItm.Name, lstItm.Value);
                        SqlCmd.Parameters.Add(SqlParam);
                    }
                }

                SqlCmd.Parameters.Add("@Message", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;
                dtAdpt = new SqlDataAdapter(SqlCmd);
                dtAdpt.Fill(dtSet);

                string strErMsg = SqlCmd.Parameters["@Message"].Value.ToString().Trim();
                if (strErMsg.ToString().Trim() == Globals.SUCCESS_OK.ToString().Trim())
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    throw new Exception(strErMsg);
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                strReturnCode = "Error";
                strMessage = ex.Message;
            }
            finally
            {
                if (IsSuccess == true)
                {
                    strReturnCode = "OK";
                    strMessage = "Action Successful.";
                }
                else
                {
                    strReturnCode = "Error";
                    //strMessage = "Action Unsuccessful.";
                }

                if (SqlCon != null)
                {
                    if (SqlCon.State == ConnectionState.Open)
                    {
                        SqlCon.Close();
                    }
                }
            }

            return dtSet;
        }



        // Encrypt Password 
        public string EncodePassword(string originalPassword)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes);
        }



        // Checking Whether user input is a number
        public bool IsNumber(string strInput)
        {
            int intValue = 0;
            if (int.TryParse(strInput, out intValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}