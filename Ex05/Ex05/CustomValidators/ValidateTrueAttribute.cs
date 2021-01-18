using System.ComponentModel.DataAnnotations;

namespace Ex05.CustomValidators
{
    public class ValidateTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is true;
        }
    }
}
