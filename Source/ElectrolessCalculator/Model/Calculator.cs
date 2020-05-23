using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    public static class Calculator
    {
        public static Solution GetCurrentSolution(float NickelSulfate, float Volume, Solution TargetSolution) {
            Solution sol = new Solution(Volume);
            Component nickelSulfate = TargetSolution.Components.Find(c => c.ShortName == "Nickel Sulfate");
            if (nickelSulfate != null)
            {
                float targetNickelSalt = nickelSulfate.Weigth;
                float proportion = NickelSulfate / targetNickelSalt;

                foreach (Component c in TargetSolution.Components) {
                    if (c.ShortName != "Nickel Sulfate") {
                        float currentWeigth = c.Weigth * proportion;

                        sol.Components.Add(
                            new Component(
                                c.FullName,
                                c.ShortName,
                                c.ChemicalFormula,
                                currentWeigth,
                                c.Density
                                ));
                    }
                    else {
                        sol.Components.Add(
                            new Component(
                                c.FullName,
                                c.ShortName,
                                c.ChemicalFormula,
                                NickelSulfate,
                                c.Density
                            ));
                    }

                }//foreach
            }

            return sol;
        }
    }
}
