using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ElectrolessCalculator.View
{
    public class TargetErrorToMessageConverter : IValueConverter
    {
        //Source to Target
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ViewModel.TargetEditError te = value as ViewModel.TargetEditError;
            string typeMessage = "";
            switch (te.Type)
            {
                case ViewModel.TargetErrorType.NoError:
                    typeMessage = "No error";
                    break;
                case ViewModel.TargetErrorType.Invalid:
                    typeMessage = "Not a number";
                    break;
                case ViewModel.TargetErrorType.Negative:
                    typeMessage = "Negative value";
                    break;
                case ViewModel.TargetErrorType.Zero:
                    typeMessage = "Value is zero";
                    break;
                case ViewModel.TargetErrorType.TooBig:
                    typeMessage = "Value is too large";
                    break;
                case ViewModel.TargetErrorType.SumIsTooBig:
                    typeMessage = "Total volume is too large";
                    break;
            }

            return $"{te.Source}: {typeMessage}";
        }


        //Target to source
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ViewModel.TargetEditError("NaN", ViewModel.TargetErrorType.NoError);
        }
    }
}
