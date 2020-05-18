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
        public SolutionComposition(float volume, ComponentsInformation info)
        {
            this.info = info;

            this.volume = volume;
            AbsoluteWeigth = new Dictionary<SolutionComponents, float>();
            AbsoluteWeigth.Add(SolutionComponents.NickelSulfate, 0);
            AbsoluteWeigth.Add(SolutionComponents.SodiumHypophosphite, 0);
            AbsoluteWeigth.Add(SolutionComponents.SodiumAcetate, 0);
            AbsoluteWeigth.Add(SolutionComponents.SuccinicAcid, 0);
            AbsoluteWeigth.Add(SolutionComponents.LacticAcid, 0);
        }

        public float GetComponentValue(SolutionComponents component, ComponentUnits units) {
            switch (units) {
                case ComponentUnits.g:
                    return AbsoluteWeigth[component] * 1000;
                case ComponentUnits.g_l:
                    return (AbsoluteWeigth[component] * 1000) / volume;
                case ComponentUnits.kg:
                    return AbsoluteWeigth[component];
                case ComponentUnits.kg_l:
                    return AbsoluteWeigth[component] / volume;
                case ComponentUnits.l:
                    return AbsoluteWeigth[component] / info.GetDensity(component);
                case ComponentUnits.l_l:
                    return AbsoluteWeigth[component] / (info.GetDensity(component) * volume);
                case ComponentUnits.ml:
                    return AbsoluteWeigth[component] * 1000 / info.GetDensity(component);
                case ComponentUnits.ml_l:
                    return AbsoluteWeigth[component] * 1000/ (info.GetDensity(component) * volume);
                default:
                    return 0f;
            }
        }

        public void SetComponentValue(SolutionComponents component, float value, ComponentUnits units) {
            switch (units) {
                case ComponentUnits.g:
                    AbsoluteWeigth[component] = value / 1000;
                    break;
                case ComponentUnits.g_l:
                    AbsoluteWeigth[component] = value * volume / 1000;
                    break;
                case ComponentUnits.kg:
                    AbsoluteWeigth[component] = value;
                    break;
                case ComponentUnits.kg_l:
                    AbsoluteWeigth[component] = value * volume;
                    break;
                case ComponentUnits.l:
                    AbsoluteWeigth[component] = value * info.GetDensity(component);
                    break;
                case ComponentUnits.l_l:
                    AbsoluteWeigth[component] = value * volume * info.GetDensity(component);
                    break;
                case ComponentUnits.ml:
                    AbsoluteWeigth[component] = value * info.GetDensity(component) / 1000;
                    break;
                case ComponentUnits.ml_l:
                    AbsoluteWeigth[component] = value * info.GetDensity(component) * volume / 1000;
                    break;
                default:
                    break;
            }
        }
    }
}