using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class RequiredComponent_VM :ComponentBase_ViewModel
    {
        public RequiredComponent_VM(Model.Component Component, Model.ComponentUnits Units, Model.ComponentUnits VolumeUnits) : base(Component, Units) {
            this.VolumeUnits = VolumeUnits;
        }

        public Model.ComponentUnits VolumeUnits { get; set; }

        public float ComponentVolume {
            get {
                return Component.WeigthKg / Component.Density;
            }}

        public override float Value {
            get {
                return Component.WeigthKg;
            }
            set {
                Component.WeigthKg = Value;
                NotifyPropertyChanged("Value");
            }}
    }
}
