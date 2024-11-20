using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;
using Venera.vcurses;

namespace Venera.Kosmovim
{
    class kosmovim : Shell.BuiltIn
    {
        public override string Name => "vim"; //Venera Integrated Manipulator

        public override string Description => "edit plaintext files";

        public override ExitCode Execute(string[] args)
        {
            Console.Clear();
            Screen screen = new Screen();
            screen.stdscr.mvaddchar(10, 10, 'a');
            screen.refresh();
            Console.ReadLine();

            return ExitCode.Success;
        }
    }
}
