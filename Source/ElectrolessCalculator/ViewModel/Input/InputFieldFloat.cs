using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// This class is a view model for input field of a text box for float values.
    /// Contains validation logic and flags.
    /// </summary>
    public class InputFieldFloat : ViewModelBase
    {
        #region EVENTS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Event raised when value changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Called when value is changed and raises an event.
        /// </summary>
        private void OnValueChanged() {
            if (ValueChanged != null) {
                EventArgs args = new EventArgs();
                ValueChanged.Invoke(this, args);
            }
        }

        #endregion


        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="Name">Text name of the field.</param>
        /// <param name="InitialValue">Initial value.</param>
        /// <param name="Format">Format for displaying initial value.</param>
        /// <param name="Settings">Validation settings.</param>
        public InputFieldFloat(string Name, float InitialValue, string Format, ValidationSettingsFloat Settings)
        {
            this.Name = Name;
            this.Format = Format;
            lastParsed = InitialValue;
            value = InitialValue.ToString(Format);
            this.Settings = Settings;
            State = ValidationState.NoError;
        }

        #endregion


        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //Fields that back public properties
        private ValidationState state; 
        private string value;
        private float lastParsed;

        #endregion


        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------
        //PROPERTIES FOR BINDING

        /// <summary>
        /// Value for binding to view model (Text box input)
        /// </summary>
        public string Value {
            get { return value; }
            set {
                this.value = value;
                NotifyPropertyChanged("Value");
                //Performing value validation
                float parsed = Validate();
                //If value is parsed succesfully
                //Last parsed value is updated
                if (State != ValidationState.Invalid)
                    LastParsedValue = parsed;
                //Raising an event
                OnValueChanged();
            }}

        /// <summary>
        /// Flag that shows if entered value passes validation.
        /// Depends on State.
        /// </summary>
        public bool IsValid {
            get {
                if (State == ValidationState.NoError)
                    return true;
                else
                    return false;
            }}

        /// <summary>
        /// Last value succesfully parsed.
        /// </summary>
        public float LastParsedValue {
            get { return lastParsed; }
            private set {
                lastParsed = value;
                NotifyPropertyChanged("LastParsedValue");
            }}

        //-------------------------------------------------
        //STATE PROPERTY

        /// <summary>
        /// Validation state.
        /// </summary>
        public ValidationState State {
            get {
                return state;
            }
            private set {
                state = value;
                NotifyPropertyChanged("IsValid");
            }}

        //-------------------------------------------------
        //SETTINGS PROPERTIES

        /// <summary>
        /// Validation settings.
        /// </summary>
        public ValidationSettingsFloat Settings {
            get;
            private set;
        }

        /// <summary>
        /// String formatting options used to read and write from source property.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// String representation of this field name.
        /// </summary>
        public string Name {
            get;
            private set;
        }

        #endregion


        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Method that performes value validation according to settings and if value is parsed returns parsed float value.
        /// </summary>
        private float Validate() {
            float parsed_value = 0.0f;

            //Checking if value can be parsed
            bool isParsed = float.TryParse(this.value, out parsed_value);
            if (!isParsed) { 
                State = ValidationState.Invalid;
                return parsed_value;
            }

            //Checking if value is zero
            if (!Settings.CanBeZero) {
                if (parsed_value == 0.0f) {
                    State = ValidationState.Zero;
                    return parsed_value;
                }
            }

            //Checking if value is negative
            if (!Settings.CanBeNegative) {
                if (parsed_value < 0.0f) {
                    State = ValidationState.Negative;
                    return parsed_value;
                }
            }

            //Checking if value exceeds maximum
            if (Settings.HaveMaxValue) {
                if (parsed_value > Settings.MaxValue) {
                    State = ValidationState.TooBig;
                    return parsed_value;
                }
            }

            State = ValidationState.NoError;

            return parsed_value;
        }

        #endregion
    }
}