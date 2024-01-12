using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class CustomerProductModel
    {
        private int _customerId;
        private string _country;
        private int _packageId;
        private string _packageName;
        private int _trailDays;
        private string _companyName;
        private int _employeeCount;
        private string _currency;
        private decimal _dueAmount;
        private string _status;
        private DateTime _activeDate;
        private DateTime _nextBillingDate;
        private DateTime _cancelledDate;
        private int _productId;

        public int CustomerId
        {
            set { _customerId = value; }
            get { return _customerId; }
        }

        [Display(Name = "Country")]
        public string Country
        {
            set { _country = value; }
            get { return _country; }
        }

        public int PackageId
        {
            set { _packageId = value; }
            get { return _packageId; }
        }

        [Display(Name = "Package")]
        public string PackageName
        {
            set { _packageName = value; }
            get { return _packageName; }
        }

        public int TrailDays
        {
            set { _trailDays = value; }
            get { return _trailDays; }
        }

        [Display(Name = "Company Name")]
        public string CompanyName
        {
            set { _companyName = value; }
            get { return _companyName; }
        }

        [Display(Name = "No of Employees")]
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

        public int ProductId
        {
            set { _productId = value; }
            get { return _productId; }
        }

    }
}