using Cosmos.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosELFCore
{

    //class to pass arguments to the exe being run
    public unsafe class ArgumentWriter
    {
        private BinaryWriter _writer;



        public ArgumentWriter()
        {
            //clear call stack
            for (int k = 0; k < 1024; k++)
            {
                ((byte*) Invoker.Stack)[k] = 0;
            }

            _writer = new BinaryWriter(new MemoryStream((byte*) Invoker.Stack));
            //warum? fucked up. weird. 
            _writer.BaseStream.Position = 50;
        }


        public ArgumentWriter Push(char c)
        {
            _writer.Write(c);
            return this;
        }

        public ArgumentWriter Push(byte c)
        {
             _writer.Write(c);
            return this;
        }

        public ArgumentWriter Push(short c)
        {
            _writer.Write(c);
            return this;
        }

        public ArgumentWriter Push(int c)
        {
            _writer.Write(c);
            return this;
        } 

        public ArgumentWriter Push(uint c)
        {
            //Kernel.PrintDebug("arg writer got arg: " + c);
            _writer.Write(c);
            return this;
        }

        public ArgumentWriter Push(string c)
        {
            _writer.Write(c);
            return this;
        }
        
    }
}