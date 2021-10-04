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
    public class TargetSolution_ViewModel : SolutionBase_ViewModel
    {
        #region COMMANDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
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
        /// <param name="TargetSolution">Model target solution</param>
        public TargetSolution_ViewModel(Model.TargetSolution TargetSolution) : base(TargetSolution) {
            //Initializing state
            editState = false;

            //Creating commands
            StartEditCommand = new RelayCommand(new Action<object>(StartEdit), new Func<object, bool>(CanStartEdit));
            CancelEditCommand = new RelayCommand(new Action<object>(CancelEdit));
            SaveEditCommand = new RelayCommand(new Action<object>(SaveEdit), new Func<object, bool>(CanSaveEdit));

            //Initializing components list
            Components = new List<TargetComponent_ViewModel>();

            //Creating and adding components view models to the list
            foreach (Model.Component c in Solution.Components.Values) {
                //Components are displayed as gram per liter concentration,
                //except for Lactic Acid which comes in liquid form and displayed in ml per liter.
                Model.ComponentUnits units = Model.ComponentUnits.g_l;
                if (c.ShortName == "Lactic Acid")
                    units = Model.ComponentUnits.ml_l;

                TargetComponent_ViewModel c_vm = new TargetComponent_ViewModel(c, units, this);
                Components.Add(c_vm);
                
                if (c.ShortName == "Nickel Sulfate")
                    //Creating nickel metal row view model
                    NickelMetal = new TargetNickelMetal_ViewModel(c_vm);
                
            }
        }
        #endregion

        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        public List<TargetComponent_ViewModel> Components { get; private set; }
        public TargetNickelMetal_ViewModel NickelMetal { get; private set; }

        #endregion

        #region EDITING
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private bool editState;

        public bool EditState {
            get { return editState; }
            set {
                if (value != editState)
                    editState = value;
                NotifyPropertyChanged("EditState");
            }
        }

        /// <summary>
        /// Starts editing for each component and for the solution.
        /// </summary>
        /// <param name="parameter">For compatability with ICommand interface.</param>
        public void StartEdit(object parameter) {
            foreach (TargetComponent_ViewModel cmp in Components) {
                    cmp.StartEdit();
            }
            EditState = true;
        }

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
        /// Cancels edit state for each component and for the solution.
        /// </summary>
        /// <param name="parameter">For compatability with ICommand interface.</param>
        public void CancelEdit(object parameter) {
            foreach (TargetComponent_ViewModel cmp in Components) {
                    cmp.CancelEdit();
            }
            EditState = false;
        }


        public bool CanSaveEdit(object parameter) {
            //Here comes value check logic
            return true;
        }

        /// <summary>
        /// Save edited in results in components and then finishes edit state.
        /// </summary>
        /// <param name="parameter"></param>
        public void SaveEdit(object parameter) {
            foreach (TargetComponent_ViewModel cmp in Components) {
                    cmp.SaveEdit();
            }
            CancelEdit(null);
        }

        #endregion
    }
}