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
            ComponentPresenter.ComponentUnits CU = (ComponentPresenter.ComponentUnits)value;

            switch (CU)
            {
                case ComponentPresenter.ComponentUnits.g:
                    return "g";
                case ComponentPresenter.ComponentUnits.kg:
                    return "kg";
                case ComponentPresenter.ComponentUnits.l:
                    return "L";
                case ComponentPresenter.ComponentUnits.ml:
                    return "mL";
                case ComponentPresenter.ComponentUnits.g_l:
                    return "g/L";
                case ComponentPresenter.ComponentUnits.kg_l:
                    return "kg/L";
                case ComponentPresenter.ComponentUnits.l_l:
                    return "L/L";
                case ComponentPresenter.ComponentUnits.ml_l:
                    return "mL/L";
            }

            return ComponentPresenter.ComponentUnits.kg;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
