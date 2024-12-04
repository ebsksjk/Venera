using System.Collections.Generic;
using System.Text;

namespace Venera.Shell.Pipes
{
    public class PipedBuffer : OutputBuffer
    {
        public override byte[] Buffer { get => _buffer.ToArray(); }

        private List<byte> _buffer;

        public PipedBuffer()
        {
            _buffer = new List<byte>();
        }

        public override void Write(string text)
        {
            _buffer.AddRange(Encoding.ASCII.GetBytes(text));
        }

        public override void Write(char text)
        {
            _buffer.Add((byte)text);
        }

        public override void WriteLine(string text)
        {
            Write($"{text}\n");
        }
    }
}
