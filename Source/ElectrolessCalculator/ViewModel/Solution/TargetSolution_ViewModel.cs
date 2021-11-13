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
        /// Invoked while editing when editvalue of components change, or edit volume changes.
        /// </summary>
        public event EventHandler TargetEditValuesChanged;
        public event EventHandler TargetEditVolumeChanged;


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

            //Initializing edit errors list
            editErrors = new List<TargetEditError>();

            //Initializing validation indicator for volume
            IsEditVolumeValid = true;

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
                c_vm.TarCmpEditValueChanged += TarCmpEditValueChanged;

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
        private string editVolume;
        private float lastParsedEditVolume;
        private bool isEditVolumeValid;
        private List<TargetEditError> editErrors;
        

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


        /// <summary>
        /// Property used for displaying target solution volume while editing.
        /// </summary>
        public string EditVolume {
            get {
                return editVolume; }
            set {
                editVolume = value;
                //Trying to parse value
                float result;
                bool parsed = float.TryParse(value, out result);
                if (parsed)
                    lastParsedEditVolume = result;
                //Forcing components validation with new volume
                OnTargetEditVolumeChanged();
                //Validating edit
                ValidateEdit();
                NotifyPropertyChanged("EditVolume");
            }
        }

        /// <summary>
        /// Used to inform view validation indicator.
        /// </summary>
        public bool IsEditVolumeValid {
            get {
                return isEditVolumeValid;
            }
            private set {
                isEditVolumeValid = value;
                NotifyPropertyChanged("IsEditVolumeValid");
            }
        }

        /// <summary>
        /// Last value of Edit Volume that was succesfully parsed from string.
        /// </summary>
        public float LastParsedEditVolume {
            get {
                return lastParsedEditVolume;
            }}

        /// <summary>
        /// Collection of edit validation errors.
        /// </summary>
        public List<TargetEditError> EditErrors {
            get {
                return editErrors;
            }
            private set {
                editErrors = value;
                NotifyPropertyChanged("EditErrors");
                NotifyPropertyChanged("IsEditValid");
            }}

        /// <summary>
        /// This flag shows if current edit values pass validation
        /// </summary>
        public bool IsEditValid {
            get {
                if (EditErrors.Count == 0)
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
            lastParsedEditVolume = Volume;
            //Initializing edit volume without triggering validation
            editVolume = Volume.ToString("F2");
            NotifyPropertyChanged("EditVolume");
            IsEditVolumeValid = true;
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
            Volume = lastParsedEditVolume;
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
        public void OnTargetSolutionChanged() {
            if (TargetSolutionChanged != null)
                TargetSolutionChanged.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Called when edit value of one of the components changed.
        /// </summary>
        /// <param name="TarCmpObj"></param>
        /// <param name="args"></param>
        private void TarCmpEditValueChanged(TargetComponent_ViewModel TarCmpObj, TarCmpEditValueChangedArgs args){
            //Edit of solution have to be re-validated
            ValidateEdit();
        }

        private void OnTargetEditVolumeChanged() {
            if (TargetEditVolumeChanged != null) {
                TargetEditVolumeChanged.Invoke(this, new EventArgs());
            }
        }
        #endregion

        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method validates user input while editing target solution. 
        /// Validates volume input and individual components input.
        /// </summary>
        /// <returns></returns>
        private List<TargetEditError> ValidateEdit() {
            List<TargetEditError> errors = new List<TargetEditError>();

            //Trying to parse edit volume
            float parsedVolume = 0.0f;
            bool isVolumeParsed = float.TryParse(EditVolume, out parsedVolume);
            if (isVolumeParsed)
            {
                lastParsedEditVolume = parsedVolume;

                //Checking if volume is negative
                if (lastParsedEditVolume < 0)
                    errors.Add(new TargetEditError("Volume", TargetErrorType.Negative));

                //Checking if volume is zero
                if (lastParsedEditVolume == 0)
                    errors.Add(new TargetEditError("Volume", TargetErrorType.Zero));

                //Checking if volume is too big
                if (lastParsedEditVolume > 10000)
                    errors.Add(new TargetEditError("Volume", TargetErrorType.TooBig));

                //Setting the flag
                if (errors.Count > 0)
                    IsEditVolumeValid = false;
            }
            else {
                errors.Add(new TargetEditError("Volume", TargetErrorType.Invalid));
                IsEditVolumeValid = false;
            }

            //Updating value for volume validation indicator
            if (errors.Count == 0) {
                IsEditVolumeValid = true;
            }
                
            float totalCmpVolume = 0.0f;

            //Checking if components are valid
            foreach (TargetComponent_ViewModel c_vm in Components) {
                if (!c_vm.IsEditValid)
                    errors.Add(c_vm.EditError);
                else
                    //Calculating total volume of the components
                    totalCmpVolume += Model.UnitsConverter.Convert(float.Parse(c_vm.EditValue), lastParsedEditVolume, c_vm.Units, Model.ComponentUnits.l, c_vm.Component.Density);
            }

            //Checking if combined volume of components is bigger that bath volume
            if (totalCmpVolume > lastParsedEditVolume)
                errors.Add(new TargetEditError("Components", TargetErrorType.SumIsTooBig));

            //Setting result property
            EditErrors = errors;

            //Notifiying command
            //SaveEditCommand.RaiseCanExecuteChanged();

            return errors;
        }
        #endregion
    }

    /// <summary>
    /// This class represents validation error encountered while editing target solution.
    /// </summary>
    public class TargetEditError {
        public TargetErrorType Type { get; }
        public string Source { get; }
        public TargetEditError(string Source, TargetErrorType Type) {
            this.Type = Type;
            this.Source = Source;
        }
    }

    public enum TargetErrorType {
        Zero,
        Invalid,
        Negative,
        TooBig,

        //All
        SumIsTooBig,

        //No errors
        NoError
    }
}