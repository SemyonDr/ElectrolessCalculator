using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{

    public class SolutionComponent_ViewModel : ViewModelBase {
        private Model.SolutionComposition composition;
        private Model.SolutionComponents type;
        private Model.ComponentUnits units;

        public SolutionComponent_ViewModel(Model.SolutionComponents type, Model.SolutionComposition composition, Model.ComponentUnits units) {
            this.type = type;
            this.composition = composition;
            this.Units = units;
        }


        public Model.SolutionComponents Type {
            get { return type;
            }}

        public Model.ComponentUnits Units {
            get { return units; }
            set {
                units = value;
                NotifyPropertyChanged("Units");
            }}

        public string Name {
            get { return composition.info.GetShortName(type); }
        }

        public float Value {
            get {
                return composition.GetComponentValue(type, Units);
            }
            set {
                composition.SetComponentValue(type, value, Units);
                NotifyPropertyChanged("Value");
            }
        }

        public float EditValue {
            get; set;
        }
    }
}