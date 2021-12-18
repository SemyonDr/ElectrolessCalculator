using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// This class represents required component data.
    /// </summary>
    public class RequiredComponent_ViewModel :ComponentBase_ViewModel
    {
        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Component">Represented component.</param>
        /// <param name="Units">Display units for weigth.</param>
        /// <param name="VolumeUnits">Display units for volume.</param>
        public RequiredComponent_ViewModel(Model.Component Component, Model.ComponentUnits Units, Model.ComponentUnits VolumeUnits) : base(Component, Units) {
            this.VolumeUnits = VolumeUnits;
        }

        #endregion


        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------
        //Display properties

        /// <summary>
        /// First value (weigth or volume).
        /// </summary>
        public override float Value {
            get {
                //If calculation gives negative value display 0
                if (Component.WeigthKg >= 0.0f)
                    return Component.WeigthKg;
                else
                    return 0.0f; }
            set
            {
                Component.WeigthKg = Value;
                NotifyPropertyChanged("Value"); }}


        /// <summary>
        /// Volume value.
        /// </summary>
        public float ComponentVolume {
            get {
                if (Component.WeigthKg >= 0.0f)
                    return Component.WeigthKg / Component.Density;
                else
                    return 0.0f;
            }}


        /// <summary>
        /// Volume units.
        /// </summary>
        public Model.ComponentUnits VolumeUnits { get; set; }

        #endregion
    }
}
