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

            //Kernel.PrintDebug("Getting Section headers...");
            // Load the section headers
            var header = (Elf32_Shdr*)(stream.Pointer + ElfHeader.Shoff);

            Kernel.PrintDebug("Reading Sections...");
            Kernel.PrintDebug($"ElfHeader.Shnum: {ElfHeader.Shnum}");
            Kernel.PrintDebug($"ElfHeader.Shoff: {ElfHeader.Shoff}");
            Kernel.PrintDebug($"ElfHeader.sh size: {ElfHeader.Shentsize}");
            // Iterate through each section header
            for (int i = 0; i < ElfHeader.Shnum; i++)
            {
                var x = new Elf32Shdr(&header[i]);
                if (x.Type == SectionType.StringTable)
                {
                    //Kernel.PrintDebug("String table found!");
                    _stringTables.Add(x.Offset);
                }
                SectionHeaders.Add(x);
            }

            //Kernel.PrintDebug("Resolving names...");
            // Resolve names and process sub-data for each section
            for (var index = 0; index < SectionHeaders.Count; index++)
            {
                var sectionHeader = SectionHeaders[index];
                //Kernel.PrintDebug($"Resolving names at {sectionHeader.NameOffset}");
                sectionHeader.Name = ResolveName(sectionHeader, sectionHeader.NameOffset, stream);

                // Process relocation and symbol table sections
                switch (sectionHeader.Type)
                {
                    case SectionType.Relocation:
                        for (int i = 0; i < sectionHeader.Size / sectionHeader.Entsize; i++)
                        {
                            RelocationInformation.Add(new Elf32Rel(
                                    (Elf32_Rel*)(stream.Pointer + sectionHeader.Offset + i * sectionHeader.Entsize))
                            { Section = index });
                        }
                        break;

                    case SectionType.SymbolTable:
                        for (int i = 0; i < sectionHeader.Size / sectionHeader.Entsize; i++)
                        {
                            var x = new Elf32Sym(
                                (Elf32_Sym*)(stream.Pointer + sectionHeader.Offset + i * sectionHeader.Entsize));
                            //Kernel.PrintDebug($"Resolving names at {x.NameOffset}");
                            x.Name = ResolveName(sectionHeader, x.NameOffset, stream);
                            Symbols.Add(x);
                        }
                        break;
                }
            }
        }
    }
}
