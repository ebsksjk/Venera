using System;
using System.Collections.Generic;
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
        public string ResolveName(Elf32Shdr section, uint offset, MemoryStream stream)
        {
            //Kernel.PrintDebug($"resolving names at {offset}");
            if (section == null) return "nu";

            if(_stringTables == null || _stringTables.Count == 0)
            {
                //Kernel.PrintDebug("No string tables found!");
                return "na";
            }

            // Save the current position of the stream
            var old = stream.Position;

            // Determine the position in the stream to read the name from
            if (section.Type != SectionType.SymbolTable)
            {
                //Kernel.PrintDebug("section type is not symbol table!!");
                stream.Position = (_stringTables[0] + offset)-1;
                //Kernel.PrintDebug("set stream position: " + stream.Position);
            }
            else
            {
                //Kernel.PrintDebug("section type is symbol table!!");
                stream.Position = (_stringTables[1] + offset) - 1;
            }

            // Read the name from the stream
            //Kernel.PrintDebug("creating new binary reader");
            var reader = new BinaryReader(stream);
            //Kernel.PrintDebug("created new binary reader! && trying to read string");
            var s = reader.ReadString();
            Kernel.PrintDebug($"read string: {s}");

            // Restore the original position of the stream
            stream.Position = old;

            return s;
        }

        // Constructor that initializes the ElfFile object from a MemoryStream
        public ElfFile(MemoryStream stream)
        {
            //Kernel.PrintDebug("Reading Header...");
            // Load the main ELF header
            ElfHeader = new Elf32Ehdr((Elf32_Ehdr*)stream.Pointer);

            //add the first string table
            stream.Position = ElfHeader.Shstrndx;
            _stringTables.Add(stream.Position);
            
            stream.Position = ElfHeader.Shoff;
            for(int i = 0; i < ElfHeader.Shnum; i++)
            {
                SectionHeaders.Add(new Elf32Shdr((Elf32_Shdr*)stream.Pointer));
                stream.Position += ElfHeader.Shentsize;
            }
        }
    }
}
