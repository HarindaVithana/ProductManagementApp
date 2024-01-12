using MyAZHRM.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;

namespace MyAZHRM.Helpers
{
    public class Products
    {
        private Common objCommon = null;



        // Get All Packages
        public List<PackageModulesModel> GetPackageDetails(CustomerModel objCustomer, int ProductId, ref string strErMsg)
        {
            List<PackageModulesModel> lstProducts = new List<PackageModulesModel>();
            try
            {
                if (objCustomer != null)
                {
                    objCommon = new Common();

                    string strProcName = "SEL_PACKAGE_DETAILS";

                    QueryParams qryPrmCustId = new QueryParams() { Name = "@CustomerId", Value = objCustomer.Id };
                    QueryParams qryPrmProdId = new QueryParams() { Name = "@ProductId", Value = ProductId };

                    List<QueryParams> lstPrms = new List<QueryParams>()
                    { 
                       qryPrmCustId,
                       qryPrmProdId
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
                                PackageModulesModel objProducts = new PackageModulesModel();
                                objProducts.Id = (int)dtRw["RecordId"];
                                objProducts.PackageId = (int)dtRw["PackageId"];
                                objProducts.ModuleId = (int)dtRw["ModuleId"];
                                objProducts.ModuleName = dtRw["ModuleName"].ToString().Trim();
                                objProducts.PackageName = dtRw["PackageName"].ToString().Trim();
                                objProducts.Currency = dtRw["Currency"].ToString().Trim();
                                objProducts.Amount = Convert.ToDecimal(dtRw["PackageAmt"]);
                                objProducts.TrailDays = (int)dtRw["TrailDays"];
                                objProducts.Status = dtRw["Status"].ToString().Trim();
                                lstProducts.Add(objProducts);
                            }
                        }
                        else
                        {
                            strErMsg = "No Data Found.";
                        }
                    }
                }
                else
                {
                    strErMsg = "No Data Found.";
                }
            }
            catch (Exception ex)
            {
                strErMsg = ex.Message.ToString().Trim();
            }

