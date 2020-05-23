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
            Model.Solution targetComposition = LoadTargetComposition();
            Model.Solution currentComposition = Model.Calculator.GetCurrentSolution(1.1f, targetComposition.TotalVolume / 3, targetComposition);

            //Creating ViewModels
            ViewModel.Solution_ViewModel targetSolution_VM = new ViewModel.Solution_ViewModel(targetComposition);
            ViewModel.Solution_ViewModel currentSolution_VM = new ViewModel.Solution_ViewModel(currentComposition);

            //Creating Windows
            View.MainWindow mainWindow = new View.MainWindow();

            //Setting data context
            mainWindow.TargetComposition.DataContext = targetSolution_VM;
            mainWindow.TargetVolumePresenter.DataContext = targetSolution_VM;
            mainWindow.TargetEditPanel.DataContext = targetSolution_VM;

            mainWindow.CurrentComposition.DataContext = currentSolution_VM;
            
            mainWindow.Show();
        }


        private Model.Solution LoadTargetComposition() {
            Model.Solution target = new Model.Solution(300);
            target.Components.Add(
                new Model.Component(
                    "Nickel(II) Sulfate Hexahydrate",
                    "Nickel Sulfate",
                    "NiSO4*(H2O)6",
                    7.5f,
                    2.07f));
            target.Components.Add(
                new Model.Component(
                    "Sodium Hypophosphite Anhydrous",
                    "Sodium Hypophosphite",
                    "NaPO2H2",
                    7.8f,
                    0.8f));
            target.Components.Add(
                new Model.Component(
                    "Sodium Acetate Trihydrate",
                    "Sodium Acetate",
                    "C2H3NaO2(H20)3",
                    10f,
                    1.45f));
            target.Components.Add(
                new Model.Component(
                    "Succinic Acid",
                    "Succinic Acid",
                    "C4H6O4",
                    12f,
                    1.56f));
            target.Components.Add(
                new Model.Component(
                    "Lactic Acid",
                    "Lactic Acid",
                    "C3H6O3",
                    10f,
                    1.206f));

            return target;
        }
    }
}