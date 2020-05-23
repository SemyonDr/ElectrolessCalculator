using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.Model
{
    /// <summary>
    /// This class represents solution composition.
    /// </summary>
    public class Solution
    {
        //Components list, **WEIGTHS ARE IN KG**
        public List<Component> Components { get; }
     
        //Total bath volume, including water. ***VOLUME IS IN LITERS***
        private float totalVolume;

        public float TotalVolume {
            get { return totalVolume; }
            set { totalVolume = value; }
            }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Solution(float TotalVolume) {
            this.totalVolume = TotalVolume;
            Components = new List<Component>();
        }
    }
}