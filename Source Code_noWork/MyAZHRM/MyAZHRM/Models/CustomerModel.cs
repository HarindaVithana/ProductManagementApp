using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class CustomerModel
    {
        private int _id;
        private string _name;
        private string _email;
        private string _phone;
        private string _address;
        private string _country;
        private string _currency;
        private string _password;
        private DateTime _regDate;
        private DateTime _deactiveDate;
        private string _status;

        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }

        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }

        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }

        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }

        public string Country
        {
            set { _country = value; }
            get { return _country; }
        }

        public string Currency
        {
            set { _currency = value; }
            get { return _currency; }
        }

        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }

        public DateTime RegDate
        {
            set { _regDate = value; }
            get { return _regDate; }
        }

        public DateTime DeactiveDate
        {
            set { _deactiveDate = value; }
            get { return _deactiveDate; }
        }

        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }

    }
}