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
        public ComponentsInformation info { get; }

        //Total bath volume, including water. ***VOLUME IS IN LITERS***
        private float volume;
        //Components dictionary ***WEIGHT IS IN KG***
        private Dictionary<SolutionComponents, float> AbsoluteWeigth;

        public List<SolutionComponents> ComponentsList {
            get { return AbsoluteWeigth.Keys.ToList(); }
        }

        public float Volume {
            get { return volume; }
            set { volume = value; }
            }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SolutionComposition(float volume, ComponentsInformation info) {
            this.info = info;
            this.volume = volume;
            AbsoluteWeigth = new Dictionary<SolutionComponents, float>();
        }

        public void AddComponent(SolutionComponents component, float value, ComponentUnits units) {
            switch (component) {
                case SolutionComponents.NickelMetal: { 
                        float metal_kg = UnitsConverter.ConvertToAbsolute(value, volume, units, info.GetDensity(SolutionComponents.NickelMetal));
                        float salt_kg = NickelConverter.ConvertMetalToSalt(metal_kg);
                        AbsoluteWeigth.Add(SolutionComponents.NickelMetal, metal_kg);
                        AbsoluteWeigth.Add(SolutionComponents.NickelSulfate, salt_kg);
                        return;
                    }
                case SolutionComponents.NickelSulfate: { 
                        float salt_kg = UnitsConverter.ConvertToAbsolute(value, volume, units, info.GetDensity(SolutionComponents.NickelSulfate));
                        float metal_kg = NickelConverter.ConvertSaltToMetal(salt_kg);
                        AbsoluteWeigth.Add(SolutionComponents.NickelMetal, metal_kg);
                        AbsoluteWeigth.Add(SolutionComponents.NickelSulfate, salt_kg);
                        return;
                    }
                default: {
                        AbsoluteWeigth.Add(
                            component,
                            UnitsConverter.ConvertToAbsolute(value, volume, units, info.GetDensity(component))
                            );
                        break;
                    }
            }
        }

        public float GetComponentValue(SolutionComponents component, ComponentUnits units) {
            return UnitsConverter.ConvertFromAbsolute(AbsoluteWeigth[component], volume, units, info.GetDensity(component));
        }

        public void SetComponentValue(SolutionComponents component, float value, ComponentUnits units) {
            AbsoluteWeigth[component] = UnitsConverter.ConvertToAbsolute(value, volume, units, info.GetDensity(component));
        }
    }
}