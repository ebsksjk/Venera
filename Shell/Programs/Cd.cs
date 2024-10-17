using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    internal class Cd : BuiltIn
    {
        public override string Name => "cd";

        public override string Description => "change current directory";

        public override ExitCode Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"Sokolsh: cd: No Argument provided");
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
                Cosmos.System.FileSystem.Listing.DirectoryEntry dir = Kernel.FileSystem.GetDirectory(help) 
                    ?? throw new Exception();

                if (dir.mEntryType != Cosmos.System.FileSystem.Listing.DirectoryEntryTypeEnum.Directory)
                {
                    Console.WriteLine($"Sokolsh: cd: {args} is not a directory");
                    return ExitCode.Error;
                }

                Kernel.GlobalEnvironment.Set(DefaultEnvironments.CurrentWorkingDirectory, $"{dir.mFullPath.ToString().EnsureBackslash()}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Sokolsh: cd: {args} File or Directory does not exist");
                return ExitCode.Error;
            }
            return ExitCode.Success;
        }
    }
}
