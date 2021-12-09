using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// Possible results of float input validation process.
    /// </summary>
    public enum ValidationState
    {
        NoError,
        Invalid,
        Zero,
        Negative,
        TooBig
    }
}
