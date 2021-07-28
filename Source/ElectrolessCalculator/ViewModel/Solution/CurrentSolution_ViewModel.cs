using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// Model representing current solution. Have edit states and logic to switch between them.
    /// </summary>
    public class CurrentSolution_ViewModel : SolutionBase_ViewModel
    {
        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //Fields backing public properties
        private bool editState;
        #endregion

        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="solution"></param>
        public CurrentSolution_ViewModel(Model.Solution solution) : base(solution)
        {
            editState = false;

            StartEditCommand = new RelayCommand(StartEdit, CanStartEdit);
            CancelEditCommand = new RelayCommand(CancelEdit, CanCancelEdit);
            SaveEditCommand = new RelayCommand(SaveEdit, CanSaveEdit);
        }//CONSTRUCTOR
        #endregion

        #region EDITING
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //-----------------------STATE-----------------------------------------------------------------------------------

        /// <summary>
        /// Can be changed only by executing ~Editing commands.
        /// </summary>
        public bool EditState {
            get { return editState; }
            private set {
                editState = value;
                NotifyPropertyChanged("EditState");
                StartEditCommand.RaiseCanExecuteChanged();
                CancelEditCommand.RaiseCanExecuteChanged();
            }}

        //--------------------START EDITING--------------------------------------------------------------------------------

        public RelayCommand StartEditCommand { get; internal set; }

        /// <summary>
        /// Swithes on Edit mode.
        /// </summary>
        /// <param name="parameter"></param>
        public void StartEdit(object parameter) {
            if (!editState) { 
                EditState = true;
                foreach (ComponentBase_ViewModel c in Components) {
                    c.StartEdit();
                }
            }
        }

        /// <summary>
        /// Editing can be started if edit state is now normal.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanStartEdit(object parameter) {
            return !editState;
        }

        //--------------------CANCEL EDITING-------------------------------------------------------------------------------
        public RelayCommand CancelEditCommand { get; internal set; }

        /// <summary>
        /// Cancels editing.
        /// </summary>
        /// <param name="parameter"></param>
        public void CancelEdit(object parameter) {
            if (editState) {
                EditState = false;
                foreach (ComponentBase_ViewModel c in Components) {
                    c.CancelEdit();
                }
            }
        }

        /// <summary>
        /// Shows if edit can be canceled.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool CanCancelEdit(object parameter) {
            return editState;
        }

        //--------------------SAVE EDITING---------------------------------------------------------------------------------

        public RelayCommand SaveEditCommand { get; internal set; }

        private void SaveEdit(object parameter) {
            bool input_valid = true;
            foreach (ComponentBase_ViewModel c in Components) {
                input_valid = c.CanSaveEdit();
            }
            if (input_valid) {
                foreach (ComponentBase_ViewModel c in Components) {
                    c.SaveEdit();
                }
                EditState = false;
            }
        }

        private bool CanSaveEdit(object parameter) {
            foreach (ComponentBase_ViewModel c in Components) {
                if (!c.CanSaveEdit())
                    return false;
            }
            return true;
        }
        #endregion
    }
}