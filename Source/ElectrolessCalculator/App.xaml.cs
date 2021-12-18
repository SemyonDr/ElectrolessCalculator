using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElectrolessCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(Object sender, StartupEventArgs args)
        {
            //Creating settings object
            Model.Settings settings = new Model.Settings();
            //Loading settings data
            //If loading fails settings will have default values
            string loading_message = settings.UpdateFromFile();

            //Creating model data
            Model.TargetSolution targetSolution = CreateTargetSolution();
            settings.SetObject(targetSolution);
            Model.CurrentSolution currentSolution = CreateCurrentSolution(targetSolution);
            Model.RequiredMaterials requiredMaterials = new Model.RequiredMaterials(targetSolution, currentSolution);

            //Creating ViewModels
            ViewModel.TargetSolution_ViewModel targetSolution_VM = new ViewModel.TargetSolution_ViewModel(targetSolution);
            ViewModel.CurrentSolution_ViewModel currentSolution_VM = new ViewModel.CurrentSolution_ViewModel(currentSolution, targetSolution_VM);
            ViewModel.RequiredMaterials_VM requiredMaterials_VM = new ViewModel.RequiredMaterials_VM(requiredMaterials, targetSolution_VM, currentSolution_VM);
            ViewModel.Settings_ViewModel settings_VM = new ViewModel.Settings_ViewModel(settings);

            //Subscribing setting to changes in target solution
            targetSolution_VM.TargetSolutionChanged += settings_VM.TargetSolutionChangedHandler;
            settings_VM.SettingsMessage = loading_message;

            //Creating Windows
            View.MainWindow mainWindow = new View.MainWindow();

            //Setting data contexts for view elements
            //Target solution
            mainWindow.TargetSolution.DataContext = targetSolution_VM;
            mainWindow.TargetVolumePresenter.DataContext = targetSolution_VM;
            mainWindow.TargetEditPanel.DataContext = targetSolution_VM;
            mainWindow.TargetEditPanel.Message.DataContext = settings_VM;
            mainWindow.TopTargetError.DataContext = targetSolution_VM;

            //Current solution
            mainWindow.AnalizePanel.DataContext = currentSolution_VM;
            mainWindow.CurrentVolumePresenter.DataContext = currentSolution_VM;
            mainWindow.CurrentSoution.DataContext = currentSolution_VM;
            mainWindow.TopCurrentError.DataContext = currentSolution_VM;

            //Required materials
            mainWindow.RequiredMaterials.DataContext = requiredMaterials_VM;
            
            //Showing main window
            mainWindow.Show();
        }

        /// <summary>
        /// Creates target solution model object with default values.
        /// </summary>
        /// <returns></returns>
        private Model.TargetSolution CreateTargetSolution() {
            Model.Settings set = new Model.Settings();

            Model.TargetSolution target = new Model.TargetSolution(set.Volume);
            Model.ComponentFactory cf = new Model.ComponentFactory();
            target.Components.Add(CmpType.NickelSulfate, cf.CreateComponent(CmpType.NickelSulfate, set.NickelSulfate));
            target.Components.Add(CmpType.SodiumHypophosphite, cf.CreateComponent(CmpType.SodiumHypophosphite, set.SodiumHypophosphite));
            target.Components.Add(CmpType.SodiumAcetate, cf.CreateComponent(CmpType.SodiumAcetate, set.SodiumAcetate));
            target.Components.Add(CmpType.SuccinicAcid, cf.CreateComponent(CmpType.SuccinicAcid, set.SuccinicAcid));
            target.Components.Add(CmpType.LacticAcid, cf.CreateComponent(CmpType.LacticAcid, set.LacticAcid));
            return target;
        }

        /// <summary>
        /// Creates current solution model object with default values.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private Model.CurrentSolution CreateCurrentSolution(Model.TargetSolution target) {
            //Initial current volume is the same as in target solution
            Model.CurrentSolution current = new Model.CurrentSolution(target.TotalVolumeL, target);
            //Initial analize values are set to target values
            float targetNiSalt = target.GetConcentrationGL(CmpType.NickelSulfate);
            float targetNiMetal = Model.NickelConverter.ConvertSaltToMetal(targetNiSalt);
            current.NickelAnalize = Model.NickelConverter.ConvertSaltToMetal(target.GetConcentrationGL(CmpType.NickelSulfate));
            current.HypophosphiteAnalize = target.GetConcentrationGL(CmpType.SodiumHypophosphite);
            //HP analize is used by default
            current.UseHPAnalize = true;

            return current;
        }
    }
}