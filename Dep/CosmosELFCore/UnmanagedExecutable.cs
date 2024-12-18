﻿using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.Core.Memory;

namespace CosmosELFCore
{
    public unsafe class UnmanagedExecutable
    {
        private MemoryStream _stream;       //this is the original file
        private ElfFile _elf;               //this is the parsed file
        private byte* _finalExecutible;     //this is the final loaded executable

        public UnmanagedExecutable(byte* elfbin)
        {
            _stream = new MemoryStream(elfbin);
        }

        public void Load()
        {
            _elf = new ElfFile(_stream);


            /*
             * 1. determine the total size of the final loaded sections
             * 2. malloc some space for them and allocate them
             * 3. update headers location information
             */

            //calculate bss section size
            for (var i = 0; i < _elf.SectionHeaders.Count; i++)
            {
                var header = _elf.SectionHeaders[i];
                if (header.Type == SectionType.NotPresentInFile)
                {
                    uint bssbase = 0;
                    for (var index = 0; index < _elf.Symbols.Count; index++)
                    {
                        var sym = _elf.Symbols[index];
                        if (sym.Shndx == 0xFFF2)
                        {
                            var size = sym.Value;
                            sym.Value = bssbase;
                            bssbase += size;
                            sym.Shndx = (ushort) i;
                        }
                    }

                    header.Size = bssbase;
                    break;
                }
            }

            //calculate final size
            uint totalSize = 0;
            foreach (var header in _elf.SectionHeaders)
            {
                if ((header.Flag & SectionAttributes.Alloc) == SectionAttributes.Alloc)
                {
                    totalSize += header.Size;
                }
            }

            //allocate final size
            _finalExecutible = Heap.Alloc(totalSize);

            var stream = new MemoryStream(_finalExecutible);
            var writer = new BinaryWriter(stream);

            foreach (var header in _elf.SectionHeaders)
            {
                if ((header.Flag & SectionAttributes.Alloc) != SectionAttributes.Alloc) continue;

                if (header.Type == SectionType.NotPresentInFile)
                {
                    //update the meta data
                    header.Offset = writer.BaseStream.Position;

                    for (int i = 0; i < header.Size; i++)
                    {
                        writer.Write((byte) 0);
                    }
                }
                else
                {
                    //read the data from the original file
                    var reader = new BinaryReader(_stream);
                    reader.BaseStream.Position = header.Offset;

                    //update the meta data
                    header.Offset = writer.BaseStream.Position;

                    //write the data from the old file into the loaded executable
                    for (int i = 0; i < header.Size; i++)
                    {
                        writer.Write(reader.ReadByte());
                    }
                }
            }
        }


        public void Link()
        {
            foreach (var rel in _elf.RelocationInformation)
            {
                var symval = _elf.Symbols[(int) rel.Symbol].Value;

                var addr = (uint) _finalExecutible +
                           _elf.SectionHeaders[(int) _elf.SectionHeaders[rel.Section].Info].Offset;
                var refr = (uint*) (addr + rel.Offset);

                var memOffset = (uint) _finalExecutible +
                                _elf.SectionHeaders[_elf.Symbols[(int) rel.Symbol].Shndx].Offset;

                switch (rel.Type)
                {
                    case RelocationType.R38632:
                        *refr = (symval + *refr) + memOffset; // Symbol + Offset
                        break;
                    case RelocationType.R386Pc32:
                        *refr = (symval + *refr - (uint) refr) + memOffset; // Symbol + Offset - Section Offset
                        break;
                    case RelocationType.R386None:
                        //nop
                        break;
                    default:
                        Console.WriteLine($"Error RelocationType({(int)rel.Type}) not implemented");
                        break;
                }
            }
        }

        public uint Invoke(string function)
        {
            for (int i = 0; i < _elf.Symbols.Count; i++)
            {
                if (function == _elf.Symbols[i].Name)
                {
                    Invoker.Offset =
                        (uint) _finalExecutible + _elf.Symbols[i].Value +
                        _elf.SectionHeaders[_elf.Symbols[i].Shndx].Offset;

                    Invoker.StoreState();
                    Invoker.CallCode();
                    Invoker.LoadState();

                    return Invoker.Stack[0];
                } 
            }

            Console.WriteLine($"The function/entrypoint {function} could not be found in the current executable." +
                        $" It may be spelled incorrectly, does not exist or the file is corrupted");
            return 999;


        }
    }
}