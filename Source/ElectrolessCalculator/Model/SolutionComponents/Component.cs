using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    /// <summary>
    /// Model representation of the material.
    /// </summary>
    public abstract class Component {
        public float WeigthKg { get; set; }
        public string FullName { get; }
        public string ShortName { get; }
        public string ChemicalFormula { get; }
        public float Density { get; }

        public Component(float WeigthKg, string FullName, string ShortName, string ChemicalFormula, float Density) {
            this.WeigthKg = WeigthKg;
            this.FullName = FullName;
            this.ShortName = ShortName;
            this.ChemicalFormula = ChemicalFormula;
            this.Density = Density;
        }
    }
}