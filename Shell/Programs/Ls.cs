using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    internal class Ls : BuiltIn
    {
        public override string Name => "ls";

        public override string Description => "List current directory";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "directory",
                    description: "Path to target directory",
                    type: typeof(string),
                    argsPosition: 0
                ),
            ]
        };

        protected override ExitCode Execute()
        {
            string userSpecifiedPath = (string)GetArgument(0);

            string path = string.IsNullOrEmpty(userSpecifiedPath)
                ? Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory)
                : userSpecifiedPath;

            try
            {
                List<Cosmos.System.FileSystem.Listing.DirectoryEntry> list = Kernel.FileSystem.GetDirectoryListing(path);
                int longestName = 0;

                foreach (var item in list)
                {
                    if (item.mName.Length > longestName)
                    {
                        longestName = item.mName.Length;
                    }
                }
                longestName += 4;

                foreach (var item in list)
                {
                    Write(item.mName.Pad(longestName));
                    WriteLine($"{item.mSize} bytes");
                }
            }
            catch (Exception ex)
            {
                WriteLine($"Error: {ex.Message}");

                return ExitCode.Error;
            }

            return ExitCode.Success;
        }
    }
}
