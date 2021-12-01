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
            NickelSulfate_VM.EditValue.ValueChanged += NickelSalt_EditValueChanged;
        }

        private void NickelSalt_EditValueChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("EditValue");
        }

        /// <summary>
        /// Event handler for base EditValue property changed.
        /// Notifies of changing of this property in Nickel Metal view model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NickelSalt_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
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
        private bool isKeyboardFocused;
        private string editValue;
        #endregion

        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------------------------
        //Displayed properties

        public string ShortName {
            get { return "Nickel Metal"; }
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
        public string EditValue {
            get {
                if (IsKeyboardFocused)
                {   //If value is currently entered there is no need to copy it from nickel sulfate
                    return editValue;
                }
                else {
                    //If input textbox is not focused edit value should be calculated from
                    //Nickel Sulfate edit value
                    //Checking if Nickel Sulfate edit value parsed succesfully
                    if (NickelSulfate_VM.EditValue.State != ValidationState.Invalid) {
                        //If value is parsed nickel metal value calculated
                        //and then converted to string using nickel sulfate string format
                        float ni_me = Model.NickelConverter.ConvertSaltToMetal(NickelSulfate_VM.EditValue.LastParsedValue);
                        return ni_me.ToString(NickelSulfate_VM.EditValue.Format);
                    }
                    else {
                        //If parsing failed current content on nickel sulfate edit value returned
                        return NickelSulfate_VM.EditValue.Value;
                    }
                }
            }
            set {
                editValue = value;
                //Trying to parse input value
                float ni_me;
                bool isParsed = float.TryParse(value, out ni_me);
                if (isParsed) {
                    //If value is parsed converting it to nickel salt and displaying it in nickel sulfate view model
                    NickelSulfate_VM.EditValue.Value = Model.NickelConverter.ConvertMetalToSalt(ni_me).ToString(NickelSulfate_VM.EditValue.Format);
                }
                else {
                    //If failed to parse copy input text to nickel sulfate
                    NickelSulfate_VM.EditValue.Value = value;
                }
            }
        }

        /// <summary>
        /// Flag showing that associated text box is currently focused for entering value.
        /// </summary>
        public bool IsKeyboardFocused {
            get { return isKeyboardFocused; }
            set {
                isKeyboardFocused = value;
                NotifyPropertyChanged("IsKeyboardFocused");
            }}
        #endregion
    }
}