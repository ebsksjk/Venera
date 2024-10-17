using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    internal class Reboot : BuiltIn
    {
        public override string Name => "reboot";

        public override string Description => "Reboot in n Seconds";

        public override ExitCode Execute(string[] args)
        {
            int wait = 60;
            if (args.Length > 0)
            {
                int.TryParse(args[0], out wait);
                if (args[0] == "now")
                {
                    wait = 0;
                }
            }
            Thread.Sleep(wait * 1000);
            Cosmos.System.Power.Reboot();
            return ExitCode.Success;
        }
    }
}