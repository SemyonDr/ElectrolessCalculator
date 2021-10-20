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
        /// <param name="CurrentSolution_VM"></param>
        public CurrentComponent_ViewModel(Model.Component CurrentComponent, Model.ComponentUnits Units, CurrentSolution_ViewModel CurrentSolution_VM) : base(CurrentComponent, Units) {
            this.CurrentSolution_VM = CurrentSolution_VM;

        }

        public CurrentSolution_ViewModel CurrentSolution_VM { get; private set; }

        public override float Value {
            get {
                return Model.UnitsConverter.ConvertFromKg(
                    Component.WeigthKg,
                    CurrentSolution_VM.Volume,
                    Units,
                    Component.Density);
            }
            set {
                Component.WeigthKg = Model.UnitsConverter.ConvertToKg(
                    value,
                    CurrentSolution_VM.Volume,
                    Units,
                    Component.Density);
                NotifyPropertyChanged("Value");
            }
        }

    }
}
