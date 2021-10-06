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
            RefreshComponentsVM();
        }
        #endregion

        public void RefreshComponentsVM() {
            //Initialize components list
            Components = new List<CurrentComponent_ViewModel>();

            //Creating components view models
            foreach (Model.Component c in Solution.Components.Values)
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

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //Model solution alias
        new public Model.CurrentSolution Solution {
            get {
                return (Model.CurrentSolution)(base.Solution);

            }}

        public bool UseHPAnalize {
            get {
                return Solution.UseHPAnalize;
            }
            set {
                Solution.UseHPAnalize = value;
                NotifyPropertyChanged("UseHPAnalize");
                RefreshComponentsVM();
                NotifyPropertyChanged("Components");
            }}

        public float NickelAnalize {
            get {
                return Solution.NickelAnalize;
            }
            set {
                Solution.NickelAnalize = value;
                NotifyPropertyChanged("NickelAnalize");
                RefreshComponentsVM();
                NotifyPropertyChanged("Components");
            }}

        public float HypophosphiteAnalize {
            get {
                return Solution.HypophosphiteAnalize;
            }
            set {
                Solution.HypophosphiteAnalize = value;
                NotifyPropertyChanged("HypophosphiteAnalize");
                RefreshComponentsVM();
                NotifyPropertyChanged("Components");
            }}

        public List<CurrentComponent_ViewModel> Components { get; private set; }

        #endregion

    }
}