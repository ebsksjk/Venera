using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.Core;
using static Cosmos.Core.INTs;

namespace Venera.VoPo.Interrupts
{
    internal static class InterruptHandler
    {
        public static void handleInterrupt80(ref INTs.IRQContext aContext)
        {
            Console.WriteLine("Interrupt 0x80 called");
            if (aContext.Interrupt == 0x80) // Example system call interrupt
            {
                uint syscallNumber = aContext.Interrupt;
                //Syscalls.HandleSyscall(syscallNumber, args.Skip(1).ToArray());
                Kernel.PrintDebug("handleInterrupt Syscall " + syscallNumber + " called");
            }
            else
            {
                // Handle other interrupts
            }
        }

        public static void Initialize()
        {
            INTs.SetIrqHandler(0x50, handleInterrupt80);
        }


        //stole this workaround from https://github.com/CosmosOS/Cosmos/blob/master/source/Cosmos.Core/INTs.cs
        public static void Dummy()
        {
            // Compiler magic
            bool xTest = false;
            if (xTest)
            {
                unsafe
                {
                    var xCtx = new IRQContext();
                    handleInterrupt80(ref xCtx);

                }
            }
        }
    }
}
