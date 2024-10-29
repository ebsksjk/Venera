using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Console.WriteLine("Usage: elfinfo <path>");
                return ExitCode.Error;
            }

            string path;
            ElfFile elffile = null;

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
                string file_content = File.ReadAllText(path);
                byte[] binfile = Encoding.ASCII.GetBytes(file_content);

                unsafe
                {
                    fixed (byte* bin = binfile)
                    {
                        Console.WriteLine("Reading file ...");
                        CosmosELFCore.MemoryStream memoryStream = new CosmosELFCore.MemoryStream(bin);
                        Console.WriteLine("initialized memory stream!");

                        elffile = new ElfFile(memoryStream);
                        Console.WriteLine("File opened successfully!");

                        Elf32Ehdr hdr = elffile.ElfHeader;

                        Console.WriteLine($"Type: {hdr.Type}");
                        Console.WriteLine($"Machine: {hdr.Machine}");
                        Console.WriteLine($"Version: {hdr.Version}");
                        Console.WriteLine($"Entry: {hdr.Entry}");
                        Console.WriteLine($"Phoff: {hdr.Phoff}");
                        Console.WriteLine($"Shoff: {hdr.Shoff}");
                        Console.WriteLine($"Flags: {hdr.Flags}");
                        Console.WriteLine($"Ehsize: {hdr.Ehsize}");
                        Console.WriteLine($"Phentsize: {hdr.Phentsize}");
                        Console.WriteLine($"Phnum: {hdr.Phnum}");
                        Console.WriteLine($"Shentsize: {hdr.Shentsize}");
                        Console.WriteLine($"Shnum: {hdr.Shnum}");
                        Console.WriteLine($"Shstrndx: {hdr.Shstrndx}");
                    }
                }
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
