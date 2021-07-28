﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// View model for a component in the target solution.
    /// Have edit state logic added.
    /// </summary>
    public class TargetComponent_ViewModel : ComponentBase_ViewModel
    {
        //Editing is realised by adding edit state. 
        //In edit state separate value property is used for storing entered value.
        //When saving entered value is written over displayed value if entered value is valid.

        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="Component">Represented target component.</param>
        /// <param name="Units">Units used to display a component.</param>
        /// <param name="TargetSolution">Target solutuion.</param>
        public TargetComponent_ViewModel(Model.Component Component, Model.ComponentUnits Units, TargetSolution_ViewModel TargetSolution) : base(Component, Units, TargetSolution)
        {
            this.Solution = TargetSolution;
            editState = false;
        }
        #endregion

        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private bool editState;
        private float editValue_kg;

        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //Solution this components is part of.
        new public TargetSolution_ViewModel Solution { get; private set; }

        public bool EditState
        {
            get { return editState; }
            set {
                if (editState != value) {
                    editState = value;
                    NotifyPropertyChanged("EditState");
                }
            }}

        //Displayed value for editing is converted from weigth in kg (saved in the private propery), according with component displayed units.
        public float EditValue {
            get {
                return Model.UnitsConverter.ConvertFromKg(
                    editValue_kg,
                    Solution.TotalVolume,
                    Units,
                    Component.Density);
            }
            set {
                editValue_kg = Model.UnitsConverter.ConvertToKg(value,
                    Solution.TotalVolume,
                    Units,
                    Component.Density);
                NotifyPropertyChanged("EditValue");
            }}
        #endregion

        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Logic for starting editing.
        /// </summary>
        public void StartEdit() {
            EditState = true;
            EditValue = Value;
        }

        /// <summary>
        /// Logic for canceling editing.
        /// </summary>
        public void CancelEdit()
        {
            EditState = false;
        }

        /// <summary>
        /// This method checks if entered values are correct and can be saved.
        /// </summary>
        /// <returns></returns>
        public bool CanSaveEdit()
        {
            //Checking if value isn't negative
            if (EditValue < 0)
                return false;

            //Checking if total components volume is less than bath volume
            float components_total_volume = 0;
            foreach (TargetComponent_ViewModel c in Solution.Components)
            {
                components_total_volume += Model.UnitsConverter.ConvertFromKg(c.editValue_kg, 1, Model.ComponentUnits.l, c.Density);
            }

            if (components_total_volume > Solution.TotalVolume)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Logic for saving entered values.
        /// </summary>
        public void SaveEdit()
        {
            if (CanSaveEdit())
            {
                Value = EditValue;
                EditState = false;
            }
            else
            {
                //error logic goes here
            }
        }
        #endregion
    }
}