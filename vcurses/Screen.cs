using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.vcurses
{
    //A screen is the physical output device of the terminal
    class Screen
    {
        public Window stdscr;

        public void refresh()
        {
            //stdscr.refresh();
            Console.SetCursorPosition(stdscr.xPos, stdscr.yPos);
            Console.Write(new string(stdscr.content));
        }

        public Screen()
        {
            stdscr = new Window(' ', Console.WindowWidth, Console.WindowHeight, 0, 0);
        }
    }
}
