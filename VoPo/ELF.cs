using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using CosmosELFCore;

namespace Venera.VoPo
{
    // ELF header structure (64-bit ELF)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ELFHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] e_ident;     // Magic number and other info
        public ushort e_type;      // Object file type
        public ushort e_machine;   // Architecture
        public uint e_version;     // Object file version
        public ulong e_entry;      // Entry point virtual address
        public ulong e_phoff;      // Program header table file offset
        public ulong e_shoff;      // Section header table file offset
        public uint e_flags;       // Processor-specific flags
        public ushort e_ehsize;    // ELF header size in bytes
        public ushort e_phentsize; // Program header table entry size
        public ushort e_phnum;     // Program header table entry count
        public ushort e_shentsize; // Section header table entry size
        public ushort e_shnum;     // Section header table entry count
        public ushort e_shstrndx;  // Section header string table index
    }

    // Section header structure (64-bit ELF)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SectionHeader
    {
        public uint sh_name;       // Section name (index into string table)
        public uint sh_type;       // Section type
        public ulong sh_flags;     // Section flags
        public ulong sh_addr;      // Virtual address in memory
        public ulong sh_offset;    // Offset of the section in the file
        public ulong sh_size;      // Size of the section in bytes
        public uint sh_link;       // Section index link
        public uint sh_info;       // Extra information
        public ulong sh_addralign; // Alignment of the section
        public ulong sh_entsize;   // Size of entries if the section holds a table
    }

    // Program header structure (64-bit ELF)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ProgramHeader
    {
        public uint p_type;        // Type of segment
        public uint p_flags;       // Segment attributes
        public ulong p_offset;     // Offset in file
        public ulong p_vaddr;      // Virtual address in memory
        public ulong p_paddr;      // Physical address (for systems using physical memory)
        public ulong p_filesz;     // Size of segment in file
        public ulong p_memsz;      // Size of segment in memory
        public ulong p_align;      // Alignment of segment
    }

    class ELFFile
    {
        public ELFHeader header { get; set; }
        public List<SectionHeader> sheader { get; set; } = new List<SectionHeader>();
        public List<ProgramHeader> pheader { get; set; } = new List<ProgramHeader>();
        private List<uint> _stringTables = new List<uint>();

        public ELFFile(byte[] elffile)
        {
            if (elffile.Length == 0)
            {
                return;
            }

            try
            {
                unsafe
                {
                    fixed (byte* ptr = elffile)
                    {
                        CosmosELFCore.MemoryStream ms = new CosmosELFCore.MemoryStream(ptr);
                        CosmosELFCore.BinaryReader reader = new CosmosELFCore.BinaryReader(ms);
                            // Read and parse ELF header
                            ELFHeader elfHeader = ReadStruct<ELFHeader>(reader);

                            // Check if it is a valid ELF file
                            if (elfHeader.e_ident[0] != 0x7F || elfHeader.e_ident[1] != (byte)'E' || elfHeader.e_ident[2] != (byte)'L' || elfHeader.e_ident[3] != (byte)'F')
                            {
                                Kernel.PrintDebug("Not a valid ELF file.");
                                return;
                            }

                            Kernel.PrintDebug("ELF Header:");
                            Kernel.PrintDebug($"  Type: {elfHeader.e_type}");
                            Kernel.PrintDebug($"  Machine: {elfHeader.e_machine}");
                            Kernel.PrintDebug($"  Entry Point: 0x{elfHeader.e_entry:X}");

                            // Read and parse section headers
                            ms.Seek((long)elfHeader.e_shoff, SeekOrigin.Begin);
                            for (int i = 0; i < elfHeader.e_shnum; i++)
                            {
                                SectionHeader sectionHeader = ReadStruct<SectionHeader>(reader);
                                Kernel.PrintDebug($"Section {i}:");
                                Kernel.PrintDebug($"  Offset: 0x{sectionHeader.sh_offset:X}");
                                Kernel.PrintDebug($"  Size: 0x{sectionHeader.sh_size:X}");
                                Kernel.PrintDebug($"  Type: {sectionHeader.sh_type}");
                            }

                            // Read and parse program headers
                            ms.Seek((long)elfHeader.e_phoff, SeekOrigin.Begin);
                            for (int i = 0; i < elfHeader.e_phnum; i++)
                            {
                                ProgramHeader programHeader = ReadStruct<ProgramHeader>(reader);
                                Kernel.PrintDebug($"Program Header {i}:");
                                Kernel.PrintDebug($"  Offset: 0x{programHeader.p_offset:X}");
                                Kernel.PrintDebug($"  Size in File: 0x{programHeader.p_filesz:X}");
                                Kernel.PrintDebug($"  Size in Memory: 0x{programHeader.p_memsz:X}");
                                Kernel.PrintDebug($"  Type: {programHeader.p_type}");
                            } 
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.PrintDebug($"Error: {ex.Message}");
            }
        }

        // Helper method to read a structure from the binary reader
        private static T ReadStruct<T>(CosmosELFCore.BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf<T>());
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStruct = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            handle.Free();
            return theStruct;
        }
    }
}
