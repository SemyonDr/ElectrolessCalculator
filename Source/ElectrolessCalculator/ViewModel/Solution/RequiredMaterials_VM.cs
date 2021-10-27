using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class RequiredMaterials_VM : ViewModelBase
    {
        public RequiredMaterials_VM(Model.RequiredMaterials requiredMaterialsModel, TargetSolution_ViewModel TargetSolution_VM, CurrentSolution_ViewModel CurrentSolution_VM) {
            TargetSolution_VM.TargetSolutionChanged += OnSolutionsChanged;
            CurrentSolution_VM.CurrentSolutionChanged += OnSolutionsChanged;

            RequiredMaterialsModel = requiredMaterialsModel;
            RefreshComponents();
        }


        public Model.RequiredMaterials RequiredMaterialsModel { get; }

        public void RefreshComponents() {
            Components = new List<RequiredComponent_VM>();

            foreach (Model.Component c in RequiredMaterialsModel.Components.Values) {
                RequiredComponent_VM c_vm = new RequiredComponent_VM(c, Model.ComponentUnits.kg, Model.ComponentUnits.l);
                Components.Add(c_vm);
            }

            NotifyPropertyChanged("Components");
        }

        public List<RequiredComponent_VM> Components { get; private set; }

        public void OnSolutionsChanged(object sender, EventArgs e) {
            RefreshComponents();
        }
    }
}