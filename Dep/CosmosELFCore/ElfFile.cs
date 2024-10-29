using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosELFCore
{
    public unsafe class ElfFile
    {
        public Elf32Ehdr ElfHeader { get; set; }
        public List<Elf32Shdr> SectionHeaders { get; set; } = new List<Elf32Shdr>();
        public List<Elf32Rel> RelocationInformation { get; set; } = new List<Elf32Rel>();
        public List<Elf32Sym> Symbols { get; set; } = new List<Elf32Sym>();
        private List<uint> _stringTables = new List<uint>();


        //TODO: fix this
        public string ResolveName(Elf32Shdr section, uint offset, MemoryStream stream)
        {
            Console.WriteLine($"resolving names at {offset}");
            var old = stream.Posistion;
            if (section.Type != SectionType.SymbolTable)
            {
                stream.Posistion = _stringTables[0] + offset;
            }
            else
            {
                stream.Posistion = _stringTables[1] + offset;
            }
            var reader = new BinaryReader(stream);
            var s = reader.ReadString();
            stream.Posistion = old;
            return s;
        }

        public ElfFile(MemoryStream stream)
        {
            Console.WriteLine("Reading Header...");
            //load main file header
            ElfHeader = new Elf32Ehdr((Elf32_Ehdr*) stream.Pointer);

            Console.WriteLine("Getting Section headers...");
            //load section headers
            var header = (Elf32_Shdr*) (stream.Pointer + ElfHeader.Shoff);

            Console.WriteLine("Reading Sections...");
            for (int i = 0; i < ElfHeader.Shnum; i++)
            {
                var x = new Elf32Shdr(&header[i]);
                if (x.Type == SectionType.StringTable) _stringTables.Add(x.Offset);
                SectionHeaders.Add(x);
            }

            Console.WriteLine("Resolving names...");
            //now we can load names into symbols and process sub data
            for (var index = 0; index < SectionHeaders.Count; index++)
            {
                var sectionHeader = SectionHeaders[index];
                //sectionHeader.Name = ResolveName(sectionHeader, sectionHeader.NameOffset, stream);

                switch (sectionHeader.Type)
                {
                    case SectionType.Relocation:
                        for (int i = 0; i < sectionHeader.Size / sectionHeader.Entsize; i++)
                        {
                            RelocationInformation.Add(new Elf32Rel(
                                    (Elf32_Rel*) (stream.Pointer + sectionHeader.Offset + i * sectionHeader.Entsize))
                                {Section = index});
                        }

                        break;
                    case SectionType.SymbolTable:
                        for (int i = 0; i < sectionHeader.Size / sectionHeader.Entsize; i++)
                        {
                            var x = new Elf32Sym(
                                (Elf32_Sym*) (stream.Pointer + sectionHeader.Offset + i * sectionHeader.Entsize));
                            //x.Name = ResolveName(sectionHeader, x.NameOffset, stream);
                            Symbols.Add(x);
                        }

                        break;
                }
            }
        }
    }
}