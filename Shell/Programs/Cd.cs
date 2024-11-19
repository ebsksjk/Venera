using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cosmos.System.FileSystem;

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

            if (StringExtensions.IsDriveId(args[0]))
            {
                if (Kernel.FileSystem.IsValidDriveId(args[0]))
                {
                    Console.WriteLine($"Sokolsh: cd: {args[0]} is a valid drive Id!!!");
                }

                Kernel.GlobalEnvironment.Set(DefaultEnvironments.CurrentWorkingDirectory, $"{args[0]}");

                return ExitCode.Success;
            }

            string path = args[0].AbsoluteOrRelativePath();

            try
            {
                Cosmos.System.FileSystem.Listing.DirectoryEntry dir = Kernel.FileSystem.GetDirectory(path) 
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
                Console.WriteLine($"Sokolsh: cd: {args[0]} File or Directory does not exist");
                return ExitCode.Error;
            }
            return ExitCode.Success;
        }
    }
}
