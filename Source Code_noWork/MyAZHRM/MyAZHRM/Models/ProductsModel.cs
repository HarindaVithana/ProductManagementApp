using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class ProductsModel
    {
        private int _id;
        private string _productName;
        private string _SKU;
        private DateTime _createdDate;
        private decimal _retailPrice;
        private decimal _salePrice;
        private decimal _lowestPrice;
        private string _status;

        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }

        public string ProductName
        {
            set { _productName = value; }
            get { return _productName; }
        }

        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }

        public string SKU
        {
            set { _SKU = value; }
            get { return _SKU; }
        }

        public DateTime CreatedDate
        { 
            set { _createdDate = value; }
            get { return _createdDate; }
        }

        public decimal RetailPrice
        {
            set { _retailPrice = value;}
            get { return _retailPrice; }
        }

        public decimal SalePrice
        {
            set { _salePrice = value;}
            get { return _salePrice; }
        }

        public decimal LowestPrice
        {
            set { _lowestPrice = value;}
            get { return _lowestPrice; }
        }
    }
}