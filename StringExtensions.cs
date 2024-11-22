using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;
using Venera.stasi;
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

        public static string AbsoluteOrRelativePath(this string str)
        {
            string path;
            string drive = str.Split(":").First();
            if (drive != str)
            {
                //if it is an absolute path
                path = $"{drive}:{str.Split(":").Last()}";
            }
            else
            {
                //if it is a relative path
                //convert it into the corresponding absolute path
                path = $"{Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory).EnsureBackslash()}{str}";
            }
            return path;
        }

            public static bool IsDriveId(this string str)
        {

            // The string must be exactly 3 characters long to match the pattern "[0-9]:\"
            if (str.Length != 3)
            {
                return false;
            }

            // Check if the first character is a digit (0-9)
            if (str[0] < '0' || str[0] > '9')
            {
                return false;
            }

            // Check if the second character is a colon ':'
            if (str[1] != ':')
            {
                return false;
            }

            return true;

        }

        //do only use this when you have paths... otherwise it would be **undefined behaviour**
        public static bool isAccessible(this string str)
        {
            //PANIC
            if(Login.curUser == null)
            {
                //PANIC
                return true;
            }

            //yes. I know this is a bad way to do it. But I am lazy and this works.
            if (Login.curUser.Username == "root")
            {
                return true;
            }

            if (str.Contains("0:\\Users\\"))
            {
                if (!str.Contains("0:\\Users\\" + Login.curUser.Username))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
