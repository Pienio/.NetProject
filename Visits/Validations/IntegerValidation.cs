using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Visits.Validations
{
    public class IntegerValidation : ValidationRule
    {
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int i;
            if (!int.TryParse(value.ToString(), out i))
                return new ValidationResult(false, "Podana wartość nie jest liczbą!");
            else
                if (i < (MinValue ?? i) || i > (MaxValue ?? i))
                return new ValidationResult(false,"Liczba musi być z zakresu 0 - 24");
            else
                return ValidationResult.ValidResult;
        }

       
    }
}
