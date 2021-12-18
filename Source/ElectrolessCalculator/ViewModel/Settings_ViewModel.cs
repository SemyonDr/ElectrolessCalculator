using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// View model representation for Settings object.
    /// </summary>
    public class Settings_ViewModel : ViewModelBase
    {
        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings"></param>
        public Settings_ViewModel(Model.Settings settings) {
            settings_message = "";
            SettingsModel = settings;
        }

        #endregion


        #region PRIVATE FIELDS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        private string settings_message;

        #endregion


        #region PUBLIC PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        //--------------------
        //Displayed properties

        /// <summary>
        /// Text message that tells result of saving/loading.
        /// </summary>
        public string SettingsMessage {
            get {
                return settings_message;
            }
            set {
                settings_message = value;
                NotifyPropertyChanged("SettingsMessage");
            }}

        //--------------------
        //Data property

        public Model.Settings SettingsModel {
            get;
            private set; }

        #endregion

        #region EVENT HANDLERS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Handles changes in target solution.
        /// When target solution changed settings are saved to file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TargetSolutionChangedHandler(object sender, EventArgs e) {
            TargetSolution_ViewModel t_vm = sender as TargetSolution_ViewModel;
            //Refreshing setting from new values
            SettingsModel.UpdateFromObject(t_vm.TargetSolutionModel);
            //Saving refreshed setting to disc
            SettingsMessage = SettingsModel.SaveToFile();
        }
        #endregion
    }
}