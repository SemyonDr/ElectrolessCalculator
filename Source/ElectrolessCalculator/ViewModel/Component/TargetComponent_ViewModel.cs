using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{

    /// <summary>
    /// Handler for event when edit value of target component is changed.
    /// </summary>
    /// <param name="sender">Component that raised event.</param>
    /// <param name="editArgs">Data about the change.</param>
    public delegate void TrgCmpEditChangedHandler(object sender, TrgCmpEditChangedArgs editArgs);


    /// <summary>
    /// Arguments for event of changed edit value of the target component.
    /// </summary>
    public class TrgCmpEditChangedArgs {
        public bool isEditValid { get; private set; }
        public TrgEditErrorType ErrorType { get; private set; }

        public TrgCmpEditChangedArgs(bool isValid, TrgEditErrorType Error) {
            isEditValid = isEditValid;
            ErrorType = Error;
        }
    }

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
        /// This event is fired when value for editing is changed.
        /// </summary>
        public TrgCmpEditChangedHandler EditChanged;

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
            editState = false;
        }
        #endregion

        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private bool editState;
        private float editValue_conc;

        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //Solution this components is part of.
        public TargetSolution_ViewModel TargetSolution_VM { get; private set; }

        public bool EditState
        {
            get { return editState; }
            set {
                if (editState != value) {
                    editState = value;
                    NotifyPropertyChanged("EditState");
                }
            }}

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


        //Value entered by user while editing assumed to be concnetration in chosen Units
        //When edit is saved first bath volume is saved (in target solution view model), then absolute weigths of components are calculated
        //according to new volume, so concentration stay the same.
        public float EditValue {
            get {
                return editValue_conc;
            }
            set {
                editValue_conc = value;
                NotifyPropertyChanged("EditValue");
                OnEditValueChanged();
            }}

        /// <summary>
        /// This property binds with text box used for editing to check if input is a float number (don't contain letters, symbols, etc. i.e. correct)
        /// </summary>
        public bool isEditValid { get; set; }
        #endregion

        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method checks validity of user input value while in edit state and returns error type if found, or null if input is valid.
        /// </summary>
        /// <returns></returns>
        private TrgEditErrorType ValidateEditValue() {
            return TrgEditErrorType.NoError;
        }

        /// <summary>
        /// Logic for starting editing.
        /// </summary>
        public void StartEdit() {
            EditValue = (float)Math.Truncate(Value*100)/100;
            EditState = true;
        }

        /// <summary>
        /// This method checks if entered values are correct and can be saved.
        /// </summary>
        /// <returns></returns>
        public bool CanSaveEdit() {
            //Checking if value isn't negative
            if (EditValue < 0)
                return false;

            //Checking if total components volume is less than bath volume
            float components_total_volume = 0;
            foreach (TargetComponent_ViewModel c in TargetSolution_VM.Components) {
                components_total_volume += Model.UnitsConverter.ConvertFromKg(c.editValue_conc, 1, Model.ComponentUnits.l, c.Density);
            }

            if (components_total_volume > TargetSolution_VM.Volume) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Logic for canceling editing.
        /// </summary>
        public void CancelEdit() {
            EditState = false;
        }

        /// <summary>
        /// Logic for saving entered values.
        /// </summary>
        public void SaveEdit() {
                Value = EditValue;
                EditState = false;
        }
        #endregion


        #region EVENT HANDLERS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        private void OnEditValueChanged() {


        }

        #endregion
    }
}