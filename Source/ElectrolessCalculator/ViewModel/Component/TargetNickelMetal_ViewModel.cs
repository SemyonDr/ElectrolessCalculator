using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// This class represents "virtual" component of metallic nickel, which is derived from nickel sulfate (i.e. value depends on nickel sulfate).
    /// Displaying metallic nickel is another way for the user to evaluate the nickel content.
    /// Metallic nickel component does not exist in the calculation model, so it requires dedicated view model which uses nickel sulfate component as data source.
    /// </summary>
    public class TargetNickelMetal_ViewModel : ViewModelBase
    {
        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="NickelSulfate_VM"></param>
        public TargetNickelMetal_ViewModel(TargetComponent_ViewModel NickelSulfate_VM) { 
            this.NickelSulfate_VM = NickelSulfate_VM;
            //Subscribing to changes in Nickel Sulfate view model value,
            //so they are reflected in nickel metal value.
            NickelSulfate_VM.PropertyChanged += NickelSalt_PropertyChanged;
        }

        /// <summary>
        /// Event handler for base EditValue property changed.
        /// Notifies of changing of this property in Nickel Metal view model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NickelSalt_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EditValue")
                NotifyPropertyChanged("EditValue");
            if (e.PropertyName == "Value")
                NotifyPropertyChanged("Value");
            if (e.PropertyName == "EditState")
                NotifyPropertyChanged("EditState");
            if (e.PropertyName == "Units")
                NotifyPropertyChanged("Units");
        }
        #endregion

        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private TargetComponent_ViewModel NickelSulfate_VM;
        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------
        //Displayed properties

        public string ShortName {
            get { return "  Nickel Metal"; }
        }

        public string FullName {
            get { return "Nickel Metal"; }
        }

        public string ChemicalFormula {
            get { return "Ni"; }
        }

        public float Density {
            get { return 8.908f; }
        }

        public Model.ComponentUnits Units {
            get { return NickelSulfate_VM.Units; }
        }

        public bool EditState {
            get { return NickelSulfate_VM.EditState; }
        }

        //Nickel metal value is calculated from nickel sulfate value;
        //Displayed value converted from absolute weigth in kg according with selected units.
        public float Value {
            get {
                return Model.UnitsConverter.ConvertFromKg(
                    Model.NickelConverter.ConvertSaltToMetal(NickelSulfate_VM.Component.WeigthKg),
                    NickelSulfate_VM.TargetSolution_VM.Volume,
                    NickelSulfate_VM.Units,
                    NickelSulfate_VM.Component.Density);
            }
            set {
                NickelSulfate_VM.Component.WeigthKg = Model.UnitsConverter.ConvertToKg(
                    Model.NickelConverter.ConvertMetalToSalt(value),
                    NickelSulfate_VM.TargetSolution_VM.Volume,
                    NickelSulfate_VM.Units,
                    NickelSulfate_VM.Component.Density);
                NotifyPropertyChanged("Value");
            }
        }

        //Displayed value for editing is converted from weigth in kg (saved in the private propery), according with component displayed units.
        public float EditValue {
            get {
                return Model.NickelConverter.ConvertSaltToMetal(NickelSulfate_VM.EditValue);
            }
            set {
                NickelSulfate_VM.EditValue = Model.NickelConverter.ConvertMetalToSalt(value);
                NotifyPropertyChanged("EditValue");
            }
        }
        #endregion
    }
}