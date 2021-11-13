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
            //Loading Model data
            Model.TargetSolution targetSolution = LoadTargetSolution();
            Model.CurrentSolution currentSolution = LoadCurrentSolution(300, targetSolution);
            Model.RequiredMaterials requiredMaterials = new Model.RequiredMaterials(targetSolution, currentSolution);

            //Creating ViewModels
            ViewModel.TargetSolution_ViewModel targetSolution_VM = new ViewModel.TargetSolution_ViewModel(targetSolution);
            ViewModel.CurrentSolution_ViewModel currentSolution_VM = new ViewModel.CurrentSolution_ViewModel(currentSolution, targetSolution_VM);
            ViewModel.RequiredMaterials_VM requiredMaterials_VM = new ViewModel.RequiredMaterials_VM(requiredMaterials, targetSolution_VM, currentSolution_VM);
            
            //Creating Windows
            View.MainWindow mainWindow = new View.MainWindow();

            //Setting data context
            mainWindow.TargetSolution.DataContext = targetSolution_VM;
            mainWindow.TargetVolumePresenter.DataContext = targetSolution_VM;
            mainWindow.TargetEditPanel.DataContext = targetSolution_VM;
            mainWindow.TopTargetError.DataContext = targetSolution_VM;

            mainWindow.AnalizePanel.DataContext = currentSolution_VM;
            mainWindow.CurrentVolumePresenter.DataContext = currentSolution_VM;
            mainWindow.CurrentComposition.DataContext = currentSolution_VM;

            mainWindow.RequiredComponents.DataContext = requiredMaterials_VM;
            
            mainWindow.Show();
        }


        private Model.TargetSolution LoadTargetSolution() {
            Model.TargetSolution target = new Model.TargetSolution(300);
            Model.ComponentFactory cf = new Model.ComponentFactory();
            target.Components.Add(CmpType.NickelSulfate, cf.CreateComponent(CmpType.NickelSulfate, 9.3993f));
            target.Components.Add(CmpType.SodiumHypophosphite, cf.CreateComponent(CmpType.SodiumHypophosphite, 6.9f));
            target.Components.Add(CmpType.SodiumAcetate, cf.CreateComponent(CmpType.SodiumAcetate, 20));
            target.Components.Add(CmpType.SuccinicAcid, cf.CreateComponent(CmpType.SuccinicAcid, 20));
            target.Components.Add(CmpType.LacticAcid, cf.CreateComponent(CmpType.LacticAcid, 20));
            return target;
        }

        private Model.CurrentSolution LoadCurrentSolution(float volume, Model.TargetSolution target) {
            Model.CurrentSolution current = new Model.CurrentSolution(volume, target);
            float targetNiSalt = target.GetConcentration(CmpType.NickelSulfate);
            float targetNiMetal = Model.NickelConverter.ConvertSaltToMetal(targetNiSalt);
            current.NickelAnalize = Model.NickelConverter.ConvertSaltToMetal(target.GetConcentration(CmpType.NickelSulfate));
            current.HypophosphiteAnalize = target.GetConcentration(CmpType.SodiumHypophosphite);

            return current;
        }
    }
}