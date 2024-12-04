using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera
{
    public static class IntegerExtensions
    {
        // See https://stackoverflow.com/questions/1318933/c-sharp-int-to-byte
        // We can't use BitConverter in CosmosOS, so this is a good workaround.
        public static byte[] ToBytes(this int value)
        {
            byte[] bytes =
            [
                (byte)(value & 0xFF),
                (byte)((value >> 8) & 0xFF),
                (byte)((value >> 16) & 0xFF),
                (byte)((value >> 24) & 0xFF),
            ];
            return bytes;
        }

    }
}
