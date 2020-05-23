using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class Solution_ViewModel : ViewModelBase
    {
        private bool editMode;

        public bool EditMode {
            get { return editMode; }
            set {
                editMode = value;
                NotifyPropertyChanged("EditMode"); }}

        public RelayCommand StartEditCommand { get; internal set; }

        private Model.Solution solution;

        public Model.Solution Solution {
            get { return solution; }
            set {
                solution = value;
                NotifyPropertyChanged("Components");
                NotifyPropertyChanged("TotalVolume");
            }}

        public List<Component_ViewModel> Components { get; set; }

        public float TotalVolume {
            get {
                return solution.TotalVolume; }
            set {
                solution.TotalVolume = value; }}


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="solution"></param>
        public Solution_ViewModel(Model.Solution solution) {
            EditMode = false;
            StartEditCommand = new RelayCommand(StartEditing, CanStartEditing);


            this.solution = solution;
            Components = new List<Component_ViewModel>();

            foreach (Model.Component c in solution.Components) {
                Component_ViewModel c_vm = new Component_ViewModel(c, Model.ComponentUnits.g_l, this);
                if (c.ShortName == "Nickel Sulfate") {
                    NickelMetal_ViewModel ni_vm = new NickelMetal_ViewModel(c_vm, Model.ComponentUnits.g_l, this);
                    Components.Add(ni_vm);
                }
                Components.Add(c_vm);
            }
        }

        private void StartEditing(object parameter) {
            if (!EditMode) { 
                EditMode = true;
                foreach (Component_ViewModel c in Components) {
                    c.EditMode = true;
                    c.EditValue = c.Value;
                }
            }
        }

        private bool CanStartEditing(object parameter) {
            return !EditMode;
        }
    }
}