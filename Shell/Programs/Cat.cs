using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    internal class Cat : BuiltIn
    {
        public override string Name => "cat";

        public override string Description => "output plaintext from files";

        public override ExitCode Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"Sokolsh: cat: No Argument provided");
                return ExitCode.Error;
            }

            string path = args[0].AbsoluteOrRelativePath();

            try
            {

                Console.WriteLine(File.ReadAllText(path));

            }
            catch (Exception)
            {
                Console.WriteLine($"Sokolsh: cat: File {path} does not exist");
                return ExitCode.Error;
            }
            
            return ExitCode.Success;
        }
    }
}
