using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;
using System.IO;

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
            ELFFile elffile = null;

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
                string file_content = File.ReadAllText(path);
                byte[] binfile = Encoding.ASCII.GetBytes(file_content);

                elffile = new ELFFile(binfile);/*
                Elf32_Ehdr ehdr = elffile.header;

                unsafe
                {
                    Console.WriteLine($"ELF Header:");
                    Kernel.PrintDebug($"ELF Header:");
                    Console.WriteLine($"  Magic:                             {ehdr.e_ident[0]:X2} {ehdr.e_ident[1]:X2} {ehdr.e_ident[2]:X2} {ehdr.e_ident[3]:X2}");
                    Kernel.PrintDebug($"  Magic:                             {ehdr.e_ident[0]:X2} {ehdr.e_ident[1]:X2} {ehdr.e_ident[2]:X2} {ehdr.e_ident[3]:X2}");
                    Console.WriteLine($"  Type:                              {ehdr.e_type}");
                    Kernel.PrintDebug($"  Type:                              {ehdr.e_type}");
                    Console.WriteLine($"  Machine:                           {ehdr.e_machine}");
                    Kernel.PrintDebug($"  Machine:                           {ehdr.e_machine}");
                    Console.WriteLine($"  Version:                           {ehdr.e_version}");
                    Kernel.PrintDebug($"  Version:                           {ehdr.e_version}");
                    Console.WriteLine($"  Entry point address:               {ehdr.e_entry}");
                    Kernel.PrintDebug($"  Entry point address:               {ehdr.e_entry}");
                    Console.WriteLine($"  Start of program headers:          {ehdr.e_phoff}");
                    Kernel.PrintDebug($"  Start of program headers:          {ehdr.e_phoff}");
                    Console.WriteLine($"  Start of section headers:          {ehdr.e_shoff}");
                    Kernel.PrintDebug($"  Start of section headers:          {ehdr.e_shoff}");
                    Console.WriteLine($"  Flags:                             {ehdr.e_flags}");
                    Kernel.PrintDebug($"  Flags:                             {ehdr.e_flags}");
                    Console.WriteLine($"  Size of this header:               {ehdr.e_ehsize}");
                    Kernel.PrintDebug($"  Size of this header:               {ehdr.e_ehsize}");
                    Console.WriteLine($"  Size of program header entry:      {ehdr.e_phentsize}");
                    Kernel.PrintDebug($"  Size of program header entry:      {ehdr.e_phentsize}");
                    Console.WriteLine($"  Number of program headers:         {ehdr.e_phnum}");
                    Kernel.PrintDebug($"  Number of program headers:         {ehdr.e_phnum}");
                    Console.WriteLine($"  Size of section header entry:      {ehdr.e_shentsize}");
                    Kernel.PrintDebug($"  Size of section header entry:      {ehdr.e_shentsize}");
                    Console.WriteLine($"  Number of section headers:         {ehdr.e_shnum}");
                    Kernel.PrintDebug($"  Number of section headers:         {ehdr.e_shnum}");
                    Console.WriteLine($"  Section header string table index: {ehdr.e_shstrndx}");
                    Kernel.PrintDebug($"  Section header string table index: {ehdr.e_shstrndx}");
                }

                List<Elf32Shdr> shdr = elffile.SectionHeaders;

                Console.WriteLine("\nSection Headers:");
                Kernel.PrintDebug("\nSection Headers:");
                foreach (Elf32Shdr i in shdr)
                {
                    Console.WriteLine($"Name:       {i.Name}");
                    Kernel.PrintDebug($"Name:       {i.Name}");
                    Console.WriteLine($"NameOffset: {i.NameOffset}");
                    Kernel.PrintDebug($"NameOffset: {i.NameOffset}");
                    Console.WriteLine($"Addr:       {i.Addr}");
                    Kernel.PrintDebug($"Addr:       {i.Addr}");
                    Console.WriteLine($"Offset:     {i.Offset}");
                    Kernel.PrintDebug($"Offset:     {i.Offset}");
                    Console.WriteLine($"Size:       {i.Size}");
                    Kernel.PrintDebug($"Size:       {i.Size}");
                    Console.WriteLine($"Link:       {i.Link}");
                    Kernel.PrintDebug($"Link:       {i.Link}");
                    Console.WriteLine($"Info:       {i.Info}");
                    Kernel.PrintDebug($"Info:       {i.Info}");
                    Console.WriteLine($"Addalign:   {i.Addralign}");
                    Kernel.PrintDebug($"Addalign:   {i.Addralign}");
                    Console.WriteLine($"Entsize:    {i.Entsize}");
                    Kernel.PrintDebug($"Entsize:    {i.Entsize}");
                    Console.WriteLine($"");
                    Kernel.PrintDebug($"");
                }

                List<uint> strtbl = elffile._stringTables;

                Console.WriteLine("\nString Table:");

                foreach (uint i in strtbl)
                {
                    Console.WriteLine($"String Table: {i}");
                    Kernel.PrintDebug($"String Table: {i}");
                }*/

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
