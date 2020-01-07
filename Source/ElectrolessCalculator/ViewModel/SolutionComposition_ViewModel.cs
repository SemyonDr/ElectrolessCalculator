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
        //Specific gravitys
        //================
        public float NickelMetalSpecificGravity {
            get {
                return composition.GetSpecificGravity(Model.BathComponents.NickelMetal);
            }
            set {
                composition.SetSpecificGravity(Model.BathComponents.NickelMetal, value);
                NotifyPropertyChanged("NickelMetalSpecificGravity");
            }}

        public float SodiumHypophosphiteSpecificGravity {
            get {
                return composition.GetSpecificGravity(Model.BathComponents.SodiumHypophosphite);
            }
            set {
                composition.SetSpecificGravity(Model.BathComponents.SodiumHypophosphite, value);
                NotifyPropertyChanged("SodiumHypophosphiteSpecificGravity");
            }
        }

        public float SodiumAcetateSpecificGravity {
            get {
                return composition.GetSpecificGravity(Model.BathComponents.SodiumAcetate);
            }
            set {
                composition.SetSpecificGravity(Model.BathComponents.SodiumAcetate, value);
                NotifyPropertyChanged("SodiumAcetateSpecificGravity");
            }}

        public SolutionComposition_ViewModel(Model.SolutionComposition composition)
        {
            this.composition = composition;
        }
    }
}
