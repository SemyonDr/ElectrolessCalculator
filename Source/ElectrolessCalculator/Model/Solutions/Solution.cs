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
    public abstract class Solution
    {
        //Components list, **WEIGTHS ARE IN KG**
        public Dictionary<CmpType, Component> Components { get; }
     
        //Total bath volume, including water. ***VOLUME IS IN LITERS***
        private float totalVolumeL;

        public float TotalVolumeL {
            get { return totalVolumeL; }
            set { totalVolumeL = value; }
            }

        /// <summary>
        /// Returns concentration of the component in grams per liter.
        /// </summary>
        /// <param name="CType">Component type.</param>
        /// <returns></returns>
        public float GetConcentrationGL(CmpType CType) {
            return (Components[CType].WeigthKg * 1000) / totalVolumeL;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Solution(float BathVolume)
        {
            //Initializing components dictionary
            Components = new Dictionary<CmpType, Component>();

            this.totalVolumeL = BathVolume;
        }
    }
}