using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera
{
    public enum ExitCode
    {
        /// <summary>
        /// The program has finished successfully.
        /// </summary>
        Success = 0,

        /// <summary>
        /// An unknown error occured and the program had to exit early.
        /// </summary>
        Error = 1,
    }
}