            return lstProducts;
        }



        // Get Customer All Products Summary
        public List<ProductsModel> GetCustomerAllProducts(CustomerModel objCustomer, ref string strErMsg)
        {
            List<ProductsModel> lstProducts = new List<ProductsModel>();
            try
            {
                if (objCustomer != null)
                {
                    objCommon = new Common();

                    string strProcName = "SEL_CUSTOMER_ALL_PRODUCTS_DETAILS";
                    QueryParams qryPrmCustId = new QueryParams() { Name = "@Id", Value = objCustomer.Id };

                    List<QueryParams> lstPrms = new List<QueryParams>()
                    {
                       qryPrmCustId
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
                                ProductsModel objProducts = new ProductsModel();
                                objProducts.Id = (int)dtRw["ProductId"];
                                objProducts.CompanyName = dtRw["CmpName"].ToString().Trim();
                                objProducts.EmployeeCount = (int)dtRw["EmpCount"];
                                objProducts.Currency = dtRw["Currency"].ToString().Trim();
                                objProducts.DueAmount = Convert.ToDecimal(dtRw["DueAmt"]);
                                objProducts.Status = dtRw["Status"].ToString().Trim();
                                objProducts.ActiveDate = Convert.ToDateTime(dtRw["ActiveDate"]);
                                objProducts.NextBillingDate = Convert.ToDateTime(dtRw["NextBillDate"]);
                                objProducts.CancelledDate = Convert.ToDateTime(dtRw["CancelDate"]);
                                objProducts.PackageId = (int)dtRw["PackageId"];
                                objProducts.PackageName = dtRw["PackageName"].ToString().Trim();                                
                                lstProducts.Add(objProducts);
                            }
                        }
                        else
                        {
                            strErMsg = "No Data Found.";
                        }
                    }
                }
                else 
                {
                    strErMsg = "No Data Found.";
                }
            }
            catch (Exception ex)
            {
                strErMsg = ex.Message.ToString().Trim();
            }

            return lstProducts;
        }



        // Get Customer Product Details by Customer Id & Package Id & Product Id
        public CustomerProductModel GetCustomerProductDetails(int intCustomerId, int intPackageId, int intProductId, ref string strErMsg)
        {
            CustomerProductModel objCustProduct = new CustomerProductModel();
            try
            {
                if (intCustomerId != 0 && intPackageId != 0)
                {
                    objCommon = new Common();

                    string strProcName = "SEL_CUSTOMER_PRODUCT_DETAILS";

                    QueryParams qryPrmCustId = new QueryParams() { Name = "@CustomerId", Value = intCustomerId };
                    QueryParams qryPrmPackId = new QueryParams() { Name = "@PackageId", Value = intPackageId };
                    QueryParams qryPrmProdId = new QueryParams() { Name = "@ProductId", Value = intProductId };

                    List<QueryParams> lstPrms = new List<QueryParams>()
                    {
                       qryPrmCustId,
                       qryPrmPackId,
                       qryPrmProdId
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
                                objCustProduct = new CustomerProductModel();
                                objCustProduct.CustomerId = (int)dtRw["CustomerId"];
                                objCustProduct.Country = dtRw["Country"].ToString().Trim();
                                objCustProduct.PackageId = (int)dtRw["PackageId"];
                                objCustProduct.PackageName = dtRw["PackageName"].ToString().Trim();
                                objCustProduct.TrailDays = (int)dtRw["TrailDays"];
                                objCustProduct.CompanyName = dtRw["CompanyName"].ToString().Trim();
                                objCustProduct.EmployeeCount = (int)dtRw["EmployeeCount"];
                                objCustProduct.Currency = dtRw["Currency"].ToString().Trim();
                                objCustProduct.DueAmount = Convert.ToDecimal(dtRw["DueAmount"]);
                                objCustProduct.Status = dtRw["Status"].ToString().Trim();
                                objCustProduct.ActiveDate = Convert.ToDateTime(dtRw["ActiveDate"]);
                                objCustProduct.NextBillingDate = Convert.ToDateTime(dtRw["NextBillingDate"]);
                                objCustProduct.CancelledDate = Convert.ToDateTime(dtRw["CancelledDate"]);
                                objCustProduct.ProductId = (int)dtRw["ProductId"];
                            }
                        }
                        else
                        {
                            strErMsg = "No Data Found.";
                        }
                    }
                }
                else
                {
                    strErMsg = "No Data Found.";
                }
            }
            catch (Exception ex)
            {
                strErMsg = ex.Message.ToString().Trim();
            }

            return objCustProduct;
        }



        // Get Customer Product Details by Product Id
        public CustomerProductModel GetCustomerProductById(int intProductId, ref string strErMsg)
        {
            CustomerProductModel objCustProduct = new CustomerProductModel();
            try
            {
                if (intProductId != 0)
                {
                    objCommon = new Common();

                    string strProcName = "SEL_CUSTOMER_PRODUCT_BY_PRODUCT_ID";

                    QueryParams qryPrmProdId = new QueryParams() { Name = "@ProductId", Value = intProductId };

                    List<QueryParams> lstPrms = new List<QueryParams>()
                    {
                       qryPrmProdId
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
                                objCustProduct = new CustomerProductModel();
                                objCustProduct.CustomerId = (int)dtRw["CustomerId"];
                                objCustProduct.Country = dtRw["Country"].ToString().Trim();
                                objCustProduct.PackageId = (int)dtRw["PackageId"];
                                objCustProduct.PackageName = dtRw["PackageName"].ToString().Trim();
                                objCustProduct.TrailDays = (int)dtRw["TrailDays"];
                                objCustProduct.CompanyName = dtRw["CompanyName"].ToString().Trim();
                                objCustProduct.EmployeeCount = (int)dtRw["EmployeeCount"];
                                objCustProduct.Currency = dtRw["Currency"].ToString().Trim();
                                objCustProduct.DueAmount = Convert.ToDecimal(dtRw["DueAmount"]);
                                objCustProduct.Status = dtRw["Status"].ToString().Trim();
                                objCustProduct.ActiveDate = Convert.ToDateTime(dtRw["ActiveDate"]);
                                objCustProduct.NextBillingDate = Convert.ToDateTime(dtRw["NextBillingDate"]);
                                objCustProduct.CancelledDate = Convert.ToDateTime(dtRw["CancelledDate"]);
                                objCustProduct.ProductId = (int)dtRw["ProductId"];
                            }
                        }
                        else
                        {
                            strErMsg = "No Data Found.";
                        }
                    }
                }
                else
                {
                    strErMsg = "No Data Found.";
                }
            }
            catch (Exception ex)
            {
                strErMsg = ex.Message.ToString().Trim();
            }

            return objCustProduct;
        }
        


        // Insert & Update Customer Product Details
        public bool UpsertCustomerProduct(CustomerProductModel objCustomerProduct, ref string strErMsg, ref int intLastRecord)
        {
            bool IsSuccess = false;
            try
            {
                objCommon = new Common();
                string strProcName = string.Empty;
                string strReturnCode = string.Empty;
                string strMessage = string.Empty;

                // Insert
                if (objCustomerProduct.ProductId == 0)
                {
                    strProcName = "INS_CUSTOMER_PRODUCT_DETAILS";

                    QueryParams qryPrmCustId = new QueryParams() { Name = "@CustomerId", Value = objCustomerProduct.CustomerId };
                    QueryParams qryPrmPackId = new QueryParams() { Name = "@PackageId", Value = objCustomerProduct.PackageId };
                    QueryParams qryPrmCmpNme = new QueryParams() { Name = "@CompanyName", Value = objCustomerProduct.CompanyName };
                    QueryParams qryPrmEmpCnt = new QueryParams() { Name = "@EmployeeCount", Value = objCustomerProduct.EmployeeCount };
                    QueryParams qryPrmStatus = new QueryParams() { Name = "@Status", Value = Globals.CUS_PRD_PENDING.ToString().Trim() };

                    List<QueryParams> lstPrms = new List<QueryParams>()
                        {
                           qryPrmCustId,
                           qryPrmPackId,
                           qryPrmCmpNme,
                           qryPrmEmpCnt,
                           qryPrmStatus
                        };

                    IsSuccess = objCommon.UPSERT(strProcName, lstPrms, ref strReturnCode, ref strMessage, ref intLastRecord);
                    if (IsSuccess == false)
                    {
                        throw new Exception(strMessage);
                    }
                }
                // Update
                else
                {
                    strProcName = "UPD_CUSTOMER_PRODUCT_DETAILS";

                    QueryParams qryPrmProdId = new QueryParams() { Name = "@ProductId", Value = objCustomerProduct.ProductId };
                    QueryParams qryPrmCmpNme = new QueryParams() { Name = "@CompanyName", Value = objCustomerProduct.CompanyName };
                    QueryParams qryPrmEmpCnt = new QueryParams() { Name = "@EmployeeCount", Value = objCustomerProduct.EmployeeCount };
                    QueryParams qryPrmPackId = new QueryParams() { Name = "@PackageId", Value = objCustomerProduct.PackageId };


                    List<QueryParams> lstPrms = new List<QueryParams>()
                        {
                           qryPrmProdId,
                           qryPrmCmpNme,
                           qryPrmEmpCnt,
                           qryPrmPackId
                        };

                    IsSuccess = objCommon.UPSERT(strProcName, lstPrms, ref strReturnCode, ref strMessage, ref intLastRecord);
                    if (IsSuccess == false)
                    {
                        throw new Exception(strMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                strErMsg = ex.Message.ToString().Trim();
            }

            return IsSuccess;
        }



        // Cancel Request - Update Records 
        public bool CancelRequest(CancelledRequestsModel objCancelRequest, ref string strErMsg)
        {
            bool IsSuccess = false;
            try
            {
                objCommon = new Common();
                string strProcName = string.Empty;
                string strReturnCode = string.Empty;
                string strMessage = string.Empty;
                int intLastRecord = 0;

                strProcName = "INS_PRODUCT_CANCEL_REQUEST";

                QueryParams qryPrmCustId = new QueryParams() { Name = "@CustomerId", Value = objCancelRequest.CustomerId };
                QueryParams qryPrmPackId = new QueryParams() { Name = "@PackageId", Value = objCancelRequest.PackageId };
                QueryParams qryPrmProdId = new QueryParams() { Name = "@ProductId", Value = objCancelRequest.ProductId };
                QueryParams qryPrmActvty = new QueryParams() { Name = "@Activity", Value = Globals.ACT_CUS_CAN_REQ.ToString().Trim() };
                QueryParams qryPrmStatus = new QueryParams() { Name = "@Status", Value = Globals.CUS_PRD_CANCEL_REQ_PENDING.ToString().Trim() };
                QueryParams qryPrmCnclDt = new QueryParams() { Name = "@Date", Value = objCancelRequest.Date };

                List<QueryParams> lstPrms = new List<QueryParams>()
                {
                   qryPrmCustId,
                   qryPrmPackId,
                   qryPrmProdId,
                   qryPrmActvty,
                   qryPrmStatus,
                   qryPrmCnclDt
                };

                IsSuccess = objCommon.UPSERT(strProcName, lstPrms, ref strReturnCode, ref strMessage, ref intLastRecord);
                if (IsSuccess == false)
                {
                    throw new Exception(strMessage);
                }
                else
                {
                    CancelRequestEmailNotification(objCancelRequest);
                }
            }
            catch (Exception ex)
            {
                strErMsg = ex.Message.ToString().Trim();
            }

            return IsSuccess;
        }



        // Cancel Request - Send Email
        public bool CancelRequestEmailNotification(CancelledRequestsModel objCancelRequest)
        {
            bool IsSuccess = false;
            string strSubject = string.Empty;
            string strBody = string.Empty;
            string strToMail = string.Empty;
            string strCCMail = string.Empty;

            strSubject = "MyAZHRM - Package Cancel Request...!";
            strBody = "Dear Admin, <br /> <br /> Company " + objCancelRequest.CompanyName.ToString().Trim() + " has been request to cancel " + objCancelRequest.PackageName.ToString().Trim() + " package on " + objCancelRequest.Date.ToString("dd-MMM-yyyy") + ". <br /> <br /> Thank You. <br /> <br />";
            strToMail = ConfigurationManager.AppSettings["MARKETING_EMAIL"].ToString().Trim();
            strCCMail = string.Empty;

            EmailNotification objEmailNotify = new EmailNotification();
            var thrdCanclReq = new Thread(() => objEmailNotify.SendingEmail(strToMail, strBody, strSubject, strCCMail));
            thrdCanclReq.Start();
            
            IsSuccess = true;
            return IsSuccess;
        }



        // Is Exist Product Id and Package Id 
        public bool IsExistProdIdPackId(int ProductId, int PackageId, int CustomerId, ref string strErMsg)
        {
            bool IsExist = false;
            try
            {
                objCommon = new Common();
                string strProcName = "SEL_CHECK_PRODUCT_ID_PACKAGE_ID";

                QueryParams qryPrmProdId = new QueryParams() { Name = "@ProductId", Value = ProductId };
                QueryParams qryPrmPackId = new QueryParams() { Name = "@PackageId", Value = PackageId };
                QueryParams qryPrmCustId = new QueryParams() { Name = "@CustomerId", Value = CustomerId };


                List<QueryParams> lstPrms = new List<QueryParams>()
                    { 
                       qryPrmProdId,
                       qryPrmPackId,
                       qryPrmCustId
                    };

                string strReturnCode = string.Empty;
                string strMessage = string.Empty;
                bool IsSuccess = false;

                System.Data.DataSet dtSet = objCommon.GetData(strProcName, lstPrms, ref strReturnCode, ref strMessage, ref IsSuccess);
                if (IsSuccess == true)
                {
                    IsExist = true;
                }
                else
                {
                    throw new Exception(strMessage);
                }
            }
            catch (Exception ex)
            {
                strErMsg = ex.Message.ToString().Trim();
            }

            return IsExist;
        }



    }
}