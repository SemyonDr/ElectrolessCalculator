using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// View model for a component of the target solution.
    /// Includes edit state logic.
    /// </summary>
    public class TargetComponent_ViewModel : ComponentBase_ViewModel
    {
        //Editing is realised by adding edit state. 
        //In edit state separate value property is used for storing entered value.
        //When saving entered value is written over displayed value if entered value is valid.

        #region EVENTS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raised when edit value is changed.
        /// </summary>
        public event EventHandler TarCmpEditValueChanged;

        /// <summary>
        /// Fires TarCmpEditValueChanged event.
        /// </summary>
        private void OnEditValueChanged() {
            EventArgs args = new EventArgs();
            if (TarCmpEditValueChanged != null)
                TarCmpEditValueChanged.Invoke(this, args);
        }
        #endregion


        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="Component">Represented target component.</param>
        /// <param name="Units">Units used to display a component.</param>
        /// <param name="TargetSolution_VM">Target solutuion.</param>
        public TargetComponent_ViewModel(Model.Component Component, Model.ComponentUnits Units, TargetSolution_ViewModel TargetSolution_VM) : base(Component, Units)
        {
            this.TargetSolution_VM = TargetSolution_VM;

            //Initializing EditValue input field
            ValidationSettingsFloat set = new ValidationSettingsFloat();
            set.CanBeZero = false;
            set.CanBeNegative = false;
            //Max value of concentration is set as such a concentration when amount of material exceeds by volume bath volume.
            //Logically this maximum concentration is equal to material density. It is simply sanity check.
            set.HaveMaxValue = true;
            set.MaxValue = Component.Density * 1000;
            float initial_concentration = Model.UnitsConverter.ConvertFromKg(Component.WeigthKg, TargetSolution_VM.Volume, Model.ComponentUnits.g_l, Component.Density);
            InputFieldFloat editValueField = new InputFieldFloat(Component.ShortName, initial_concentration, "F1", set);
            EditValue = editValueField;
        }

        #endregion


        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private bool editState;

        #endregion


        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------
        //Data binding

        //Solution this components is a part of.
        public TargetSolution_ViewModel TargetSolution_VM { get; private set; }

        //---------------------------------------------------------------------------------------------------------------
        //Displayed properties

        /// <summary>
        /// Indicates if component in the state of editing.
        /// </summary>
        public bool EditState {
            get { return editState; }
            set {
                if (editState != value) {
                    editState = value;
                    NotifyPropertyChanged("EditState");
                }
            }}


        /// <summary>
        /// Displayed value when not editing.
        /// </summary>
        public override float Value {
            get {
                return Model.UnitsConverter.ConvertFromKg(
                    Component.WeigthKg,
                    TargetSolution_VM.Volume,
                    Units,
                    Component.Density); }
            set {
                Component.WeigthKg = Model.UnitsConverter.ConvertToKg(
                    value,
                    TargetSolution_VM.Volume,
                    Units,
                    Component.Density);
                NotifyPropertyChanged("Value");
            }}


        /// <summary>
        /// Displayed value when editing.
        /// </summary>
        public InputFieldFloat EditValue {
            get;
            private set;
        }

        #endregion


        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Logic for starting editing.
        /// </summary>
        public void StartEdit() {
            EditValue.Value = Value.ToString(EditValue.Format);
            EditState = true;
        }

        /// <summary>
        /// This method checks if entered values are correct and can be saved.
        /// </summary>
        /// <returns></returns>
        public bool CanSaveEdit() {
            return EditValue.IsValid;
        }

        /// <summary>
        /// Logic for canceling editing.
        /// </summary>
        public void CancelEdit() {
            EditValue.Value = Value.ToString(EditValue.Format);
            EditState = false;
            
        }

        /// <summary>
        /// Logic for saving entered values.
        /// </summary>
        public void SaveEdit() {
            Value = EditValue.LastParsedValue;
            EditState = false;
        }
        #endregion
    }
}