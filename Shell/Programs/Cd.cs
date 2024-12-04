using System;

namespace Venera.Shell.Programs
{
    internal class Cd : BuiltIn
    {
        public override string Name => "cd";

        public override string Description => "Change current directory";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "file_path",
                    description: "Path to target file",
                    argsPosition: 0,
                    type: typeof(string)
                )
            ]
        };

        protected override ExitCode Execute()
        {
            if (Args.Length == 0)
            {
                return ExitCode.Usage;
            }

            string cmdPath = (string)GetArgument(0);

            if (StringExtensions.IsDriveId(cmdPath))
            {
                if (Kernel.FileSystem.IsValidDriveId(cmdPath))
                {
                    Console.WriteLine($"Sokolsh: cd: {cmdPath} is a valid drive Id!!!");
                }

                Kernel.GlobalEnvironment.Set(DefaultEnvironments.CurrentWorkingDirectory, $"{cmdPath}");

                return ExitCode.Success;
            }

            string path = cmdPath.AbsoluteOrRelativePath();

            try
            {
                Cosmos.System.FileSystem.Listing.DirectoryEntry dir = Kernel.FileSystem.GetDirectory(path)
                    ?? throw new Exception();

                if (dir.mEntryType != Cosmos.System.FileSystem.Listing.DirectoryEntryTypeEnum.Directory)
                {
                    Console.WriteLine($"Sokolsh: cd: {Args} is not a directory");
                    return ExitCode.Error;
                }

                if (dir.mFullPath.isAccessible() == false)
                {
                    Console.WriteLine($"Sokolsh: cd: {Args} is not accessible");
                    return ExitCode.Error;
                }
                Kernel.GlobalEnvironment.Set(DefaultEnvironments.CurrentWorkingDirectory, $"{dir.mFullPath.ToString().EnsureBackslash()}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Sokolsh: cd: {path} File or Directory does not exist");
                return ExitCode.Error;
            }
            return ExitCode.Success;
        }
    }
}
