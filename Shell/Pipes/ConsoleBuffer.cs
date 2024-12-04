using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Pipes
{
    public class ConsoleBuffer : OutputBuffer
    {
        public override byte[] Buffer { get => null; }

        public override void Write(string text)
        {
            Console.Write(text);
        }

        public override void Write(char text)
        {
            Console.Write(text);
        }

        public override void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
