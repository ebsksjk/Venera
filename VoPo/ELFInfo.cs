using CosmosELFCore;
using System;
using System.IO;
using Venera.Shell;

namespace Venera.VoPo
{
    public class ELFInfo : BuiltIn
    {
        public override string Name => "readelf";

        public override string Description => "Display information about an elf file.";
        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "enable_sections",
                    description: "Print sections",
                    shortForm: 'S',
                    type: typeof(bool)
                ),
                new(
                    valueName: "enable_header",
                    description: "Print header",
                    shortForm: 'h',
                    type: typeof(bool)
                ),
                new(
                    valueName: "enable_relocations",
                    description: "Print relocations",
                    shortForm: 'r',
                    type: typeof(bool)
                ),
                new(
                    valueName: "enable_str_table",
                    description: "Print string table",
                    shortForm: 't',
                    type: typeof(bool)
                ),
                new(
                    valueName: "enable_symbols",
                    description: "Print symbols",
                    shortForm: 's',
                    type: typeof(bool)
                ),
                new(
                    valueName: "file",
                    description: "Target binary file",
                    argsPosition: 0,
                    required: true,
                    type: typeof(string)
                )
            ]
        };

        protected override ExitCode Execute()
        {

            if (Args.Length == 0)
            {
                Console.WriteLine("Usage: elffile [-Shrts] <args>");
                return ExitCode.Error;
            }

            string path;
            bool displayHeader = false;
            bool displaySections = false;
            bool displaySymbols = false;
            bool displayRelocation = false;
            bool displayStringTable = false;

            string elfPath = (string)GetArgument(0);
            if (elfPath.StartsWith(@"\"))
            {
                //if it is an absolute path
                path = $"0:{elfPath}";
            }
            else
            {
                //if it is a relative path
                //convert it into the corresponding absolute path
                path = $"{Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory).EnsureBackslash()}{elfPath}";
            }

            displayHeader = (bool)GetArgument("h");
            displaySections = (bool)GetArgument("S");
            displaySymbols = (bool)GetArgument("s");
            displayRelocation = (bool)GetArgument("r");
            displayStringTable = (bool)GetArgument("t");

            try
            {
                Console.WriteLine($"Opening file {path}!");
                byte[] binfile = File.ReadAllBytes(path);
                ElfParser p = new ElfParser(binfile);
                if (displayHeader)
                    p.printHeader();
                if (displaySections)
                    p.printSectionHeaders();
                if (displaySymbols)
                    p.printSymbols();
                if (displayRelocation)
                    p.printRelocationInformation();
                if (displayStringTable)
                    p.printStringTable();
            }

            catch (Exception)
            {
                Console.WriteLine($"Sokolsh: readelf: File {path} does not exist or is broken");
                return ExitCode.Error;
            }

            return ExitCode.Success;
        }
    }
}
