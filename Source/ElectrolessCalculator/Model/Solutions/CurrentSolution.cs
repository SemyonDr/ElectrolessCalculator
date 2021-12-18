using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    /// <summary>
    /// This class represents current bath composition estimated by analizes.
    /// </summary>
    public class CurrentSolution : Solution
    {
        private TargetSolution targetSolution;

        //Metallic nickel concentration obtained by analize, g/l
        public float NickelAnalize { get; set; }
        //Sodium hypophosphite concentration obtained by analize, g/l. Optional for calculation.
        public float HypophosphiteAnalize { get; set; }

        //If hypophosphite analize is used in calculation
        public bool UseHPAnalize { get; set; }

        private Calculator calculator;

        //Dictionary of components is created by the calculator
        new public Dictionary<CmpType, Component> Components {
            get {
                    return calculator.GetCurrentSolution(targetSolution, this);
            }
        }

        public CurrentSolution(float TotalVolumeL, TargetSolution TargetSolution) : base(TotalVolumeL) {
            calculator = new Calculator();
            targetSolution = TargetSolution;
        }
    }
}