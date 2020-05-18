using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    /// <summary>
    /// This class contains reference chemical and physical information about a solution component.
    /// </summary>
    public class ComponentInfo
    {
        public string FullName { get; }
        public string ShortName { get; }
        public string ChemicalFormula { get; }
        public float Density { get; set; }
        public float DefaultDensity { get; }

        public ComponentInfo(string FullName,
                                string ShortName,
                                string ChemicalFormula,
                                float Density,
                                float DefaultDensity)
        {
            this.FullName = FullName;
            this.ShortName = ShortName;
            this.ChemicalFormula = ChemicalFormula;
            this.Density = Density;
            this.DefaultDensity = DefaultDensity;
        }
    }
}
