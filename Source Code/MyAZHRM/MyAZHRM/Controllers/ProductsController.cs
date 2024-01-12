using MyAZHRM.Helpers;
using MyAZHRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyAZHRM.Controllers
{
    public class ProductsController : Controller
    {
        private CustomerModel objCustomer = null;
        private Products objProducts = null;
        private CustomerProductModel objCustProduct = null;
        private Common objCommon = null;



        public ActionResult MyProducts()
        {
            //ViewBag.Message = "My Products Details";

            List<ProductsModel> lstProducts = new List<ProductsModel>();
            objCustomer = new CustomerModel();
            if (Session["LoggedUsr"] != null)
            {
                objCustomer = (CustomerModel)Session["LoggedUsr"];
                string strErMsg = string.Empty;
                objProducts = new Products();
                lstProducts = objProducts.GetCustomerAllProducts(objCustomer, ref strErMsg);
            }

            return View(lstProducts);
        }



        public ActionResult AddProduct()
        {
            List<PackageModulesModel> lstProducts = new List<PackageModulesModel>();
            string strErMsg = string.Empty;

            objCustomer = new CustomerModel();
            if (Session["LoggedUsr"] != null)
            {
                objCustomer = (CustomerModel)Session["LoggedUsr"];
            }

            bool IsValidate = false;
            if (Request.QueryString.HasKeys() == true)
            {
                if (ValidateQueryStrings(Request.QueryString["ProductId"].ToString().Trim(), Request.QueryString["PackageId"].ToString().Trim(), objCustomer.Id, ref strErMsg) == true)
                {
                    IsValidate = true;
                }
                else
                {
                    throw new HttpException(strErMsg);
                }
            }
            else 
            {
                IsValidate = true;
            }

            if (IsValidate == true)
            {
                objProducts = new Products();
                int intProductId = 0;
                if (Request.QueryString["ProductId"] != null)
                {
                    intProductId = Convert.ToInt32(Request.QueryString["ProductId"]);
                }

                lstProducts = objProducts.GetPackageDetails(objCustomer, intProductId, ref strErMsg);
            }
            //else
            //{
            //    throw new HttpException(strErMsg);
            //}

            return View(lstProducts);
        }



        public ActionResult ProductDetails() 
        {
            //ViewBag.Message = "Product Details";
            return View();
        }



        // ============================== Releated to AddProduct.cshtml =========================================================



        public PartialViewResult CustomerProductDetails(int ProductId, int PackageId)
        {
            CustomerProductModel objCustProduct = new CustomerProductModel();
            objCustomer = new CustomerModel();        
            if (Session["LoggedUsr"] != null)
            {
                objCustomer = (CustomerModel)Session["LoggedUsr"];
                string strErMsg = string.Empty;
                objProducts = new Products();

                if (ProductId != 0)
                {
                    objCustProduct = objProducts.GetCustomerProductById(ProductId, ref strErMsg);
                }
                else
                {
                    objCustProduct = objProducts.GetCustomerProductDetails(objCustomer.Id, PackageId, ProductId, ref strErMsg);
                }
            }

            return PartialView("_CustomerProduct", objCustProduct);
        }

    

        public JsonResult UpdProductDetails(CustomerProductModel CustomerProduct)
        {
            List<string> lstRsltMsgs = new List<string>();
            List<CustomerProductModel> lstRsltObj = new List<CustomerProductModel>();
            objCustProduct = new CustomerProductModel();

            bool IsSuccess = false;
            string strErMsg = string.Empty; 
            int intLastRecord = 0;

            if (CustomerProduct != null)
            {
                objProducts = new Products();
                IsSuccess = objProducts.UpsertCustomerProduct(CustomerProduct, ref strErMsg, ref intLastRecord);

                if (IsSuccess == true && string.IsNullOrEmpty(strErMsg) == true)
                {
                    int intProductId = 0;
                    if (CustomerProduct.ProductId != 0)
                    {
                        intProductId = CustomerProduct.ProductId;
                    }
                    else
                    {
                        intProductId = intLastRecord;
                    }

                    objProducts = new Products();
                    objCustProduct = objProducts.GetCustomerProductById(intProductId, ref strErMsg);
                    // PartialView("_CustomerProduct", objCustProduct);
                }
            }
            else
            {
                strErMsg = "No data found to update";
            }

            lstRsltMsgs.Add(IsSuccess.ToString().Trim());
            lstRsltMsgs.Add(strErMsg.Trim());
            lstRsltObj.Add(objCustProduct);

            return Json(new { lstRslt1 = lstRsltMsgs, lstRslt2 = lstRsltObj }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult CancelRequest(CancelledRequestsModel CancelRequest)
        {
            List<string> lstRsltMsgs = new List<string>();
            bool IsSuccess = false;
            string strErMsg = string.Empty;

            if (CancelRequest != null)
            {
                CancelRequest.Date = DateTime.Now;
                objProducts = new Products();
                IsSuccess = objProducts.CancelRequest(CancelRequest, ref strErMsg);
            }
            else
            {
                strErMsg = "No data found to update";
            }

            lstRsltMsgs.Add(IsSuccess.ToString().Trim());
            lstRsltMsgs.Add(strErMsg.Trim());
            return Json(new { lstRslt1 = lstRsltMsgs }, JsonRequestBehavior.AllowGet);
        }

  

        // Vlidate Product Id and Package Id
        private bool ValidateQueryStrings(string strProductId, string strPackageId, int intCustomerId, ref string strErMsg)
        {
            bool IsSuccess = false;
            objCommon = new Common();
            objProducts = new Products();

            if (string.IsNullOrEmpty(Request.QueryString["ProductId"]) == true)
            {
                strErMsg = "Product Id cannot be null or empty.";
            }
            else if (string.IsNullOrEmpty(Request.QueryString["PackageId"]) == true)
            {
                strErMsg = "Package Id cannot be null or empty.";
            }
            else if (objCommon.IsNumber(Request.QueryString["ProductId"].ToString().Trim()) == false)
            {
                strErMsg = "Invalid Product Id.";
            }
            else if (objCommon.IsNumber(Request.QueryString["PackageId"].ToString().Trim()) == false)
            {
                strErMsg = "Invalid Package Id.";
            }
            else if (objProducts.IsExistProdIdPackId(Convert.ToInt32(Request.QueryString["ProductId"].ToString().Trim()), Convert.ToInt32(Request.QueryString["PackageId"].ToString().Trim()), intCustomerId, ref strErMsg) == true)
            {
                IsSuccess = true;
            }

            return IsSuccess;
        }



    }
}
