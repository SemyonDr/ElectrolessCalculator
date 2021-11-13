using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public delegate void TarCmpEditValueHandler(TargetComponent_ViewModel sender, TarCmpEditValueChangedArgs args);

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
        public event TarCmpEditValueHandler TarCmpEditValueChanged;
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
            EditError = new TargetEditError(this.ShortName, TargetErrorType.NoError);
            TargetSolution_VM.TargetEditVolumeChanged += OnTargetEditVolumeChanged;
        }

        #endregion

        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private bool editState;
        private string editValue;
        private TargetEditError editError;

        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //Solution this components is part of.
        public TargetSolution_ViewModel TargetSolution_VM { get; private set; }


        /// <summary>
        /// Indicates if component in the state of editing.
        /// </summary>
        public bool EditState
        {
            get { return editState; }
            set {
                if (editState != value) {
                    editState = value;
                    NotifyPropertyChanged("EditState");
                }
            } }


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
            } }


        /// <summary>
        /// Displayed value when editing.
        /// </summary>
        public string EditValue {
            //Value entered by user while editing assumed to be concnetration in chosen Units
            //When edit is saved first bath volume is saved (in target solution view model), then absolute weigths of components are calculated
            //according to new volume, so concentration stay the same.
            get {
                return editValue;
            }
            set {
                editValue = value;
                OnEditValueChanged();
                NotifyPropertyChanged("EditValue");
            }
        }

        /// <summary>
        /// Shows if edit value is validated successfully.
        /// </summary>
        public bool IsEditValid {
            get {
                return (EditError.Type == TargetErrorType.NoError);
            }
        }

        /// <summary>
        /// Validation error.
        /// </summary>
        public TargetEditError EditError {
            get { return editError; }
            private set {
                //No need to notify about this property change, since it only used internally
                editError = value;
                NotifyPropertyChanged("IsEditValid");
            }}
        #endregion


        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Validates edit value of this components and returns first error encountered, or "no error" if value is valid.
        /// </summary>
        /// <returns></returns>
        public TargetEditError ValidateEdit() {
            //Variable to hold edit value parsed from input text
            float parsedValue = 0.0f;

            //Check if input is float
            bool isParsed = float.TryParse(editValue, out parsedValue);
            if (!isParsed) {
                EditError = new TargetEditError(this.ShortName, TargetErrorType.Invalid);
                return EditError;
            }

            //Check for negative value
            if (parsedValue < 0) {
                EditError = new TargetEditError(this.ShortName, TargetErrorType.Negative);
                return EditError;
            }

            //Check if component volume is bigger than bath volume
            float cmpVolume = Model.UnitsConverter.Convert(parsedValue, TargetSolution_VM.LastParsedEditVolume, Units, Model.ComponentUnits.l, Component.Density);
            if (cmpVolume > TargetSolution_VM.LastParsedEditVolume) {
                EditError = new TargetEditError(this.ShortName, TargetErrorType.TooBig);
                return EditError;
            }

            EditError = new TargetEditError(this.ShortName, TargetErrorType.NoError);
            return EditError;
        }


        /// <summary>
        /// Logic for starting editing.
        /// </summary>
        public void StartEdit() {
            //Setting edit value without triggering validation
            editValue = Value.ToString("F2");
            NotifyPropertyChanged("EditValue");
            EditState = true;
        }

        /// <summary>
        /// This method checks if entered values are correct and can be saved.
        /// </summary>
        /// <returns></returns>
        public bool CanSaveEdit() {
            return IsEditValid;
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
                Value = float.Parse(EditValue);
                EditState = false;
        }
        #endregion


        #region EVENT HANDLERS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On changing edit value value is validated and event is fired.
        /// </summary>
        private void OnEditValueChanged() {
            ValidateEdit();
            TarCmpEditValueChangedArgs args = new TarCmpEditValueChangedArgs(EditError);
            if (TarCmpEditValueChanged != null)
                TarCmpEditValueChanged.Invoke(this, args);
        }

        /// <summary>
        /// Called when solution edit volume is changed and triggers re-validation of component edit value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTargetEditVolumeChanged(object sender, EventArgs e)
        {
            ValidateEdit();
        }
        #endregion
    }



    /// <summary>
    /// Arguments for the event of changing the edit value.
    /// </summary>
    public class TarCmpEditValueChangedArgs {
        public bool IsValid {
            get {
                return (Error.Type == TargetErrorType.NoError);
            }}

        public TargetEditError Error { get; private set; }

        public TarCmpEditValueChangedArgs(TargetEditError Error) {
            this.Error = Error;
        }
    }
}