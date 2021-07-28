using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    public class NickelSulfateCmp : Component {
        public NickelSulfateCmp(float Weigth) :
            base(Weigth,
                "Nickel(II) Sulfate Hexahydrate",
                "Nickel Sulfate",
                "NiSO4*(H2O)6",
                2.07f) {
        }
    }
}