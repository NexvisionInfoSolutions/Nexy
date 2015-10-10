using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Base
{
    public class CustomValidationMutualExclude : ValidationAttribute
    {
        public CustomValidationMutualExclude(params string[] propertyNames)
        {
            this.PropertyNames = propertyNames;
        }

        public string[] PropertyNames { get; private set; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var properties = this.PropertyNames.Select(validationContext.ObjectType.GetProperty);
            var propertType = properties.FirstOrDefault().DeclaringType;
            var values = properties.Select(p => p.GetValue(validationContext.ObjectInstance, null));
            var totalEntires = 0;
            foreach (var val in values)
            {
                if (!(string.IsNullOrWhiteSpace("" + val) || ("" + val).Equals("0") || ("" + val).Equals("0.0") || ("" + val).Equals("0.00")))
                    totalEntires++;
            }
            if (totalEntires > 1)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }
}
