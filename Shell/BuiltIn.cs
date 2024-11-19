using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell
{
    public abstract class BuiltIn
    {
        /// <summary>
        /// Specifies the which is used to invoke this built-in command.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Small description used to display along side the name in the help command.
        /// </summary>
        public abstract string Description { get; }

        public abstract ExitCode Execute(string[] args);

        protected string GetApplicationStorage()
        {
            return "";
        }
    }
}
