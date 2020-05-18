using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class SolutionComposition_ViewModel : ViewModelBase
    {
        Model.SolutionComposition composition;

        public SolutionComposition_ViewModel(Model.SolutionComposition composition) {
            this.composition = composition;
        }
    }
}