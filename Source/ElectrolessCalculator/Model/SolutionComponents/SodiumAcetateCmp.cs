using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    public class SodiumAcetateCmp : Component
    {
        public SodiumAcetateCmp(float Weigth) :
            base(Weigth,
                "Sodium Acetate Trihydrate",
                "Sodium Acetate",
                "C2H3NaO2(H20)3",
                1.45f
                ) {
        }
    }
}