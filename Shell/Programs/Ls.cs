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

        public override ExitCode Execute(string[] args)
        {
            string path = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);

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
                    Console.Write(item.mName.Pad(longestName));
                    Console.WriteLine($"{item.mSize} bytes");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                return ExitCode.Error;
            }

            return ExitCode.Success;
        }
    }
}
