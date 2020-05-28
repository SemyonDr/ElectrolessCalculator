using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class Component_ViewModel : ViewModelBase {
        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //Private fields backing public properties
        private bool editState;
        private float editValue_kg;
        private Model.ComponentUnits units;
        private Model.ComponentUnits editUnits;

        //---------------------------------------------------------------------------------------------------------------
        //Binding to represented model object
        private Model.Component component;
        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------
        //Binding to solution view model
        public SolutionBase_ViewModel solution { get; set; }


        //---------------------------------------------------------------------------------------------------------------
        //Logical properties
        public bool EditState {
            get { return editState; }
            set {
                if (editState != value) {
                    editState = value;
                    NotifyPropertyChanged("EditState");
                }
            }}


        //---------------------------------------------------------------------------------------------------------------
        //Displayed values for view

        //This list is used for building combobox for selected units
        public List<Model.ComponentUnits> UnitsList { get; }

        public string ShortName {
            get { return component.ShortName; }}

        public string FullName {
            get { return component.FullName; }}

        public string ChemicalFormula {
            get { return component.ChemicalFormula; }}

        public float Density {
            get { return component.Density; }}


        //Displayed value converted from component weigth in kg according with selected units.
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

        //Displayed value for editing is converted from weigth in kg (saved in private propery) according with units selected during editing.
        public float EditValue {
            get {
                return Model.UnitsConverter.ConvertFromKg(
                    editValue_kg,
                    solution.TotalVolume,
                    editUnits,
                    component.Density); }
            set {
                editValue_kg = Model.UnitsConverter.ConvertToKg(value,
                    solution.TotalVolume,
                    editUnits,
                    component.Density);
                NotifyPropertyChanged("EditValue"); }}


        public Model.ComponentUnits Units {
            get { return units; }
            set {
                units = value;
                NotifyPropertyChanged("Value");
                NotifyPropertyChanged("Units"); }}


        public Model.ComponentUnits EditUnits {
            get { return editUnits; }
            set {
                editUnits = value;
                NotifyPropertyChanged("EditUnits");
                NotifyPropertyChanged("EditValue"); }}
        #endregion

        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Component"></param>
        /// <param name="Units"></param>
        public Component_ViewModel(Model.Component Component, Model.ComponentUnits Units, SolutionBase_ViewModel solution)
        {
            editState = false;
            this.component = Component;
            this.Units = Units;
            this.solution = solution;

            Model.ComponentUnits[] unitsArray = (Model.ComponentUnits[])Enum.GetValues(typeof(Model.ComponentUnits));
            UnitsList = new List<Model.ComponentUnits>(unitsArray);
        }
        #endregion

        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        public void StartEdit() {
            EditState = true;
            EditUnits = Units;
            EditValue = Value;
        }

        public void CancelEdit() {
            EditState = false;
        }

        public bool CanSaveEdit() {
            //Checking if value isn't negative
            if (EditValue < 0)
                return false;

            //Checking if total components volume is less than bath volume
            float components_total_volume = 0;
            foreach (Component_ViewModel c in solution.Components) {
                components_total_volume += Model.UnitsConverter.ConvertFromKg(c.editValue_kg, 1, Model.ComponentUnits.l, c.Density);
            }

            if (components_total_volume > solution.TotalVolume) {
                return false;
            }

            return true; 
        }

        public void SaveEdit() {
            if (CanSaveEdit()) {
                Units = EditUnits;
                Value = EditValue;
                EditState = false;
            }
        }
        #endregion
    }
}