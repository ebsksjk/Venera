using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell
{
    public class BufferOut : OutputStream
    {
        public override byte[] Buffer { get; set; }

        public override void Write(string text)
        {
            // An buffer anfügen.
        }

        public override void WriteLine(string text)
        {
            throw new NotImplementedException();
        }
    }
}
