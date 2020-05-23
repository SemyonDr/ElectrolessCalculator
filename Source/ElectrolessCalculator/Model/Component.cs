using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    public class Component {
        public float Weigth { get; set; }
        public string FullName { get; }
        public string ShortName { get; }
        public string ChemicalFormula { get; }
        public float Density { get; set; }

        public Component(string FullName, string ShortName, string ChemicalFormula, float Weigth, float Density) {
            this.Weigth = Weigth;
            this.FullName = FullName;
            this.ShortName = ShortName;
            this.ChemicalFormula = ChemicalFormula;
            this.Density = Density;
        }
    }
}