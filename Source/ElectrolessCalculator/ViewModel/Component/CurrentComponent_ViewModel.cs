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
        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="CurrentComponent"></param>
        /// <param name="Units"></param>
        /// <param name="CurrentSolution_VM"></param>
        public CurrentComponent_ViewModel(Model.Component CurrentComponent, Model.ComponentUnits Units, CurrentSolution_ViewModel CurrentSolution_VM) : base(CurrentComponent, Units) {
            this.CurrentSolution_VM = CurrentSolution_VM;
        }
        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------
        //Data binding

        public CurrentSolution_ViewModel CurrentSolution_VM { get; private set; }


        //---------------------------------------------------------------------------------------------------------------
        //Displayed properties

        public override float Value {
            get {
                return Model.UnitsConverter.ConvertFromKg(
                    Component.WeigthKg,
                    CurrentSolution_VM.Volume.LastParsedValue,
                    Units,
                    Component.Density); }
            set {
                Component.WeigthKg = Model.UnitsConverter.ConvertToKg(
                    value,
                    CurrentSolution_VM.Volume.LastParsedValue,
                    Units,
                    Component.Density);
                NotifyPropertyChanged("Value"); }}

        #endregion
    }
}
