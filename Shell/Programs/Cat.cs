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

            string help;

            if (args[0].StartsWith(@"\"))
            {
                help = $"0:{args[0]}";
            }
            else
            {
                help = $"{Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory).EnsureBackslash()}{args[0]}";
            }
            Console.WriteLine(help);

            try
            {

                Console.WriteLine(File.ReadAllText(help));

            }
            catch (Exception)
            {
                Console.WriteLine($"Sokolsh: cat:{args} does not exist");
                return ExitCode.Error;
            }
            
            return ExitCode.Success;
        }
    }
}
