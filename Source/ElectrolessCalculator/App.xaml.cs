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
            Model.SolutionComposition TargetComposition = new Model.SolutionComposition();
            TargetComposition.SetConcentration(Model.BathComponents.NickelMetal, 6);
            TargetComposition.SetConcentration(Model.BathComponents.SodiumHypophosphite, 25);
            TargetComposition.SetConcentration(Model.BathComponents.SodiumAcetate, 10);
            TargetComposition.SetConcentration(Model.BathComponents.SuccinicAcid, 15);
            TargetComposition.SetConcentration(Model.BathComponents.LacticAcid, 10);

            ViewModel.SolutionComposition_ViewModel TargetComposition_VM = new ViewModel.SolutionComposition_ViewModel(TargetComposition);

            View.MainWindow mainWindow = new View.MainWindow();


            mainWindow.TargetCompositionPresenter.DataContext = TargetComposition_VM;

            mainWindow.Show();
        }
    }
}
