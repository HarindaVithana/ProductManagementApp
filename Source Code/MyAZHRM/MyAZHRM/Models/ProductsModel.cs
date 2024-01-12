using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class ProductsModel
    {
        private int _id;
        private int _customerId;
        private int _packageId;
        private string _companyName;
        private int _employeeCount;
        private string _currency;
        private decimal _dueAmount;
        private string _status;
        private DateTime _activeDate;
        private DateTime _nextBillingDate;
        private DateTime _cancelledDate; 
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

        public string CompanyName
        {
            set { _companyName = value; }
            get { return _companyName; }
        }

        public int EmployeeCount
        {
            set { _employeeCount = value; }
            get { return _employeeCount; }
        }

        public string Currency
        {
            set { _currency = value; }
            get { return _currency; }
        }

        public decimal DueAmount
        {
            set { _dueAmount = value; }
            get { return _dueAmount; }
        }

        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }

        public DateTime ActiveDate
        {
            set { _activeDate = value; }
            get { return _activeDate; }
        }

        public DateTime NextBillingDate
        {
            set { _nextBillingDate = value; }
            get { return _nextBillingDate; }
        }

        public DateTime CancelledDate
        {
            set { _cancelledDate = value; }
            get { return _cancelledDate; }
        }

        public string PackageName
        {
            set { _packageName = value; }
            get { return _packageName; }
        }



    }
}