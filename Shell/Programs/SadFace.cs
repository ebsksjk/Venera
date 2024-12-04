using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    class SadFace : BuiltIn
    {
        public override string Name => ":(";

        public override string Description => "sadfaces (venera blue screen equivalent)";

        public override CommandDescription ArgumentDescription => new();

        protected override ExitCode Execute()
        {
            WriteLine(":(");
            return ExitCode.Success;
        }
    }
}
