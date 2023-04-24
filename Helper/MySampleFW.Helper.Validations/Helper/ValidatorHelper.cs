using FluentValidation.Results;

namespace MySampleFW.Validations.Helper
{
    public class ValidatorHelper
    {
        public static List<string> GetErrors(List<ValidationFailure> failures)
        {
            var errors = new List<string>();
            foreach (var item in failures)
                errors.Add(item.ErrorMessage);
            return errors;
        }
    }
}
