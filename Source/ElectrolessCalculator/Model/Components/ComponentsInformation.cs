using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    /// <summary>
    /// This class is used as the accessor to reference chemical information for set of a components.
    /// </summary>
    public class ComponentsInformation
    {
        private Dictionary<SolutionComponents, ComponentInfo> components;

        public ComponentsInformation() {
            components = new Dictionary<SolutionComponents, ComponentInfo>();
        }


        public void AddComponentInfo(SolutionComponents component, ComponentInfo info) {
            components.Add(component, info);
        }

        public float GetDensity(SolutionComponents component) {
            return components[component].Density;
        }

        public void SetDensity(SolutionComponents component, float value) {
            components[component].Density = value;
        }

        public float GetDefaultDensity(SolutionComponents component) {
            return components[component].DefaultDensity;
        }

        public void ResetDensityToDefault(SolutionComponents component) {
            components[component].Density = components[component].DefaultDensity;
        }

        public string GetShortName(SolutionComponents component) {
            return components[component].ShortName;
        }

        public string GetFullName(SolutionComponents component) {
            return components[component].FullName;
        }

        public string GetChemicalFormula(SolutionComponents component) {
            return components[component].ChemicalFormula;
        }
    }
}
