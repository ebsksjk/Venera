using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;

namespace Venera.Kosmovim
{
    class KosmoVim : Shell.BuiltIn
    {
        public override string Name => "vim"; // Venera Integrated Manipulator

        public override string Description => "Edit plaintext files";

        public override CommandDescription ArgumentDescription => throw new NotImplementedException();

        protected override ExitCode Execute()
        {
            Console.ReadKey();

            return ExitCode.Success;
        }
    }
}
