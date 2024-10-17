using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Venera
{
    public static class StringExtensions
    {
        public static string Pad(this string str, int padding)
        {
            int freeSpace = padding - str.Length;

            if (freeSpace < 0)
            {
                str = str.Substring(0, padding - 3);
                str += "...";
            }
            else
            {
                for (int i = 0; i < freeSpace; i++)
                {
                    str += " ";
                }
            }

            return str;
        }

        public static string EnsureBackslash(this string str)
        {
            if (!str.EndsWith(@"\"))
            {
                str += @"\";
            }
            return str;
        }
    }
}
