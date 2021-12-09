using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class Settings_ViewModel : ViewModelBase
    {
        private string settings_message;

        public Model.Settings SettingsModel {
            get;
            private set; }

        public string SettingsMessage {
            get {
                return settings_message; }
            set {
                settings_message = value;
                NotifyPropertyChanged("SettingsMessage");
            }
        }

        public Settings_ViewModel(Model.Settings settings) {
            settings_message = "";
            SettingsModel = settings;
        }

        public void TargetSolutionChangedHandler(object sender, EventArgs e) {
            TargetSolution_ViewModel t_vm = sender as TargetSolution_ViewModel;
            SettingsModel.UpdateFromObject(t_vm.TargetSolutionModel);
            SettingsMessage = SettingsModel.SaveToFile();
        }
    }
}
