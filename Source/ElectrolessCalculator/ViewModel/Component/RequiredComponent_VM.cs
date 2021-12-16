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
                if (Component.WeigthKg >= 0.0f)
                    return Component.WeigthKg / Component.Density;
                else
                    return 0.0f;
            }}

        public override float Value {
            get {
                if (Component.WeigthKg >= 0.0f)
                    return Component.WeigthKg;
                else
                    return 0.0f;
            }
            set {
                Component.WeigthKg = Value;
                NotifyPropertyChanged("Value");
            }}
    }
}
