using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosELFCore
{
    public unsafe class MemoryStream : Stream
    {
        public byte* Pointer;

        public MemoryStream(byte* data)
        {
            Pointer = data;
        }
        public override void Write(byte dat)
        {
            Pointer[Position++] = dat;
        }
        public override int Read()
        {
            return Pointer[Position++];
        }
    }
}
