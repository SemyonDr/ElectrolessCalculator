using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// This class represents "virtual" component of metallic nickel, which is derived from nickel sulfate (value depends on nickel sulfate).
    /// Displaying metallic nickel is another way for user to evaluate the nickel content.
    /// Metallic nickel component does not exist in the calculation model, so it requires dedicated view model which uses nickel sulfate component as data source.
    /// </summary>
    public class TargetNickelMetal_ViewModel : TargetComponent_ViewModel
    {
        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="NickelSulfate_VM"></param>
        public TargetNickelMetal_ViewModel(TargetComponent_ViewModel NickelSulfate_VM) : base(NickelSulfate_VM.Component, NickelSulfate_VM.Units, NickelSulfate_VM.Solution) {
            //Subscribing to changes in Nickel Sulfate view model edit value,
            //so they are reflected in nickel metal edit value.
            base.PropertyChanged += NickelSalt_EditValue_PropertyChanged;
        }

        /// <summary>
        /// Event handler for base EditValue property changed.
        /// Notifies of changing of this property in Nickel Metal view model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NickelSalt_EditValue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EditValue")
                NotifyPropertyChanged("EditValue");
        }
        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------
        //Displayed properties

        new public string ShortName {
            get { return "Nickel Metall"; }
        }

        new public string FullName {
            get { return "Nickel Metall"; }
        }

        new public string ChemicalFormula {
            get { return "Ni"; }
        }

        new public float Density {
            get { return 8.908f; }
        }

        //Displayed value converted from absolute weigth in kg according with selected units.
        //Nickel metal value is calculated from nickel sulfate value;
        new public virtual float Value {
            get {
                return Model.UnitsConverter.ConvertFromKg(
                    Model.NickelConverter.ConvertSaltToMetal(Component.WeigthKg),
                    Solution.TotalVolume,
                    Units,
                    Component.Density);
            }
            set {
                Component.WeigthKg = Model.UnitsConverter.ConvertToKg(
                    Model.NickelConverter.ConvertMetalToSalt(value),
                    Solution.TotalVolume,
                    Units,
                    Component.Density);
                NotifyPropertyChanged("Value");
            }
        }

        //Displayed value for editing is converted from weigth in kg (saved in the private propery), according with component displayed units.
        new public float EditValue {
            get {
                return Model.NickelConverter.ConvertSaltToMetal(base.EditValue);
            }
            set {
                base.EditValue = Model.NickelConverter.ConvertMetalToSalt(value);
                NotifyPropertyChanged("EditValue");
                base.NotifyPropertyChanged("EditValue");
            }
        }
        #endregion
    }
}