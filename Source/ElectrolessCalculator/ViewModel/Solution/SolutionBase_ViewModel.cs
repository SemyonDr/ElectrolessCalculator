using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// Base class for solution view models.
    /// </summary>
    public class SolutionBase_ViewModel : ViewModelBase {
        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //Field backs public property
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
                NotifyPropertyChanged("TotalVolume");
            }}

        /// <summary>
        /// Bath volume.
        /// </summary>
        public float TotalVolume {
            get { return solution.TotalVolumeL; }
            set { solution.TotalVolumeL = value; }}

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
        }
        #endregion
    }
}