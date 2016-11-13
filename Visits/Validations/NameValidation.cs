using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Visits.Validations
{
    class NameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(!string.IsNullOrWhiteSpace(value.ToString()), "Nazwa nie może być pusta ani złożona z białych znaków.");
        }
    }
}
