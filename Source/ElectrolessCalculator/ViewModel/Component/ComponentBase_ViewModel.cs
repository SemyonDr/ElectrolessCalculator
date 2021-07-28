﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// Base class for a solution component view model.
    /// Contains properties for dispayng the component data.
    /// </summary>
    public abstract class ComponentBase_ViewModel : ViewModelBase {
        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //Private fields backing public properties
        private Model.ComponentUnits units;
        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------
        //Data binding

        //Binding to solution view model
        public SolutionBase_ViewModel Solution { get; private set; }

        //Binding to represented model object
        public Model.Component Component { get; private set; }

        //---------------------------------------------------------------------------------------------------------------
        //Displayed properties

        public string ShortName {
            get { return Component.ShortName; }}

        public string FullName {
            get { return Component.FullName; }}

        public string ChemicalFormula {
            get { return Component.ChemicalFormula; }}

        public float Density {
            get { return Component.Density; }}

        //Displayed value converted from absolute weigth in kg according with selected units.
        public virtual float Value {
            get {
                return Model.UnitsConverter.ConvertFromKg(
                    Component.WeigthKg,
                    Solution.TotalVolume,
                    units,
                    Component.Density); }
            set {
                Component.WeigthKg = Model.UnitsConverter.ConvertToKg(
                    value,
                    Solution.TotalVolume,
                    units,
                    Component.Density);
                NotifyPropertyChanged("Value"); }}

        //Units will be converted to text by view converter
        public Model.ComponentUnits Units {
            get { return units; }
            set {
                units = value;
                NotifyPropertyChanged("Value");
                NotifyPropertyChanged("Units"); }}
        #endregion

        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Component">Solution component.</param>
        /// <param name="Units">Component units.</param>
        /// <param name="Solution_VM">Solution view model.</param>
        public ComponentBase_ViewModel(Model.Component Component, Model.ComponentUnits Units, SolutionBase_ViewModel Solution_VM)
        {
            this.Component = Component;
            this.Units = Units;
            this.Solution = Solution_VM;
        }
        #endregion
    }
}