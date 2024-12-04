using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell
{
    public class ConsoleOut : OutputStream
    {
        public override byte[] Buffer { get => null; set => throw new NotImplementedException(); }

        public override void Write(string text)
        {
            Console.Write(text);
        }

        public override void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
