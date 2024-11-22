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
            screen.stdscr.box();
            //for(int i = 1; i < 10; i++)
            //{
            //    screen.stdscr.mvaddchar(i, i, (char)i);
            //}
            screen.stdscr.mvaddchar(screen.stdscr.xSize - 1, screen.stdscr.ySize - 1, 'X');
            screen.stdscr.mvaddchar(1, 1, 'A');
            Kernel.PrintDebug(new string(screen.stdscr.content));
            screen.refresh();
            Console.SetCursorPosition(1, 1);
            Console.ReadLine();

            return ExitCode.Success;
        }
    }
}
