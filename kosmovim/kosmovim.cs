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

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "file",
                    description: "the file to edit",
                    argsPosition: 0,
                    valueDefault: "null",
                    type: typeof(string))
            ]
        };

        private static int wwidth = Console.WindowWidth - 1;
        private static int wheight = Console.WindowHeight - 1;
        private static int curY = 0;
        private static int curX = 0;

        protected override ExitCode Execute()
        {
            
            ClearScreen();
            box();
            if((string)GetArgument(0) == "null")
            {
                PutString(1, 1, "No file specified", 0x1F);
                Console.Clear();
                return ExitCode.Error;
            }
            string f = (string)GetArgument(0);
            Kernel.PrintDebug($"file: {f}");
            string fCon = File.ReadAllText(f.AbsoluteOrRelativePath());
            Kernel.PrintDebug($"file con: {fCon}");
            (curX, curY) = (1, 1);
            PutString(1, 1, fCon);
            Console.SetCursorPosition(10, 10);
            Console.ReadKey();
            Console.SetCursorPosition(0, 0);
            Console.Clear();

            return ExitCode.Success;
        }

        private static void box()
        {
            PutChar(0, 0, (char)0xC9);
            PutChar(0, wwidth, (char)0xBB);
            PutChar(wheight, 0, (char)0xC8);
            PutChar(wheight, wwidth, (char)0xBC);
            for(int i = 1; i < wwidth; i++)
            {
                PutChar(0, i, (char)0xCD);
                PutChar(wheight, i, (char)0xCD);
            }
            for(int i = 1; i < wheight; i++)
            {
                PutChar(i, 0, (char)0xBA);
                PutChar(i, wwidth, (char)0xBA);
            }
        }

        private static void PutChar(int line, int col, char c, int color = 0x1F)
        {
            unsafe
            {
                byte* xAddress = (byte*)0xB8000;

                xAddress += (line * 80 + col) * 2;

                xAddress[0] = (byte)c;
                xAddress[1] = (byte)color;
            }
        }

        private static void PutString(int line, int startCol, string msg, int color = 0x1F)
        {
            int wX = startCol;
            int wY = line;
            for (int i = 0; i < msg.Length; i++)
            {
                if(wX >= wwidth)
                {
                    wX = 1;
                    wY++;
                }
                if(wY >= wheight)
                {
                    return; //not implemented yet :) but eventually, we do need to scroll...
                }
                PutChar(wY, wX, msg[i], color);
                wX++;
            }
        }

        private static void ClearScreen()
        {
            for (int i = 0; i < 80; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    PutChar(j, i, ' ', 0x1F);
                }
            }
        }
    }
}
