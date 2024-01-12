using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class ModulesModel
    {
        private int _id;
        private string _name;

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

    }
}