using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class Component_ViewModel : ViewModelBase {

        public List<Model.ComponentUnits> UnitsList { get; }

        private bool editMode;

        public bool EditMode { 
            get { return editMode; }
            set { editMode = value;
                NotifyPropertyChanged("EditMode"); }}

        public Solution_ViewModel solution { get; set; }
        private Model.Component component;
        private Model.ComponentUnits units;

        public Model.ComponentUnits Units {
            get { return units; }
            set {
                units = value;
                NotifyPropertyChanged("Units");
                NotifyPropertyChanged("Value");
            }}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Component"></param>
        /// <param name="Units"></param>
        public Component_ViewModel(Model.Component Component, Model.ComponentUnits Units, Solution_ViewModel solution) {
            this.component = Component;
            this.Units = Units;
            this.solution = solution;

            Model.ComponentUnits[] unitsArray = (Model.ComponentUnits[])Enum.GetValues(typeof(Model.ComponentUnits));
            UnitsList = new List<Model.ComponentUnits>(unitsArray);
        }

        public string ShortName {
            get { return component.ShortName; }}

        public string FullName {
            get { return component.FullName; }}

        public string ChemicalFormula {
            get { return component.ChemicalFormula; }}

        public float Density {
            get { return component.Density; }
        }

        public virtual float Value {
            get {
                return Model.UnitsConverter.ConvertFromKg(
                    component.Weigth, 
                    solution.TotalVolume,
                    units,
                    component.Density); }
            set {
                component.Weigth = Model.UnitsConverter.ConvertToKg(value,
                    solution.TotalVolume,
                    units,
                    component.Density);
                NotifyPropertyChanged("Value"); }}


        private float editValue;
        public float EditValue {
            get {
                return editValue;
            }
            set {
                editValue = value;
                NotifyPropertyChanged("EditValue");
            }
        }
    }
}