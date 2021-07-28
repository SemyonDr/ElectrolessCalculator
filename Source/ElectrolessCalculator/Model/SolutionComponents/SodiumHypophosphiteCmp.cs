using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    public class SodiumHypophosphiteCmp : Component
    {
        public SodiumHypophosphiteCmp(float Weigth) :
            base(Weigth,
                "Sodium Hypophosphite Anhydrous",
                "Sodium Hypophosphite",
                "NaPO2H2",
                0.8f) {
        }
    }
}