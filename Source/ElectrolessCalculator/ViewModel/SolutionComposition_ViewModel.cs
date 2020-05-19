using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class SolutionComposition_ViewModel : ViewModelBase
    {
        private Model.SolutionComposition composition;
        public List<SolutionComponent_ViewModel> Composition { get; set; }

        public SolutionComposition_ViewModel(Model.SolutionComposition composition) {
            this.composition = composition;
            Composition = new List<SolutionComponent_ViewModel>();
            foreach (Model.SolutionComponents component in composition.ComponentsList) {
                //This logic should be in another place, so it will be possible to load proper units settings for each component
                Composition.Add(new SolutionComponent_ViewModel(component, composition, Model.ComponentUnits.g_l));
            }
        }
    }
}