using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// This class is used as settings for InputFieldFloat validation method.
    /// </summary>
    public class ValidationSettingsFloat
    {
        public bool CanBeZero = true;
        public bool CanBeNegative = true;
        public bool HaveMaxValue = false;
        public float MaxValue = 0.0f;

        /// <summary>
        /// Makes a copy object for a settings.
        /// </summary>
        /// <param name="original">Original settings.</param>
        /// <returns></returns>
        public static ValidationSettingsFloat MakeCopy(ValidationSettingsFloat original) {
            ValidationSettingsFloat copy = new ValidationSettingsFloat();
            copy.CanBeNegative = original.CanBeNegative;
            copy.CanBeZero = original.CanBeZero;
            copy.HaveMaxValue = original.HaveMaxValue;
            copy.MaxValue = original.MaxValue;
            return copy;
        }
    }
}
