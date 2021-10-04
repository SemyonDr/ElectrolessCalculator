using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// This class represents current component data.
    /// </summary>
    public class CurrentComponent_ViewModel : ComponentBase_ViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CurrentComponent"></param>
        /// <param name="Units"></param>
        /// <param name="Solution"></param>
        public CurrentComponent_ViewModel(Model.Component CurrentComponent, Model.ComponentUnits Units, CurrentSolution_ViewModel Solution) : base(CurrentComponent, Units, Solution) {
            

        }
    }
}
