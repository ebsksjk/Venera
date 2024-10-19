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

            string path;

            if (args[0].StartsWith(@"\"))
            {
                //if it is an absolute path
                path = $"0:{args[0]}";
            }
            else
            {
                //if it is a relative path
                //convert it into the corresponding absolute path
                path = $"{Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory).EnsureBackslash()}{args[0]}";
            }

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
