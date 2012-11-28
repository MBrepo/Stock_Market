using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace App.StockMarket.Infrastructure.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class GreaterThanAttribute : ValidationAttribute
    {
        private int _value;


        public GreaterThanAttribute(int value)
        {
            this._value = value;
        }


        public override bool IsValid(object value)
        {
            return value != null && value is int && (int)value > this._value;
        }
    }
}
