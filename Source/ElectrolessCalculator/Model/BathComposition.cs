using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    public class SolutionComposition
    {
        private float volume;

        private Dictionary<BathComponents, float> AbsoluteWeigth;

        public float Volume {
            get { return volume; }
            set { volume = value; }
            }

        public float GetAbsoluteWeigth(BathComponents Component)
        {
            return AbsoluteWeigth[Component];
        }

        public float GetSpecificGravity(BathComponents Component)
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

        public void SetSpecificGravity(BathComponents Component, float value)
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

        public SolutionComposition(float SolutionVolume, float MetalNickelSpecificMass, SolutionComposition TargetComposition)
        {
            this.volume = SolutionVolume;
            float proportion = NickelConverter.ConvertMetalToSalt(MetalNickelSpecificMass) / TargetComposition.GetSpecificGravity(BathComponents.NickelSulfate);

            foreach (BathComponents comp in AbsoluteWeigth.Keys)
            {
                AbsoluteWeigth[comp] = proportion * volume * TargetComposition.GetSpecificGravity(comp);
            }
        }
    }
}