using System;
using Venera.Shell;
using System.IO;
using CosmosELFCore;

namespace Venera.VoPo
{
    public class ELFInfo : BuiltIn
    {
        public override string Name => "readelf";

        public override string Description => "Display information about an elf file.";

        public override ExitCode Execute(string[] args)
        {

            if (args.Length == 0)
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

            if (args[^1].StartsWith(@"\"))
            {
                //if it is an absolute path
                path = $"0:{args[^1]}";
            }
            else
            {
                //if it is a relative path
                //convert it into the corresponding absolute path
                path = $"{Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory).EnsureBackslash()}{args[^1]}";
            }

            for(var i = 0; i < args.Length-1; i++)
            {
                switch (args[i])
                {
                    case "-h":
                        displayHeader = true;
                        break;
                    case "-S":
                        displaySections = true;
                        break;
                    case "-s":
                        displaySymbols = true;
                        break;
                    case "-r":
                        displayRelocation = true;
                        break;
                    case "-t":
                        displayStringTable = true;
                        break;
                    default:
                        Console.WriteLine($"Sokolsh: elfino: Invalid argument {args[i]}");
                        return ExitCode.Error;
                }
            }
            try
            {   
                Console.WriteLine($"Opening file {path}!");
                byte[] binfile = File.ReadAllBytes(path);
                ElfParser p = new ElfParser(binfile);
                if(displayHeader)
                    p.printHeader();
                if(displaySections)
                    p.printSectionHeaders();
                if(displaySymbols)
                    p.printSymbols();
                if(displayRelocation)
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
