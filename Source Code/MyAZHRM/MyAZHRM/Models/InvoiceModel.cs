using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class InvoiceModel
    {
        private int _id;
        private int _customerId;
        private int _packageId;
        private string _billNo;
        private DateTime _date;
        private DateTime _billFrom;
        private DateTime _billTo;
        private string _billCurrency;
        private decimal _billAmount;
        private string _customerName;
        private string _customerEmail;
        private string _customerPhone;
        private string _customerAddress;

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

        public string BillNo
        {
            set { _billNo = value; }
            get { return _billNo; }
        }

        public DateTime Date
        {
            set { _date = value; }
            get { return _date; }
        }

        public DateTime BillFrom
        {
            set { _billFrom = value; }
            get { return _billFrom; }
        }

        public DateTime BillTo
        {
            set { _billTo = value; }
            get { return _billTo; }
        }

        public string BillCurrency
        {
            set { _billCurrency = value; }
            get { return _billCurrency; }
        } 

        public decimal BillAmount
        {
            set { _billAmount = value; }
            get { return _billAmount; }
        }

        public string CustomerName
        {
            set { _customerName = value; }
            get { return _customerName; }
        }

        public string CustomerEmail
        {
            set { _customerEmail = value; }
            get { return _customerEmail; }
        }

        public string CustomerPhone
        {
            set { _customerPhone = value; }
            get { return _customerPhone; }
        }

        public string CustomerAddress
        {
            set { _customerAddress = value; }
            get { return _customerAddress; }
        }

    }
}