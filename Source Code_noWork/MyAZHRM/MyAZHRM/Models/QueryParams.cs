using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAZHRM.Models
{
    public class QueryParams
    {
        public object _value;
        public string _name;
       
        public object Value
        {
            set { _value = value; }
            get { return _value; }
        }

        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
           
    }
}