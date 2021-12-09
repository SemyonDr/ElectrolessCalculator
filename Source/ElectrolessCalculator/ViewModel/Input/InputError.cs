using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// Represents input error message.
    /// </summary>
    public class InputError
    {
        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Error source.
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Source">Text that represents error source.</param>
        /// <param name="Message">Error message.</param>
        public InputError(string Source, string Message) {
            this.Message = Message;
            this.Source = Source;
        }
    }
}
