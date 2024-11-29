using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;

namespace Venera.Kosmovim
{
    class GenJunk : BuiltIn
    {
        public override string Name => "genjunk";

        public override string Description => "generate junk file";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "nc",
                    description: "Character count, how much to generate.",
                    type: typeof(int),
                    required: true,
                    argsPosition: 0
                ),
                new(
                    valueName: "file",
                    description: "Output file",
                    type: typeof(string),
                    required: true,
                    argsPosition: 1
                ),
            ]
        };

        protected override ExitCode Execute()
        {
            if (Args.Length < 2)
            {
                Console.WriteLine("Usage: genjunk <nc> <file>");
                return ExitCode.Error;
            }

            string path = Args[1].AbsoluteOrRelativePath();
            Kernel.PrintDebug($"path: {path}");
            if (File.Exists(path))
            {
                Console.WriteLine($"genjunk: file {path} already exists");
                return ExitCode.Error;
            }
            //write random text to file
            File.WriteAllText(path, CreateString(int.Parse(Args[0])));

            return ExitCode.Success;
        }

        internal string CreateString(int stringLength)
        {
            Random rd = new Random();
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
