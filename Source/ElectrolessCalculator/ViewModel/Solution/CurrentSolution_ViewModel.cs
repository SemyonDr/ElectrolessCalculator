using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// Model representing current solution. Have edit states and logic to switch between them.
    /// </summary>
    public class CurrentSolution_ViewModel : ViewModelBase
    {
        #region EVENTS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Invoked when analize input, or current bath volume changed.
        /// </summary>
        public event EventHandler CurrentSolutionChanged;

        /// <summary>
        /// Invokes current solution changed event.
        /// </summary>
        private void OnCurrentSolutionChanged() {
            if (CurrentSolutionChanged != null)
                CurrentSolutionChanged.Invoke(this, new EventArgs());
        }

        #endregion


        #region COMMANDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Command for setting current bath volume as fraction of target bath volume.
        /// </summary>
        public ICommand SetVolumeFractionCommand { get; private set; }

        #endregion


        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="SolutionModel"></param>
        public CurrentSolution_ViewModel(Model.CurrentSolution SolutionModel, TargetSolution_ViewModel Target_VM)
        {
            //Saving model of the solution
            solutionModel = SolutionModel;

            //Initializing input errors list
            inputErrors = new List<InputError>();

            //Initializing input fields objects
            InitializeInputFields(SolutionModel);

            //Reference to target solution is used to set current folume by fractions of target volume.
            target_VM = Target_VM;

            //Creating commands
            SetVolumeFractionCommand = new RelayCommand(new Action<object>(SetVolumeFraction));

            //Subscribing to event of target solution components (and volume) changing
            Target_VM.TargetSolutionChanged += OnTargetSolutionChanged;
            //Subscribing to target edit volume changes
            target_VM.EditVolume.ValueChanged += TargetVolumeChangedHandler;

            //Creates initial component view models.
            RefreshComponentsVM();
        }

        /// <summary>
        /// This method creates and sets Input Field objects during creation of current solution view model.
        /// </summary>
        /// <param name="SolutionModel"></param>
        private void InitializeInputFields(Model.CurrentSolution SolutionModel) {
            //Max value of concentration is set as such a concentration when amount of material exceeds by volume bath volume.
            //Logically this maximum concentration is equal to material density. It is simply sanity check.

            //Nickel analize
            ValidationSettingsFloat niValidationSettings = new ValidationSettingsFloat();
            niValidationSettings.CanBeNegative = false;
            niValidationSettings.CanBeZero = false;
            niValidationSettings.HaveMaxValue = true;
            niValidationSettings.MaxValue = Model.NickelConverter.ConvertSaltToMetal(solutionModel.Components[CmpType.NickelSulfate].Density * 1000);
            InputFieldFloat nickelAnalize = new InputFieldFloat("Nickel Analize", solutionModel.NickelAnalize, "F1", niValidationSettings);
            NickelAnalize = nickelAnalize;

            //Sodium hypophosphite analize
            ValidationSettingsFloat hpValidationSettings = ValidationSettingsFloat.MakeCopy(niValidationSettings);
            hpValidationSettings.MaxValue = solutionModel.Components[CmpType.SodiumHypophosphite].Density * 1000;
            InputFieldFloat hpAnalize = new InputFieldFloat("Hypophosphite Analize", solutionModel.HypophosphiteAnalize, "F1", hpValidationSettings);
            HypophosphiteAnalize = hpAnalize;

            //Solution volume 
            ValidationSettingsFloat volumeValidationSettings = ValidationSettingsFloat.MakeCopy(niValidationSettings);
            volumeValidationSettings.CanBeZero = true;
            volumeValidationSettings.MaxValue = 10000.0f;   //Just a sanity check
            InputFieldFloat volume = new InputFieldFloat("Volume", solutionModel.TotalVolumeL, "F0", volumeValidationSettings);
            Volume = volume;

            //Subscribing to input fields changes
            NickelAnalize.ValueChanged += InputChanged;
            HypophosphiteAnalize.ValueChanged += InputChanged;
            Volume.ValueChanged += InputChanged;
        }
        #endregion


        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------
        //FIELDS BACKING PUBLIC PROPERTIES

        private TargetSolution_ViewModel target_VM;
        private List<CurrentComponent_ViewModel> components;
        private Model.CurrentSolution solutionModel;
        private List<InputError> inputErrors;
        #endregion



        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------
        //PROPERTIES FOR BINDING

        /// <summary>
        /// Value indicating if hypophosphite analize should be used in calculation.
        /// </summary>
        public bool UseHPAnalize {
            get {
                return SolutionModel.UseHPAnalize;
            }
            set {
                SolutionModel.UseHPAnalize = value;
                NotifyPropertyChanged("UseHPAnalize");
                //Components should be recalculated
                RefreshComponentsVM();
                OnCurrentSolutionChanged();
            }}

        /// <summary>
        /// Value of metalic nickel analize in g/l.
        /// </summary>
        public InputFieldFloat NickelAnalize {
            get;
            private set;
        }

        /// <summary>
        /// Value of sodium hypophosphite analize, g/L.
        /// </summary>
        public InputFieldFloat HypophosphiteAnalize {
            get;
            private set;
        }

        /// <summary>
        /// Current bath volume.
        /// </summary>
        public InputFieldFloat Volume {
            get;
            private set;
        }

        /// <summary>
        /// Input errors found during validation.
        /// </summary>
        public List<InputError> InputErrors {
            get {
                return inputErrors;
            }
            set {
                inputErrors = value;
                NotifyPropertyChanged("InputErrors");
                NotifyPropertyChanged("IsInputValid");
            }}

        /// <summary>
        /// This flag shows if current input values pass validation
        /// </summary>
        public bool IsInputValid {
            get {
                if (InputErrors.Count == 0)
                    return true;
                else
                    return false;
            }}

        /// <summary>
        /// List of current components view models.
        /// </summary>
        public List<CurrentComponent_ViewModel> Components {
            get { return components; }
            private set {
                components = value;
                NotifyPropertyChanged("Components");
            }}


        //-------------------------------------------------
        //OTHER PROPERTIES

        /// <summary>
        /// Model object of the current solution.
        /// </summary>
        public Model.CurrentSolution SolutionModel {
            get {
                return solutionModel;
            }
            set {
                solutionModel = value;
                NotifyPropertyChanged("Volume");
            }}

        #endregion


        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Sets current bath volume as fraction of target bath volume.
        /// </summary>
        /// <param name="Fraction">Fraction of target volume.</param>
        private void SetVolumeFraction(object Fraction) {
            float fraction = float.Parse((string)Fraction);
            Volume.Value = (target_VM.Volume * fraction).ToString(Volume.Format);
        }


        /// <summary>
        /// Refreshes model current solution and creates VMs for components.
        /// </summary>
        public void RefreshComponentsVM()
        {
            //Initialize components list
            List<CurrentComponent_ViewModel> new_components = new List<CurrentComponent_ViewModel>();

            //Creating components view models
            foreach (Model.Component c in SolutionModel.Components.Values) //At this moment new current components are calculated in the model
            {
                //Components are displayed as gram per liter concentration,
                //except for Lactic Acid which comes in liquid form and displayed in ml pre liter.
                Model.ComponentUnits units = Model.ComponentUnits.g_l;
                if (c.ShortName == "Lactic Acid")
                    units = Model.ComponentUnits.ml_l;

                CurrentComponent_ViewModel c_vm = new CurrentComponent_ViewModel(c, units, this);
                new_components.Add(c_vm);
            }

            Components = new_components;
        }


        /// <summary>
        /// Creates list of errors found on input during validation.
        /// </summary>
        private void RefreshErrors() {
            List<InputError> errors = new List<InputError>();
            if (!NickelAnalize.IsValid)
                errors.Add(new InputError(NickelAnalize.Name, ValidationErrorToMessageConverter.Convert(NickelAnalize.State)));
            if (!HypophosphiteAnalize.IsValid)
                errors.Add(new InputError(HypophosphiteAnalize.Name, ValidationErrorToMessageConverter.Convert(HypophosphiteAnalize.State)));
            if (!Volume.IsValid)
                errors.Add(new InputError(Volume.Name, ValidationErrorToMessageConverter.Convert(Volume.State)));
            InputErrors = errors;
        }
        #endregion


        #region EVENT HANDLERS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Executed when target solution changes. Refreshes components list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTargetSolutionChanged(object sender, EventArgs e) {
            //Current concentrations calculated as fractions of target concentrations, so when
            //target concentrations are saved current components should be recalculated
            RefreshComponentsVM();            
        }

        /// <summary>
        /// Handles input changes.
        /// Sets model values, triggers refreshment of the components and errors list.
        /// </summary>
        /// <param name="sender">Input Field object that was changed.</param>
        /// <param name="e">Arguments are empty.</param>
        private void InputChanged(object sender, EventArgs e)
        {
            InputFieldFloat Field = sender as InputFieldFloat;
            //Refreshing model values
            if (Field.Name == "Nickel Analize")
                solutionModel.NickelAnalize = Field.LastParsedValue;
            if (Field.Name == "Hypophosphite Analize")
                solutionModel.HypophosphiteAnalize = Field.LastParsedValue;
            if (Field.Name == "Volume")
                solutionModel.TotalVolumeL = Field.LastParsedValue;
            //Current components calculated as a function of analize values
            //and should be recalculated
            RefreshComponentsVM();
            //Renewing input errors list
            RefreshErrors();
            //Raising change event
            OnCurrentSolutionChanged();
        }

        /// <summary>
        /// Handles changes in target solution volume.
        /// Sets current volume equal to new target volume.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetVolumeChangedHandler(object sender, EventArgs e) {
            if (target_VM.EditVolume.IsValid)
                Volume.Value = target_VM.EditVolume.Value;
        }
        #endregion
    }
}