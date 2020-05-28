using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class SolutionBase_ViewModel : ViewModelBase {
        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //Fields backing public properties
        private Model.Solution solution;
        #endregion

        #region SOLUTION PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Model object of the solution represented.
        /// </summary>
        public Model.Solution Solution {
            get { return solution; }
            set {
                solution = value;
                NotifyPropertyChanged("Components");
                NotifyPropertyChanged("TotalVolume");
            }}

        /// <summary>
        /// View model components list.
        /// </summary>
        public List<Component_ViewModel> Components { get; set; }

        /// <summary>
        /// Bath volume.
        /// </summary>
        public float TotalVolume {
            get { return solution.TotalVolume; }
            set { solution.TotalVolume = value; }}

        #endregion

        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="solution"></param>
        public SolutionBase_ViewModel(Model.Solution solution)
        {
            this.solution = solution;

            //Creating view models for the components
            Components = new List<Component_ViewModel>();

            foreach (Model.Component c in solution.Components)
            {
                Component_ViewModel c_vm = new Component_ViewModel(c, Model.ComponentUnits.g_l, this);
                if (c.ShortName == "Nickel Sulfate")
                {
                    NickelMetal_ViewModel ni_vm = new NickelMetal_ViewModel(c_vm, Model.ComponentUnits.g_l, this);
                    Components.Add(ni_vm);
                }
                Components.Add(c_vm);
            }
        }//CONSTRUCTOR
        #endregion
    }
}