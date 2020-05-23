using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    public static class UnitsConverter
    {
        public static float Convert(float value, float bathVolume, ComponentUnits oldUnits, ComponentUnits newUnits, float componentDensity) {
            float absolute = ConvertToKg(value, bathVolume, oldUnits, componentDensity);
            return ConvertFromKg(value, bathVolume, newUnits, componentDensity);
        }

        public static float ConvertFromKg(float value, float bathVolume, ComponentUnits newUnits, float componentDensity) {
            switch (newUnits)
            {
                case ComponentUnits.g:
                    return value * 1000;
                case ComponentUnits.g_l:
                    return (value * 1000) / bathVolume;
                case ComponentUnits.kg:
                    return value;
                case ComponentUnits.kg_l:
                    return value / bathVolume;
                case ComponentUnits.l:
                    return value / componentDensity;
                case ComponentUnits.l_l:
                    return value / (componentDensity * bathVolume);
                case ComponentUnits.ml:
                    return value * 1000 / componentDensity;
                case ComponentUnits.ml_l:
                    return value * 1000 / (componentDensity * bathVolume);
                default:
                    return 0.0f;
            }
        }

        public static float ConvertToKg(float value, float bathVolume, ComponentUnits oldUnits, float ComponentDensity) {
            switch (oldUnits) {
                case ComponentUnits.g:
                    return value / 1000;
                case ComponentUnits.g_l:
                    return value * bathVolume / 1000;
                case ComponentUnits.kg:
                    return value;
                case ComponentUnits.kg_l:
                    return value * bathVolume;
                case ComponentUnits.l:
                    return value * ComponentDensity;
                case ComponentUnits.l_l:
                    return value * bathVolume * ComponentDensity;
                case ComponentUnits.ml:
                    return value * ComponentDensity / 1000;
                case ComponentUnits.ml_l:
                    return value * ComponentDensity * bathVolume / 1000;
                default:
                    return 0.0f;
            }
        }
    }
}
