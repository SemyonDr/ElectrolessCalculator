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
        public event EventHandler CurrentSolutionChanged;

        #region COMMANDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
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

            //Reference to target solution is used to set current folume by fractions.
            target_VM = Target_VM;

            //Creating commands
            SetVolumeFractionCommand = new RelayCommand(new Action<object>(SetVolumeFraction));

            //Subscribing to event of target solution components (and volume) changing
            Target_VM.TargetSolutionChanged += OnTargetSolutionChanged;

            //Calculate components values
            RefreshComponentsVM();
        }
        #endregion



        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        private TargetSolution_ViewModel target_VM;
        private Model.CurrentSolution solutionModel;

        #endregion



        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Current solution volume.
        /// </summary>
        public float Volume {
            get {
                return solutionModel.TotalVolumeL; }
            set {
                solutionModel.TotalVolumeL = value;
                NotifyPropertyChanged("Volume");
                //When volume changes absolute weigths of components should be recalculated
                RefreshComponentsVM();
                NotifyPropertyChanged("Components");
                OnCurrentSolutionChanged();
            }}


        /// <summary>
        /// Model object of the current solution.
        /// </summary>
        public Model.CurrentSolution SolutionModel {
            get {
                return solutionModel; }
            set {
                solutionModel = value;
                NotifyPropertyChanged("Volume");
            }
        }


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
                NotifyPropertyChanged("Components");
                OnCurrentSolutionChanged();
            }
        }


        /// <summary>
        /// Value of metalic nickel analize in g/l.
        /// </summary>
        public float NickelAnalize {
            get {
                return SolutionModel.NickelAnalize;
            }
            set {
                SolutionModel.NickelAnalize = value;
                NotifyPropertyChanged("NickelAnalize");
                //Components should be recalculated
                RefreshComponentsVM();
                NotifyPropertyChanged("Components");
                OnCurrentSolutionChanged();
            }
        }

        /// <summary>
        /// Value of sodium hypophosphite analize, g/L.
        /// </summary>
        public float HypophosphiteAnalize {
            get {
                return SolutionModel.HypophosphiteAnalize;
            }
            set {
                SolutionModel.HypophosphiteAnalize = value;
                NotifyPropertyChanged("HypophosphiteAnalize");
                //Components should be recalculated
                RefreshComponentsVM();
                NotifyPropertyChanged("Components");
                OnCurrentSolutionChanged();
            }
        }


        /// <summary>
        /// List of current components view models
        /// </summary>
        public List<CurrentComponent_ViewModel> Components { get; private set; }

        #endregion




        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        private void SetVolumeFraction(object Fraction)
        {
            float fraction = float.Parse((string)Fraction);
            Volume = target_VM.Volume * fraction;
        }



        public void RefreshComponentsVM()
        {
            //Initialize components list
            Components = new List<CurrentComponent_ViewModel>();

            //Creating components view models
            foreach (Model.Component c in SolutionModel.Components.Values) //At this moment new current components are calculated in the model
            {
                //Components are displayed as gram per liter concentration,
                //except for Lactic Acid which comes in liquid form and displayed in ml pre liter.
                Model.ComponentUnits units = Model.ComponentUnits.g_l;
                if (c.ShortName == "Lactic Acid")
                    units = Model.ComponentUnits.ml_l;

                CurrentComponent_ViewModel c_vm = new CurrentComponent_ViewModel(c, units, this);
                Components.Add(c_vm);
            }
        }
        #endregion





        #region EVENT HANDLERS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        private void OnTargetSolutionChanged(object sender, EventArgs e) {
            //Current concentrations calculated as fractions of target concentrations, so when
            //target concentrations are saved current components should be recalculated
            RefreshComponentsVM();
            //Refresh view
            NotifyPropertyChanged("Components");
        }

        private void OnCurrentSolutionChanged() {
            if (CurrentSolutionChanged != null)
                CurrentSolutionChanged.Invoke(this, new EventArgs());
        }
        #endregion
    }
}