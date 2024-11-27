using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{

    
    
    class Pride : BuiltIn
    {
        public override string Name => "pride";

        public override string Description => "prides the screen";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "trans",
                    description: "trans rights?",
                    shortForm: 't',
                    type: typeof(bool)
                ),
                new(
                    valueName: "enby",
                    description: "nb rights?",
                    shortForm: 'n',
                    type: typeof(bool)
                ),
                new(
                    valueName: "gay",
                    description: "gay rights?",
                    shortForm: 'g',
                    type: (typeof(bool))
                ),
                new(
                    valueName: "bi",
                    description: "bi rights?",
                    shortForm: 'b',
                    type: (typeof(bool))
                ),
                new(
                    valueName: "name",
                    description: "someone's name...",
                    argsPosition: 0,
                    valueDefault: "null",
                    type: typeof(string))
            ]
        };

        protected override ExitCode Execute()
        {

            bool trans = (bool)GetArgument("t");
            bool nb = (bool)GetArgument("n");
            bool gay = (bool)GetArgument("g");
            bool bi = (bool)GetArgument("b");

            string name = (string)GetArgument(0);
            switch (name.ToLower())
            {
                case "mara":
                    trans = true;
                    break;

                case "niklas":
                    nb = true;
                    break;

                case "nicolas":
                    gay = true;
                    break;

                default:
                    break;
            }

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            if (trans)
            {
                gays.getTransPride(Console.WindowHeight).ForEach(l => l.draw());
            }else if (nb)
            {
                gays.getNonBinaryPride(Console.WindowHeight).ForEach(l => l.draw());
            } else if (gay)
            {
                gays.getGayPride(Console.WindowHeight).ForEach(l => l.draw());
            }
            else { 
                gays.getRainbowPride(Console.WindowHeight).ForEach(l => l.draw()); 
            }
            
            Console.ReadLine();
            return ExitCode.Success;
        }
        
    }

    class Pixel
    {
        public char c;
        public ConsoleColor color;
        public Pixel(char _c, ConsoleColor _color)
        {
            c = _c;
            color = _color;
        }
    }

    class Line
    {
        public int height;
        public ConsoleColor color;

        public Line(int _height, ConsoleColor _color)
        {
            height = _height;
            color = _color;
        }

        public void draw()
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < height; i++)
            {
                for(int j = 0; j < Console.WindowWidth; j++)
                {
                    Console.Write("█");
                }
            }
        }
    }

    static class gays
    {
        //height is the height of the console window
        public static List<Line> getRainbowPride(int height)
        {
            int lineHeight = height / 7;
            Kernel.PrintDebug($"Line height: {lineHeight}; Console height: {height}");
            List<Line> lines = new List<Line>();

            lines.Add(new Line(lineHeight, ConsoleColor.Red));
            lines.Add(new Line(lineHeight, ConsoleColor.DarkRed));
            lines.Add(new Line(lineHeight, ConsoleColor.DarkYellow));
            lines.Add(new Line(lineHeight, ConsoleColor.Yellow));
            lines.Add(new Line(lineHeight, ConsoleColor.Green));
            lines.Add(new Line(lineHeight, ConsoleColor.Blue));
            lines.Add(new Line(lineHeight, ConsoleColor.DarkBlue));
            return lines;
        }

        public static List<Line> getTransPride(int height)
        {
            int lineHeight = height / 5;
            Kernel.PrintDebug($"Line height: {lineHeight}; Console height: {height}");
            List<Line> lines = new List<Line>();
            lines.Add(new Line(lineHeight, ConsoleColor.Blue));
            lines.Add(new Line(lineHeight, ConsoleColor.Magenta));
            lines.Add(new Line(lineHeight, ConsoleColor.White));
            lines.Add(new Line(lineHeight, ConsoleColor.Magenta));
            lines.Add(new Line(lineHeight, ConsoleColor.Blue));
            return lines;
        }

        public static List<Line> getNonBinaryPride(int height)
        {
            int lineHeight = height / 4;
            Kernel.PrintDebug($"Line height: {lineHeight}; Console height: {height}");
            List<Line> lines = new List<Line>();
            lines.Add(new Line(lineHeight, ConsoleColor.Yellow));
            lines.Add(new Line(lineHeight, ConsoleColor.White));
            lines.Add(new Line(lineHeight, ConsoleColor.DarkMagenta));
            lines.Add(new Line(lineHeight, ConsoleColor.Black));
            return lines;
        }

        public static List<Line> getGayPride(int height)
        {
            int lineHeight = height / 7;
            Kernel.PrintDebug($"Line height: {lineHeight}; Console height: {height}");
            List<Line> lines = new List<Line>();
            lines.Add(new Line(lineHeight, ConsoleColor.DarkCyan));
            lines.Add(new Line(lineHeight, ConsoleColor.Cyan));
            lines.Add(new Line(lineHeight, ConsoleColor.Gray));
            lines.Add(new Line(lineHeight, ConsoleColor.White));
            lines.Add(new Line(lineHeight, ConsoleColor.Gray));
            lines.Add(new Line(lineHeight, ConsoleColor.Blue));
            lines.Add(new Line(lineHeight, ConsoleColor.DarkGray));
            return lines;
        }

        public static List<Line> getBiFlag(int height)
        {
            int lineHeight = height / 5;
            Kernel.PrintDebug($"Line height: {lineHeight}; Console height: {height}");
            List<Line> lines = new List<Line>();
            lines.Add(new Line(lineHeight, ConsoleColor.Magenta));
            lines.Add(new Line(lineHeight, ConsoleColor.Magenta));
            lines.Add(new Line(lineHeight, ConsoleColor.DarkMagenta));
            lines.Add(new Line(lineHeight, ConsoleColor.Blue));
            lines.Add(new Line(lineHeight, ConsoleColor.Blue));
            return lines;
        }
    }
}
