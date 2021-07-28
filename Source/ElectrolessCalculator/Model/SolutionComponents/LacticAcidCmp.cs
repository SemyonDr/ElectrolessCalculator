using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    public class LacticAcidCmp : Component
    {
        public float VolumeL { get { return base.WeigthKg/base.Density; } set { base.WeigthKg = value*base.Density; } }

        public LacticAcidCmp(float WeigthKg) :
            base(WeigthKg,
                "Lactic Acid",
                "Lactic Acid",
                "C3H6O3",
                1.206f  //kg/L
                ) {
        }
    }
}
