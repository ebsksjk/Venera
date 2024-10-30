using System;
using System.Collections.Generic;
using System.Text;


//wheee was soll hier hin? elf infos? können wir doch bestimmt implementieren :)
namespace CosmosELFCore
{
    class ElfParser
    {
        private ElfFile elffile;
        private byte[] binfile;

        public ElfParser(byte[] _binfile)
        {
            binfile = _binfile;
            unsafe
            {
                fixed (byte* bin = binfile)
                {
                    Console.WriteLine("Reading file ...");
                    CosmosELFCore.MemoryStream memoryStream = new CosmosELFCore.MemoryStream(bin);
                    Console.WriteLine("initialized memory stream!");

                    elffile = new ElfFile(memoryStream);
                    Console.WriteLine("File opened successfully!");
                }
            }
        }

        public void printHeader()
        {
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

        public void printSectionHeaders()
        {
            foreach (var section in elffile.SectionHeaders)
            {
                if (section == null) continue;
                /*foreach (PropertyInfo property in section.GetType().GetProperties())
                {
                    if (property.GetValue(section) == null)
                    {
                        isNull = true;
                    }
                }*/

                if (section.Name == null)
                {
                    continue;
                }

                Console.WriteLine($"Section: {section.Name}");
            }
        }

        public void printRelocationInformation()
        {
            foreach (var relocation in elffile.RelocationInformation)
            {
                Console.WriteLine($"Offset: {relocation.Offset}");
                Console.WriteLine($"Info: {relocation.Info}");
                Console.WriteLine($"Section: {relocation.Section}");
                Console.WriteLine($"Symbol: {relocation.Symbol}");
                Console.WriteLine($"Type: {relocation.Type}");
            }
        }

        public void printSymbols()
        {
            foreach (Elf32Sym symbol in elffile.Symbols)
            {
                if (symbol == null) continue;
                if (symbol.Name == null) continue;

                Console.WriteLine($"Symbol: {symbol.Name}");
            }
        }

    }
}
