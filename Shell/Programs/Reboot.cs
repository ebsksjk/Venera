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

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "timeout",
                    description: "Reboot in n seconds",
                    argsPosition: 0,
                    valueDefault: "60",
                    type: typeof(string)
                )
            ]
        };

        protected override ExitCode Execute()
        {
            string waitArgs = (string)GetArgument(0);

            int.TryParse(waitArgs, out int wait);
            if (waitArgs == "now")
            {
                wait = 0;
            }

            Thread.Sleep(wait * 1000);
            Cosmos.System.Power.Reboot();
            return ExitCode.Success;
        }
    }
}