using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    public static class NickelConverter
    {
        public static float ConvertMetalToSalt(float NickelMetalMass)
        {
            return NickelMetalMass * 262.8f / 58.7f;
        }

        public static float ConvertSaltToMetal(float NickelSaltMass)
        {
            return NickelSaltMass * 58.7f / 262.8f;
        }
    }
}
