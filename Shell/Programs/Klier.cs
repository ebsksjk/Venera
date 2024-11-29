using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    public class Klier : BuiltIn
    {
        public override string Name => "klier";

        public override string Description => "kliers the screen";

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
