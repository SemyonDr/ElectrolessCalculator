using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// Class representing required materials list for view.
    /// </summary>
    public class RequiredMaterials_VM : ViewModelBase
    {
        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requiredMaterialsModel">Model object for the list.</param>
        /// <param name="TargetSolution_VM">View model of associated target solution.</param>
        /// <param name="CurrentSolution_VM">View model of associated current solution.</param>
        public RequiredMaterials_VM(Model.RequiredMaterials requiredMaterialsModel, TargetSolution_ViewModel TargetSolution_VM, CurrentSolution_ViewModel CurrentSolution_VM) {
            //Subscribing to changes in taget and current solutions
            TargetSolution_VM.TargetSolutionChanged += SolutionsChangedHandler;
            CurrentSolution_VM.CurrentSolutionChanged += SolutionsChangedHandler;

            //Saving model object
            RequiredMaterialsModel = requiredMaterialsModel;

            //Initializing the list
            RefreshComponents();
        }

        #endregion

        #region PULBIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------
        //Displayed properties

        /// <summary>
        /// Required components list.
        /// </summary>
        public List<RequiredComponent_ViewModel> Components { get; private set; }

        //-------------------------------------------------
        //Data properties

        /// <summary>
        /// Model object of required material list.
        /// </summary>
        public Model.RequiredMaterials RequiredMaterialsModel { get; }

        #endregion

        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Refreshes required materials model object and creates new list of view models of the components.
        /// </summary>
        private void RefreshComponents() {
            Components = new List<RequiredComponent_ViewModel>();

            //Model is refreshed when values of model components are requested
            //Creating view model for components
            foreach (Model.Component c in RequiredMaterialsModel.Components.Values) {
                RequiredComponent_ViewModel c_vm = new RequiredComponent_ViewModel(c, Model.ComponentUnits.kg, Model.ComponentUnits.l);
                Components.Add(c_vm);
            }

            //Notifying that components list is changed
            NotifyPropertyChanged("Components");
        }

        #endregion

        #region EVENT HANDLERS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Handles changes in target and current solutions.
        /// Refreshes required materials list when they are changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolutionsChangedHandler(object sender, EventArgs e) {
            RefreshComponents();
        }
        #endregion
    }
}