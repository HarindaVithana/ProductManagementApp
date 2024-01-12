using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class CancelledRequestsModel
    {
        private int _id;
        private int _customerId;
        private int _packageId;
        private int _productId;
        private string _activity;
        private DateTime _date;
        private string _previousStatus;

        private string _companyName;
        private string _packageName;
  
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }

        public int CustomerId
        {
            set { _customerId = value; }
            get { return _customerId; }
        }

        public int PackageId
        {
            set { _packageId = value; }
            get { return _packageId; }
        }

        public int ProductId
        {
            set { _productId = value; }
            get { return _productId; }
        }

        public string Activity
        {
            set { _activity = value; }
            get { return _activity; }
        }

        public DateTime Date
        {
            set { _date = value; }
            get { return _date; }
        }

        public string PreviousStatus
        {
            set { _previousStatus = value; }
            get { return _previousStatus; }
        }

        public string CompanyName
        {
            set { _companyName = value; }
            get { return _companyName; }
        }

        public string PackageName
        {
            set { _packageName = value; }
            get { return _packageName; }
        }

    }
}