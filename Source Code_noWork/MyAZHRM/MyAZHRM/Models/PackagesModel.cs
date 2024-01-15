using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class PackagesModel
    {
        private int _id;
        private string _name;
        private decimal _amount;
        private int _trailDays;

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

        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }

        public int TrailDays
        {
            set { _trailDays = value; }
            get { return _trailDays; }
        }

    }
}