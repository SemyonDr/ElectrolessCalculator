using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// Simple converter that converts validation states to string mesages.
    /// </summary>
    public static class ValidationErrorToMessageConverter
    {
        public static string Convert(ValidationState error) {
            switch (error) {
                case ValidationState.NoError:
                    return "";
                case ValidationState.Negative:
                    return "Value is negative";
                case ValidationState.Invalid:
                    return "Not a number";
                case ValidationState.TooBig:
                    return "Value is too big";
                case ValidationState.Zero:
                    return "Value is zero";
            }

            return "";
        }
    }
}
