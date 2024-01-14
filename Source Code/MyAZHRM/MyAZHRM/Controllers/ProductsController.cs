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
            ProductsModel objProduct = new ProductsModel();
            return View(objProduct);
        }


        [HttpPost]
        [AllowAnonymous]
        public string AddNewProduct(string prodVal, string skuVal, string retVal, string saledVal, string lowVal)
        {
            string returnMsg = string.Empty;
            ProductsModel objProduct = new ProductsModel();
            objProducts = new Products();


            decimal numret;
            decimal numsale;
            decimal numlow;

            if(decimal.TryParse(retVal, out numret) && decimal.TryParse(saledVal, out numsale) && decimal.TryParse(lowVal, out numlow))
            {
                objProduct.ProductName = prodVal;
                objProduct.SKU = skuVal;
                objProduct.RetailPrice = numret;
                objProduct.SalePrice = numsale;
                objProduct.LowestPrice = numlow;
                objProduct.CreatedDate = DateTime.Now;

                bool isSubmit = objProducts.AddNewProduct(objProduct, ref returnMsg);

                if(isSubmit)
                {
                    returnMsg = "Success";
                }
            }
            else
            {
                returnMsg = "Invalid Values";
            }

            return returnMsg;

        }

        [HttpPost]
        [AllowAnonymous]
        public string UpdateStatus(string Id, string isChecked)
        {
            string returnMsg = string.Empty;
            ProductsModel objProduct = new ProductsModel();
            objProducts = new Products();

            objProduct.Id = Convert.ToInt32(Id);
            
            if(isChecked == "1")
            {
                objProduct.Status = "Active";
            }
            else
            {
                objProduct.Status = "Inactive";
            }

            bool isUpdate = objProducts.UpdateStatus(objProduct, ref returnMsg);

            if(isUpdate)
            {
                returnMsg = "Success";
            }

            return returnMsg;
        }
    }
}
