using System;
using System.Reflection;
using Cosmos.Core;
using IL2CPU.API.Attribs;
using XSharp.Assembler;
using static Cosmos.Core.INTs;

namespace Venera.VoPo.Interrupts
{
    public static class InterruptHandler
    {
        public static byte venInt = 0x80;
        public static void HandleInterrupt_80(ref INTs.IRQContext aContext)
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
            INTs.SetIrqHandler(0x80, HandleInterrupt_80);
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
                    HandleInterrupt_80(ref xCtx);

                }
            }
        }

        public static void getVenIntHandler()
        {
            a_getVenIntHandler();
        }

        [PlugMethod(Assembler = typeof(getIntWrap))]
        public static void a_getVenIntHandler()
        {
        }


        [Plug(Target = typeof(InterruptHandler))]
        public class getIntWrap : AssemblerMethod
        {
            private static MethodBase GetMethodDef(Assembly aAssembly, string aType, string aMethodName, bool aErrorWhenNotFound)
            {
                Type xType = aAssembly.GetType(aType, false);
                if (xType != null)
                {
                    MethodBase xMethod = xType.GetMethod(aMethodName);
                    if (xMethod != null)
                    {
                        return xMethod;
                    }
                }
                if (aErrorWhenNotFound)
                {
                    throw new Exception("Method '" + aType + "::" + aMethodName + "' not found!");
                }
                return null;
            }

            public static MethodBase GetInterruptHandler(byte aInterrupt)
            {
                Console.WriteLine("Getting interrupt handler for HandleInterrupt_" + aInterrupt.ToString("X2"));
                return GetMethodDef(typeof(InterruptHandler).Assembly, typeof(InterruptHandler).FullName
                    , "HandleInterrupt_" + aInterrupt.ToString("X2"), true);
            }

            public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
            {
                SetIntHandler(0x80, HandleInterrupt_80);
                MethodBase xMethod = GetInterruptHandler(venInt);
                if (xMethod == null)
                {
                    throw new Exception("ich bin eine dumme und hässliche hurenexception");
                }
                Console.WriteLine("Method found for int 0x80: " + xMethod.Name);
            }
        }
    }
}
