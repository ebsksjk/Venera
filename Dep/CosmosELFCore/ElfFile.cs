using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using Cosmos.System;

namespace CosmosELFCore
{
    // Represents an ELF (Executable and Linkable Format) file
    public unsafe class ElfFile
    {
        // ELF header containing metadata about the ELF file
        public Elf32Ehdr ElfHeader { get; set; }

        // List of section headers in the ELF file
        public List<Elf32Shdr> SectionHeaders { get; set; } = new List<Elf32Shdr>();

        // List of relocation entries in the ELF file
        public List<Elf32Rel> RelocationInformation { get; set; } = new List<Elf32Rel>();

        // List of symbols in the ELF file
        public List<Elf32Sym> Symbols { get; set; } = new List<Elf32Sym>();

        // List of offsets to string tables in the ELF file
        public List<uint> _stringTables = new List<uint>();

        // Resolves a name from a section and offset within the ELF file
        public string ResolveSectionName(Elf32Shdr section, uint offset, MemoryStream stream)
        {
            var old = stream.Position;
            stream.Position = (_stringTables[0] + (offset));

            // Read the name from the stream
            var reader = new BinaryReader(stream);
            var s = reader.ReadString();
            stream.Position = old;

            return s;
        }

        public string ResolveSymbolName(Elf32Sym symbol, MemoryStream stream)
        {
            var old = stream.Position;
            stream.Position = (_stringTables[1] + (symbol.NameOffset));

            // Read the name from the stream
            var reader = new BinaryReader(stream);
            var s = reader.ReadString();
            stream.Position = old;

            return s;
        }

        // Constructor that initializes the ElfFile object from a MemoryStream
        public ElfFile(MemoryStream stream)
        {
            //Kernel.PrintDebug("Reading Header...");
            // Load the main ELF header
            ElfHeader = new Elf32Ehdr((Elf32_Ehdr*)stream.Pointer);

            var stringtable = (Elf32_Shdr*)(stream.Pointer + ElfHeader.Shoff + (ElfHeader.Shstrndx * ElfHeader.Shentsize));
            _stringTables.Add(new Elf32Shdr(stringtable).Offset);
            var header = (Elf32_Shdr*)(stream.Pointer + ElfHeader.Shoff);


            for (int i = 0; i < ElfHeader.Shnum; i++)
            {
                var x = new Elf32Shdr(&header[i]);
                if (x.Type == SectionType.StringTable && i != ElfHeader.Shstrndx) _stringTables.Add(x.Offset);
                SectionHeaders.Add(x);
            }

            int index = 0;
            foreach(var i in SectionHeaders)
            {
                var name = ResolveSectionName(i, i.NameOffset, stream);
                i.Name = name;
                //Kernel.PrintDebug($"section name: {name} @ offset {i.NameOffset}");
                if (i.Type == SectionType.SymbolTable)
                {
                    for (int j = 0; j < i.Size / i.Entsize; j++)
                    {
                        var curSym = new Elf32Sym((Elf32_Sym*)(stream.Pointer + i.Offset + j * i.Entsize));
                        var syName = ResolveSymbolName(curSym, stream);
                        //Kernel.PrintDebug($"symbol name: {syName} @ offset {curSym.NameOffset}");
                        curSym.Name = syName;
                        Symbols.Add(curSym);
                    }
                } else if(i.Type == SectionType.Relocation) {
                    for (int j = 0; j < i.Size / i.Entsize; j++)
                    {
                        RelocationInformation.Add(new Elf32Rel((Elf32_Rel*)(stream.Pointer + i.Offset + j * i.Entsize)){ Section = index });
                    }
                }
                index++;
            }
        }
    }
}
