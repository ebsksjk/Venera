using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmosELFCore;
using IL2CPU.API;
using IL2CPU.API.Attribs;
using Venera.Shell;
using XSharp;
using XSharp.Assembler;

namespace Venera.VoPo.Interrupts
{
    internal class manint : BuiltIn
    {
        public override string Name => "manint";

        public override string Description => "manually issues an interrupt (for testing)";

        public override ExitCode Execute(string[] args){
            Console.WriteLine("Issuing interrupt 0x80");
            callInt();
            Console.WriteLine("Interrupt issued");
            return ExitCode.Success;
        }

        //we tell the compiler to use the asmWrap method to assemble the callInt method
        [PlugMethod(Assembler = typeof(asmWrap))]
        public static void callInt()
        {
        }

        //we define the asmWrap method (and class) to actually assemble the callInt method
        [Plug(Target = typeof(manint))]
        public class asmWrap : AssemblerMethod
        {
            public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
            {
                XS.LiteralCode($"int 0x50");
                XS.LiteralCode($"db 0xF00D");
            }
        }
    }
}
