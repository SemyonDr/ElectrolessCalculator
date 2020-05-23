using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ElectrolessCalculator.View
{
    public class ValueUnitsToTextConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.ComponentUnits CU = (Model.ComponentUnits)value;

            switch (CU)
            {
                case Model.ComponentUnits.g:
                    return "g";
                case Model.ComponentUnits.kg:
                    return "kg";
                case Model.ComponentUnits.l:
                    return "L";
                case Model.ComponentUnits.ml:
                    return "mL";
                case Model.ComponentUnits.g_l:
                    return "g/L";
                case Model.ComponentUnits.kg_l:
                    return "kg/L";
                case Model.ComponentUnits.l_l:
                    return "L/L";
                case Model.ComponentUnits.ml_l:
                    return "mL/L";
            }

            return Model.ComponentUnits.kg;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string units = value as string;
            switch (units)
            {
                case ("g"):
                    return Model.ComponentUnits.g;
                case ("kg"):
                    return Model.ComponentUnits.kg;
                case ("L"):
                    return Model.ComponentUnits.l;
                case ("mL"):
                    return Model.ComponentUnits.ml;
                case ("g/L"):
                    return Model.ComponentUnits.g_l;
                case ("kg/L"):
                    return Model.ComponentUnits.kg_l;
                case ("L/L"):
                    return Model.ComponentUnits.l_l;
                case ("ml/L"):
                    return Model.ComponentUnits.ml_l;
                default:
                    return "NaN";
            }
        }
    }
}
