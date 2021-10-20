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
        public event EventHandler EditSaved;


        #endregion

        #region COMMANDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //Those commands are binded to buttons in view.

        public ICommand StartEditCommand { get; private set; }
        public ICommand CancelEditCommand { get; private set; }
        public ICommand SaveEditCommand { get; private set; }
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
        private float editVolume;

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
        public float EditVolume {
            get {
                return editVolume; }
            set {
                editVolume = value;
                NotifyPropertyChanged("EditVolume");
            }}

        //---------------------------------------------------------------------------------------------------------------
        //Internal properties
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
            EditVolume = Volume;
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
            //Here comes value check logic
            return true;
        }


        /// <summary>
        /// Save edited in results in components and then finishes edit state.
        /// </summary>
        /// <param name="parameter"></param>
        public void SaveEdit(object parameter) {
            Volume = EditVolume;
            foreach (TargetComponent_ViewModel cmp in Components) {
                    cmp.SaveEdit();
            }
            CancelEdit(null);

            //Raise event on editing completion to inform subscribers of new values
            if (EditSaved != null)
                EditSaved.Invoke(this, new EventArgs());
        }

        #endregion
    }
}