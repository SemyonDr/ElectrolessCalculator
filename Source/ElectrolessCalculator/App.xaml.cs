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
            Model.ComponentsInformation info = LoadComponentsInfo();
            Model.SolutionComposition TargetComposition = LoadTargerComposition(info);

            //Creating ViewModels
            ViewModel.SolutionComposition_ViewModel TargetComposition_VM = new ViewModel.SolutionComposition_ViewModel(TargetComposition);

            //Creating Windows
            View.MainWindow mainWindow = new View.MainWindow();

            //Setting data context
            mainWindow.TargetComposition.DataContext = TargetComposition_VM;

            
            mainWindow.Show();
        }

        private Model.ComponentsInformation LoadComponentsInfo() {
            Model.ComponentsInformation info = new Model.ComponentsInformation();
            Model.ComponentInfo nickel_metal_info = new Model.ComponentInfo(
                "Nickel Metal",
                "Nickel",
                "Ni",
                8.908f,
                8.908f
                );
            Model.ComponentInfo nickel_info = new Model.ComponentInfo(
                "Nickel(II) Sulfate Hexahydrate",
                "Nickel Sulfate",
                "NiSO4*(H2O)6",
                2.07f,
                2.07f);
            Model.ComponentInfo hypo_info = new Model.ComponentInfo(
                "Sodium Hypophosphite Anhydrous",
                "Sodium Hypophosphite",
                "NaPO2H2",
                0.8f,
                0.8f);
            Model.ComponentInfo acetate_info = new Model.ComponentInfo(
                "Sodium Acetate Trihydrate",
                "Sodium Acetate",
                "C2H3NaO2(H20)3",
                1.45f,
                1.45f);
            Model.ComponentInfo succinic_info = new Model.ComponentInfo(
                "Succinic Acid",
                "Succinic Acid",
                "C4H6O4",
                1.56f,
                1.56f);
            Model.ComponentInfo lactic_info = new Model.ComponentInfo(
                "Lactic Acid",
                "Lactic Acid",
                "C3H6O3",
                1.206f,
                1.206f);

            info.AddComponentInfo(Model.SolutionComponents.NickelMetal, nickel_metal_info);
            info.AddComponentInfo(Model.SolutionComponents.NickelSulfate, nickel_info);
            info.AddComponentInfo(Model.SolutionComponents.SodiumHypophosphite, hypo_info);
            info.AddComponentInfo(Model.SolutionComponents.SodiumAcetate, acetate_info);
            info.AddComponentInfo(Model.SolutionComponents.SuccinicAcid, succinic_info);
            info.AddComponentInfo(Model.SolutionComponents.LacticAcid, lactic_info);

            return info;
        }

        private Model.SolutionComposition LoadTargerComposition(Model.ComponentsInformation info) {
            Model.SolutionComposition TargetComposition = new Model.SolutionComposition(1, info);
            TargetComposition.AddComponent(Model.SolutionComponents.NickelMetal, 6, Model.ComponentUnits.g_l);
            TargetComposition.AddComponent(Model.SolutionComponents.SodiumHypophosphite, 25, Model.ComponentUnits.g_l);
            TargetComposition.AddComponent(Model.SolutionComponents.SodiumAcetate, 10, Model.ComponentUnits.g_l);
            TargetComposition.AddComponent(Model.SolutionComponents.SuccinicAcid, 15, Model.ComponentUnits.g_l);
            TargetComposition.AddComponent(Model.SolutionComponents.LacticAcid, 10, Model.ComponentUnits.ml_l);

            return TargetComposition;
        }
    }
}