using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class PackageModulesModel
    {
        private int _id;
        private int _packageId;
        private int _moduleId;

        private string _moduleName;
        private string _packageName;
        private string _currency;
        private decimal _amount;
        private int _trailDays;

        private string _status;

        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }

        public int PackageId
        {
            set { _packageId = value; }
            get { return _packageId; }
        }

        public int ModuleId
        {
            set { _moduleId = value; }
            get { return _moduleId; }
        }

        public string ModuleName
        {
            set { _moduleName = value; }
            get { return _moduleName; }
        }

        public string PackageName
        {
            set { _packageName = value; }
            get { return _packageName; }
        }

        public string Currency
        {
            set { _currency = value; }
            get { return _currency; }
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

        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }

    }
}