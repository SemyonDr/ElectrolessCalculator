using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    public class SolutionComposition_ViewModel : ViewModelBase
    {
        Model.SolutionComposition composition;

        //================
        //Absolute weigths
        //================
        public float NickelMetalAbsoluteWeigth {
            get {
                return composition.GetAbsoluteWeigth(Model.BathComponents.NickelMetal);
            }
            set{
                composition.SetAbsoluteWeigth(Model.BathComponents.NickelMetal, value);
                NotifyPropertyChanged("NickelMetalAbsoluteWeigth");
            }}

        public float NickelSulfateAbsoluteWeigth{
            get{
                return composition.GetAbsoluteWeigth(Model.BathComponents.NickelSulfate);
            }
            set{
                composition.SetAbsoluteWeigth(Model.BathComponents.NickelSulfate, value);
                NotifyPropertyChanged("NickelSulfateAbsoluteWeigth");
            }}

        public float SodiumHypophosphiteAbsoluteWeigth{
            get {
                return composition.GetAbsoluteWeigth(Model.BathComponents.SodiumHypophosphite);
            }
            set {
                composition.SetAbsoluteWeigth(Model.BathComponents.SodiumHypophosphite, value);
                NotifyPropertyChanged("SodiumHypophosphiteAbsoluteWeigth");
            }}

        //================
        //Concentrations
        //================
        public float NickelMetalConcentration {
            get {
                return composition.GetConcentration(Model.BathComponents.NickelMetal);
            }
            set {
                composition.SetConcentration(Model.BathComponents.NickelMetal, value);
                NotifyPropertyChanged("NickelMetalConcentration");
            }}

        public float SodiumHypophosphiteConcentration {
            get {
                return composition.GetConcentration(Model.BathComponents.SodiumHypophosphite);
            }
            set {
                composition.SetConcentration(Model.BathComponents.SodiumHypophosphite, value);
                NotifyPropertyChanged("SodiumHypophosphiteConcentration");
            }
        }

        public float SodiumAcetateConcentration {
            get {
                return composition.GetConcentration(Model.BathComponents.SodiumAcetate);
            }
            set {
                composition.SetConcentration(Model.BathComponents.SodiumAcetate, value);
                NotifyPropertyChanged("SodiumAcetateConcentration");
            }}

        public float SuccinicAcidConcentration {
            get{
                return composition.GetConcentration(Model.BathComponents.SuccinicAcid);
            }
            set{
                composition.SetConcentration(Model.BathComponents.SuccinicAcid, value);
                NotifyPropertyChanged("SuccinicAcidConcentration");
            }}

        public float LacticAcidConcentration {
            get {
                return composition.GetConcentration(Model.BathComponents.LacticAcid);
            }
            set {
                composition.SetConcentration(Model.BathComponents.LacticAcid, value);
                NotifyPropertyChanged("LacticAcidConcentration");
            }}

        public SolutionComposition_ViewModel(Model.SolutionComposition composition)
        {
            this.composition = composition;
        }
    }
}
