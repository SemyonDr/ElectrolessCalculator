using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    /// <summary>
    /// This class represents solution composition by list of absolute weigths of its components
    /// and a bath volume which allows to calculate specific gravity and other relative values.
    /// </summary>
    public class SolutionComposition
    {
        //Total bath volume, including water.
        private float volume;
        //Components dictionary
        private Dictionary<BathComponents, float> AbsoluteWeigth;

        public float Volume {
            get { return volume; }
            set { volume = value; }
            }

        public float GetAbsoluteWeigth(BathComponents Component)
        {
            return AbsoluteWeigth[Component];
        }

        public float GetConcentration(BathComponents Component)
        {
            return AbsoluteWeigth[Component] / volume;
        }

        public void SetAbsoluteWeigth(BathComponents Component, float value)
        {
            if (Component == BathComponents.NickelMetal)
            {
                AbsoluteWeigth[BathComponents.NickelSulfate] = NickelConverter.ConvertMetalToSalt(value);
            }
            else
            {
                AbsoluteWeigth[Component] = value;
            }
        }

        public void SetConcentration(BathComponents Component, float value)
        {
            if (Component == BathComponents.NickelMetal)
            {
                AbsoluteWeigth[BathComponents.NickelSulfate] = NickelConverter.ConvertMetalToSalt(value) * volume;
            }
            else
            {
                AbsoluteWeigth[Component] = value * volume;
            }
        }

        public SolutionComposition()
        {
            volume = 1;
            AbsoluteWeigth = new Dictionary<BathComponents, float>();
            AbsoluteWeigth.Add(BathComponents.NickelSulfate, 0);
            AbsoluteWeigth.Add(BathComponents.SodiumHypophosphite, 0);
            AbsoluteWeigth.Add(BathComponents.SodiumAcetate, 0);
            AbsoluteWeigth.Add(BathComponents.SuccinicAcid, 0);
            AbsoluteWeigth.Add(BathComponents.LacticAcid, 0);
        }

        public SolutionComposition(float SolutionVolume, float MetalNickelConcentration, SolutionComposition TargetComposition)
        {
            this.volume = SolutionVolume;
            float proportion = NickelConverter.ConvertMetalToSalt(MetalNickelConcentration) / TargetComposition.GetConcentration(BathComponents.NickelSulfate);

            foreach (BathComponents comp in AbsoluteWeigth.Keys)
            {
                AbsoluteWeigth[comp] = proportion * volume * TargetComposition.GetConcentration(comp);
            }
        }
    }
}