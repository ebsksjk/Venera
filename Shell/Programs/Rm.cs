using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    internal class Rm: BuiltIn
    {
        public override string Name => "rm";

        public override string Description => "delete file";

        public override ExitCode Execute(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Usage: rm <file>");
                return ExitCode.Error;
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"Sokolsh: rm: File {args[0]} does not exist");
                return ExitCode.Error;
            }
            File.Delete(args[0]);
            return ExitCode.Success;
        }
    }
}
