using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    public class Clear : BuiltIn
    {
        public override string Name => "clear";

        public override string Description => "Clears the screen";

        public override CommandDescription ArgumentDescription => new();

        protected override ExitCode Execute()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            return ExitCode.Success;
        }
    }
}
