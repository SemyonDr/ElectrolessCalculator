using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    /// <summary>
    /// This class represents final product of calculation - 
    /// amount of materials to add to current solution to achieve target composition.
    /// </summary>
    public class RequiredMaterials
    {
        private Calculator calc;
        private TargetSolution target;
        private CurrentSolution current;

        public Dictionary<CmpType, Component> Components {
            get {
                return calc.GetRequiredMaterials(target, current);
            }}

        public RequiredMaterials(TargetSolution targetSolution, CurrentSolution currentSolution) {
            calc = new Calculator();
            target = targetSolution;
            current = currentSolution;
        }
    }
}