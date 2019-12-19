using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NyimboProject.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidCompareAttribute : ValidationAttribute
    {
        private string _Left { get; set; }
        private string _Right { get; set; }

        public ValidCompareAttribute(string leftValueName, string rightValueName)
        {
            _Left = leftValueName;
            _Right = rightValueName;
        }

        public override bool IsValid(object value)
        {
            var props = value.GetType()
                .GetProperties()
                .Where(p => p.Name == _Left || p.Name == _Right)
                .ToDictionary(k => k.Name, e => e.GetValue(value));

            return props[_Left].Equals(props[_Right]);
        }
    }
}