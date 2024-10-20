
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;



namespace Venera.VoPo
{

    internal class ELF
    {
        Elf32_Ehdr header;
        public unsafe ELF(byte[] data)
        {
            fixed (byte* ptr = data)
            {
                header = *(Elf32_Ehdr*)ptr;
            }
        }

        public Elf32_Ehdr getHeader(){

                return header;
        }

    }

    //Elf 32 bit symbol table entry
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct Elf32_Sym
    {
        [FieldOffset(0)] public uint st_name;
        [FieldOffset(4)] public uint st_value;
        [FieldOffset(8)] public uint st_size;
        [FieldOffset(12)] public byte st_info;
        [FieldOffset(13)] public byte st_other;
        [FieldOffset(14)] public ushort st_shndx;
    }

    //relocation table entry
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct Elf32_Rel
    {
        [FieldOffset(0)] public uint r_offset;
        [FieldOffset(4)] public uint r_info;
    }

    //ELF Header struct
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct Elf32_Ehdr
    {
        [FieldOffset(0)] public fixed byte e_ident[16]; //stuff for identification -> index 4 should be 1, we're 32 bit
        [FieldOffset(16)] public ushort e_type;         //type of elf file
        [FieldOffset(18)] public ushort e_machine;      //type. probably 3 ig (Intel 386)
        [FieldOffset(20)] public uint e_version;        //version (should be 1)
        [FieldOffset(24)] public uint e_entry;          //entry point address. or zero. zero is bad
        [FieldOffset(28)] public uint e_phoff;          //program header table offset from beginning of file
        [FieldOffset(32)] public uint e_shoff;          //section header table offset from beginning of file
        [FieldOffset(36)] public uint e_flags;          //machine flags or smth
        [FieldOffset(40)] public ushort e_ehsize;       //size of this header
        [FieldOffset(42)] public ushort e_phentsize;    //program header table entry size
        [FieldOffset(44)] public ushort e_phnum;        //program header table entry num
        [FieldOffset(46)] public ushort e_shentsize;    //section header table entry size
        [FieldOffset(48)] public ushort e_shnum;        //section header table entry num
        [FieldOffset(50)] public ushort e_shstrndx;     //section header table index of the section name string table (what?)
    }

    public enum ELFType
    {
        ET_NONE = 0,            //no file type
        ET_REL = 1,             //relocatable file
        ET_EXEC = 2,            //executable file <-- we want this :3
        ET_DYN = 3,             //shared object file
        ET_CORE = 4,            //core file
        ET_LOPROC = 0xff00,     //processor spefic
        ET_HIPROC = 0xffff      // ''
    }


    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct Elf32_Shdr
    {
        [FieldOffset(0)] public uint sh_name;       //name (index of an c string in the string table)
        [FieldOffset(4)] public uint sh_type;       //Section Type (see SectionType)
        [FieldOffset(8)] public uint sh_flags;      //weird flags
        [FieldOffset(12)] public uint sh_addr;      //is this section interesting? then put it at the position specified here. if not, it is 0
        [FieldOffset(16)] public uint sh_offset;    //offset of the corresponding section
        [FieldOffset(20)] public uint sh_size;      //size of the corresponding section
        [FieldOffset(24)] public uint sh_link;      //index of a section header in the section header table. if this section is a symbol table, this is the index of the string table
        [FieldOffset(28)] public uint sh_info;      //additional info
        [FieldOffset(32)] public uint sh_addralign; //alignment
        [FieldOffset(36)] public uint sh_entsize;   //entrysize
    }

    public enum SectionType
    {
        None = 0,
        ProgramInformation = 1,
        SymbolTable = 2,
        StringTable = 3,
        RelocationAddend = 4,
        NotPresentInFile = 8,
        Relocation = 9,
    }

    [Flags]
    public enum SectionAttributes
    {
        Write = 0x01,
        Alloc = 0x02,
        Executable = 0x4
    }



    //symbol table entry
    public unsafe class Elf32Sym
    {
        public string Name;
        public uint NameOffset; //index of the name in the string table
        public uint Value;      //value of the symbol
        public uint Size;       //size of the symbol
        public byte Info;       //info (see also binding and type)
        public byte Other;      //0. just 0.
        public ushort Shndx;    //section header index
        public SymbolBinding Binding;
        public SymbolType Type;

        public Elf32Sym(Elf32_Sym* st)
        {
            NameOffset = st->st_name;
            Value = st->st_value;
            Size = st->st_size;
            Info = st->st_info;
            Other = st->st_other;
            Shndx = st->st_shndx;

            Binding = (SymbolBinding)(Info >> 0x4);
            Type = (SymbolType)(Info & 0x0F);
        }
    }

    public enum SymbolBinding
    {
        Local = 0, // Local scope
        Global = 1, // Global scope
        Weak = 2  // Weak, (ie. __attribute__((weak)))
    }

    public enum SymbolType
    {
        None = 0, // No type
        Object = 1, // Variables, arrays, etc.
        Function = 2,  // Methods or functions
        Common = 5
    }


    public unsafe class Elf32Shdr
    {
        public string Name;
        public uint NameOffset;
        public SectionType Type;
        public SectionAttributes Flag;
        public uint Addr;
        public uint Offset;
        public uint Size;
        public uint Link;
        public uint Info;
        public uint Addralign;
        public uint Entsize;

        public Elf32Shdr(Elf32_Shdr* st)
        {
            NameOffset = st->sh_name;
            Type = (SectionType)st->sh_type;
            Flag = (SectionAttributes)st->sh_flags;
            Addr = st->sh_addr;
            Offset = st->sh_offset;
            Size = st->sh_size;
            Link = st->sh_link;
            Info = st->sh_info;
            Addralign = st->sh_addralign;
            Entsize = st->sh_entsize;
        }

    }

}
