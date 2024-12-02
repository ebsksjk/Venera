using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;
using Venera.Kosmovim;

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
            int cH = wheight - 2;
            int cW = wwidth - 2;
            char[] content = new char[(wwidth - 2) * (wheight - 2)];
            for(int i = 0; i < wwidth - 2; i++)
            {
                for(int j = 0; j < wheight - 2; j++)
                {
                    content[cW * i + j] = ' ';
                }
            }
            ConsoleTextTweaks.ClearScreen();
            box();
            if((string)GetArgument(0) == "null")
            {
                ConsoleTextTweaks.PutString(1, 1, "No file specified", 0x1F);
                Console.Clear();
                return ExitCode.Error;
            }
            string f = (string)GetArgument(0);
            Kernel.PrintDebug($"file: {f}");
            string fCon = File.ReadAllText(f.AbsoluteOrRelativePath());
            Kernel.PrintDebug($"file con: {fCon}");
            for (int i = 0; i < fCon.Length; i++)
            {
                content[cW * curX + curY] = fCon[i];
                curX++;
                if (curX >= wwidth - 2)
                {
                    curX = 0;
                    curY++;
                }
                if (curY >= wheight - 2)
                {
                    break;
                }
            }
            while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                //get input
                ConsoleKeyInfo cki = Console.ReadKey(true);

                //handle input

                //draw screen
                for (int i = 0; i < wwidth - 2; i++)
                {
                    for(int j = 0; j < wheight - 2; j++)
                    {
                        ConsoleTextTweaks.PutChar(j, i, (byte)content[cW * i + j]);
                    }
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.Clear();

            return ExitCode.Success;
        }

        private static void box()
        {
            ConsoleTextTweaks.PutChar(0, 0, 0xC9);
            ConsoleTextTweaks.PutChar(0, wwidth, 0xBB);
            ConsoleTextTweaks.PutChar(wheight, 0, 0xC8);
            ConsoleTextTweaks.PutChar(wheight, wwidth, 0xBC);
            for(int i = 1; i < wwidth; i++)
            {
                ConsoleTextTweaks.PutChar(0, i, 0xCD);
                ConsoleTextTweaks.PutChar(wheight, i, 0xCD);
            }
            for(int i = 1; i < wheight; i++)
            {
                ConsoleTextTweaks.PutChar(i, 0, 0xBA);
                ConsoleTextTweaks.PutChar(i, wwidth, 0xBA);
            }
        }

        
    }
}
