using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// View model for target solution. 
    /// </summary>
    public class TargetSolution_ViewModel : ViewModelBase
    {
        #region EVENTS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Invoked when editing is finished and entered values are saved.
        /// </summary>
        public event EventHandler TargetSolutionChanged;

        /// <summary>
        /// Fires TargetSolutionChanged event
        /// </summary>
        private void OnTargetSolutionChanged() {
            if (TargetSolutionChanged != null)
                TargetSolutionChanged.Invoke(this, new EventArgs());
        }

        #endregion

        #region COMMANDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //Those commands are binded to buttons in view.

        public RelayCommand StartEditCommand { get; private set; }
        public RelayCommand CancelEditCommand { get; private set; }
        public RelayCommand SaveEditCommand { get; private set; }
        #endregion


        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="TargetSolutionModel">Model target solution</param>
        public TargetSolution_ViewModel(Model.TargetSolution TargetSolutionModel) {
            //Saving model object
            targetSolutionModel = TargetSolutionModel;

            //Initializing editing state
            editState = false;

            //Creating commands
            StartEditCommand = new RelayCommand(new Action<object>(StartEdit), new Func<object, bool>(CanStartEdit));
            CancelEditCommand = new RelayCommand(new Action<object>(CancelEdit));
            SaveEditCommand = new RelayCommand(new Action<object>(SaveEdit), new Func<object, bool>(CanSaveEdit));

            //Creating input field for editVolume
            ValidationSettingsFloat set = new ValidationSettingsFloat();
            set.CanBeNegative = false;
            set.CanBeZero = false;
            set.HaveMaxValue = true;
            set.MaxValue = 10000.0f; //Maximum value is sanity check
            InputFieldFloat editVolumeInputField = new InputFieldFloat("Volume", TargetSolutionModel.TotalVolumeL, "F0", set);
            EditVolume = editVolumeInputField;
            //Subscribing to edit volume changes
            EditVolume.ValueChanged += OnInputValueChanged;

            //Initializing input errors list
            inputErrors = new List<InputError>();

            //Initializing components list
            Components = new List<TargetComponent_ViewModel>();

            //Creating and adding components view models to the list
            foreach (Model.Component c in this.targetSolutionModel.Components.Values) {
                //Components are displayed as gram per liter concentration,
                //except for Lactic Acid which comes in liquid form and displayed in ml per liter.
                Model.ComponentUnits units = Model.ComponentUnits.g_l;
                if (c.ShortName == "Lactic Acid")
                    units = Model.ComponentUnits.ml_l;

                TargetComponent_ViewModel c_vm = new TargetComponent_ViewModel(c, units, this);
                Components.Add(c_vm);

                //Subscribing to component edit value changed event
                c_vm.EditValue.ValueChanged += OnInputValueChanged;

                //Creating nickel metal "virtual" component view model
                //and adding it after nickel sulfate.
                if (c.ShortName == "Nickel Sulfate")
                    NickelMetal = new TargetNickelMetal_ViewModel(c_vm);
            }
        }
        #endregion


        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        private Model.TargetSolution targetSolutionModel;
        private List<InputError> inputErrors;

        #endregion


        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------
        //Displayed properties

        /// <summary>
        /// View models of the components of the target solution.
        /// Used to display components list in view.
        /// </summary>
        public List<TargetComponent_ViewModel> Components { get; private set; }


        /// <summary>
        /// View model used to display nickel metal "virtual" component.
        /// </summary>
        public TargetNickelMetal_ViewModel NickelMetal { get; private set; }


        /// <summary>
        /// Property for displaying target bath volume.
        /// </summary>
        public float Volume {
            get {
                return targetSolutionModel.TotalVolumeL; }
            set {
                targetSolutionModel.TotalVolumeL = value;
                NotifyPropertyChanged("Volume");
            }}


        public InputFieldFloat EditVolume {
            get;
            private set;
        }


        /// <summary>
        /// Collection of edit validation errors.
        /// </summary>
        public List<InputError> InputErrors {
            get {
                return inputErrors;
            }
            private set {
                inputErrors = value;
                NotifyPropertyChanged("InputErrors");
                NotifyPropertyChanged("IsEditValid");
            }}

        /// <summary>
        /// This flag shows if current edit values pass validation
        /// </summary>
        public bool IsEditValid {
            get {
                if (InputErrors.Count == 0)
                    return true;
                else
                    return false;
            }}

        
        #endregion


        #region PRIVATE PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private Model.TargetSolution TargetSolutionModel {
            get { return targetSolutionModel; }
            set {
                targetSolutionModel = value;
                NotifyPropertyChanged("Volume");
                NotifyPropertyChanged("Components");
            }}

        #endregion


        #region EDITING
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private bool editState;

        /// <summary>
        /// Indicates if solution in the state of editing.
        /// </summary>
        public bool EditState {
            get { return editState; }
            set {
                if (value != editState)
                    editState = value;
                NotifyPropertyChanged("EditState");
            }}


        /// <summary>
        /// Checks if editing can be started.
        /// </summary>
        /// <param name="parameter">For compatability with ICommand interface.</param>
        /// <returns></returns>
        public bool CanStartEdit(object parameter) {
            //If editing is already in progress can't start again
            return !EditState;
        }


        /// <summary>
        /// Starts editing for each component and for the solution.
        /// </summary>
        /// <param name="parameter">For compatability with ICommand interface.</param>
        public void StartEdit(object parameter) {
            EditVolume.Value = Volume.ToString(EditVolume.Format);
            foreach (TargetComponent_ViewModel cmp in Components) {
                    cmp.StartEdit();
            }
            EditState = true;
        }


        /// <summary>
        /// Cancels edit state for each component and for the solution.
        /// </summary>
        /// <param name="parameter">For compatability with ICommand interface.</param>
        public void CancelEdit(object parameter) {
            foreach (TargetComponent_ViewModel cmp in Components) {
                    cmp.CancelEdit();
            }
            EditState = false;
            EditVolume.Value = Volume.ToString(EditVolume.Format);
        }


        /// <summary>
        /// Checks if input from user is valid and can be saved.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanSaveEdit(object parameter) {
            return IsEditValid;
        }


        /// <summary>
        /// Save edited in results in components and then finishes edit state.
        /// </summary>
        /// <param name="parameter"></param>
        public void SaveEdit(object parameter) {
            Volume = EditVolume.LastParsedValue;
            foreach (TargetComponent_ViewModel cmp in Components) {
                    cmp.SaveEdit();
            }
            CancelEdit(null);

            //Raise event on editing completion to inform subscribers of new values
            OnTargetSolutionChanged();
        }

        #endregion

        #region EVENT HANDLERS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        private void OnInputValueChanged(object sender, EventArgs e) {
            RefreshErrors();
        }
        #endregion

        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Collects input errors from components and edit volume and creates an error list.
        /// </summary>
        public void RefreshErrors() {
            List<InputError> errors = new List<InputError>();

            //Checking edit volume for errors
            if (!EditVolume.IsValid) {
                InputError volume_error = new InputError(EditVolume.Name, ValidationErrorToMessageConverter.Convert(EditVolume.State));
                errors.Add(volume_error);
            }

            float total = 0.0f;
            //Checking components for errors and calculating total volume of components
            foreach (TargetComponent_ViewModel c in Components) {
                if (!c.EditValue.IsValid) {
                    InputError cmp_error = new InputError(c.EditValue.Name, ValidationErrorToMessageConverter.Convert(c.EditValue.State));
                    errors.Add(cmp_error);
                }
                total += Model.UnitsConverter.Convert(c.EditValue.LastParsedValue, EditVolume.LastParsedValue, c.Units, Model.ComponentUnits.l, c.Density);
            }

            //Checking if total volume of components isn't bigger than bath total volume
            if (total > EditVolume.LastParsedValue) {
                InputError total_error = new InputError("Components", "Sum is too big");
                errors.Add(total_error);
            }

            InputErrors = errors;
        }
        #endregion
    }
}