using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class NickelMetal_ViewModel : Component_ViewModel
    {
        private Component_ViewModel nickelSalt;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="NickelSalt"></param>
        /// <param name="Units"></param>
        public NickelMetal_ViewModel(Component_ViewModel NickelSalt, Model.ComponentUnits Units, SolutionBase_ViewModel solution) : base(new Model.Component("Nickel Metal","Metallic Nickel","Ni", 1.0f, 8.908f), Units, solution)
        {
            this.nickelSalt = NickelSalt;
            NickelSalt.PropertyChanged += NickelSalt_PropertyChanged;
            this.Units = Units;
        }

        private void NickelSalt_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value") {
                NotifyPropertyChanged("Value");
            }
        }

        public override float Value {
            get {
                float salt_kg = Model.UnitsConverter.ConvertToKg(nickelSalt.Value, solution.TotalVolume, nickelSalt.Units, nickelSalt.Density);
                float metal_kg = Model.NickelConverter.ConvertSaltToMetal(salt_kg);
                return Model.UnitsConverter.ConvertFromKg(metal_kg, solution.TotalVolume, Units, Density); }
            set {
                float nickel_kg = Model.UnitsConverter.ConvertToKg(value, solution.TotalVolume, Units, Density);
                float salt_kg = Model.NickelConverter.ConvertMetalToSalt(nickel_kg);
                nickelSalt.Value = Model.UnitsConverter.ConvertFromKg(salt_kg, solution.TotalVolume, nickelSalt.Units, nickelSalt.Density);
                NotifyPropertyChanged("Value");
            }
        }
    }
}
