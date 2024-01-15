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

                    List<QueryParams> lstPrms = new List<QueryParams>();

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
                                objProducts.Id = (int)dtRw["Id"];
                                objProducts.SKU = dtRw["SKU"].ToString().Trim();
                                objProducts.ProductName = dtRw["ProductName"].ToString().Trim();
                                objProducts.CreatedDate = Convert.ToDateTime(dtRw["CreatedDate"]);
                                objProducts.RetailPrice = Convert.ToDecimal(dtRw["RetailPrice"]);
                                objProducts.SalePrice = Convert.ToDecimal(dtRw["SalePrice"]);
                                objProducts.LowestPrice = Convert.ToDecimal(dtRw["LowestPrice"]);
                                objProducts.Status = dtRw["status"].ToString();
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

        public bool AddNewProduct(ProductsModel objProduct, ref string message)
        {
            objCommon = new Common();
            string Procedure = "INS_CUSTOMER_PRODUCT_DETAILS";
            QueryParams prodparam = new QueryParams() { Name = "@ProductName", Value = objProduct.ProductName.ToString() };
            QueryParams skuparam = new QueryParams() { Name = "@SKU",Value = objProduct.SKU.ToString() };
            QueryParams createparam = new QueryParams() { Name = "@CreatedDate", Value = objProduct.CreatedDate.ToString() };
            QueryParams retparam = new QueryParams() { Name = "@RetailPrice", Value = objProduct.RetailPrice.ToString() };
            QueryParams saleparam = new QueryParams() { Name = "@SalePrice", Value = objProduct.SalePrice.ToString() };
            QueryParams lowparam = new QueryParams() { Name = "@LowestPrice", Value = objProduct.LowestPrice.ToString() };
            QueryParams stateparam = new QueryParams() { Name = "@Status", Value = "Active" };

            List<QueryParams> paramList = new List<QueryParams>()
            {
                prodparam,
                skuparam,
                createparam,
                retparam,
                saleparam,
                lowparam,
                stateparam
            };

            string strReturnCode = string.Empty;            
            int intLastRecord = 0;

            // Instert to db
            bool upsert = objCommon.UPSERT(Procedure, paramList, ref strReturnCode, ref message, ref intLastRecord);
            return upsert;
        }

        public bool UpdateStatus(ProductsModel objProduct, ref string message)
        {
            objCommon = new Common();
            string Procedure = "UPD_PRODUCT_STATUS";
            QueryParams idparam = new QueryParams() { Name = "@ProductId", Value = objProduct.Id.ToString() };
            QueryParams stateparam = new QueryParams() { Name = "@Status", Value = objProduct.Status.ToString() };

            List<QueryParams> paramList = new List<QueryParams>()
            {
                idparam,
                stateparam
            };

            string strReturnCode = string.Empty;
            int intLastRecord = 0;

            // Instert to db
            bool upsert = objCommon.UPSERT(Procedure, paramList, ref strReturnCode, ref message, ref intLastRecord);
            return upsert;
        }
    }
}