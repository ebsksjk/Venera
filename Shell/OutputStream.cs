using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell
{
    public abstract class OutputStream
    {
        public abstract byte[] Buffer { get; set; }

        public abstract void WriteLine(string text);

        public abstract void Write(string text);

    }
}
