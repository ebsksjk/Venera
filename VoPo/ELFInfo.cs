using System;
using Venera.Shell;
using System.IO;
using CosmosELFCore;

namespace Venera.VoPo
{
    public class ELFInfo : BuiltIn
    {
        public override string Name => "elfinfo";

        public override string Description => "Display information about an elf file.";

        public override ExitCode Execute(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: elffile <args>");
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
                Console.WriteLine($"Opening file {path}!");
                byte[] binfile = File.ReadAllBytes(path);
                ElfParser p = new ElfParser(binfile);
                p.printHeader();
                //p.printSectionHeaders();
                //p.printSymbols();
                //p.printRelocationInformation();
                p.printStringTable();
            }

            catch (Exception)
            {
                Console.WriteLine($"Sokolsh: elfino: File {path} does not exist or is broken");
                return ExitCode.Error;
            }

            return ExitCode.Success;
        }
    }
}
