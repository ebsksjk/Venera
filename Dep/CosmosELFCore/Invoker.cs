using Cosmos.Core.Memory;
using IL2CPU.API;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Text;
using XSharp;
using XSharp.Assembler;

namespace CosmosELFCore
{
    public static unsafe class Invoker
    {
        public static uint Offset;
        //hilfe
        public static uint eax, ebx, ecx, edx, esi, edi, esp, ebp;
        public static uint* Stack = (uint*)Heap.Alloc(1024); //das ist static, also MÜSSEN wir irgendwie mallloc usw implementen

        //ja lol
        public static void Dump()
        {
            Console.WriteLine(
                $"eax:{eax}, ebx:{ebx}, ecx:{ecx}, edx:{edx}, esi:{esi}, edi:{edi}, esp: {esp}, ebp: {ebp}");
            for (int i = 0; i < 512; i++)
            {
                Console.Write(((byte*)Stack)[i] + " ");
            }
        }

        //hier sind die plug method header
        [PlugMethod(Assembler = typeof(PlugStoreState))]
        public static void StoreState()
        {
            eax = 0u;
            ebx = 0u;
            ecx = 0u;
            edx = 0u;
            esi = 0u;
            edi = 0u;
            esp = 0u;
            ebp = 0u;
        }

        [PlugMethod(Assembler = typeof(PlugLoadState))]
        public static void LoadState()
        {
        }

        [PlugMethod(Assembler = typeof(PlugCall))]
        public static void CallCode()
        {
        }
    }

    [Plug(Target = typeof(Invoker))]
    public class PlugStoreState : AssemblerMethod
    {
        //wir passen die pseudo register in die tatsächlichen register
        public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
        {
            XS.LiteralCode($"mov [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.eax))}], eax");
            XS.LiteralCode($"mov [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.ebx))}], ebx");
            XS.LiteralCode($"mov [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.ecx))}], ecx");
            XS.LiteralCode($"mov [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.edx))}], edx");
            XS.LiteralCode($"mov [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.edi))}], edi");
        }
    }


    [Plug(Target = typeof(Invoker))]
    //wir laden die tatsächlichen Register in die Pseudo-Register
    public class PlugLoadState : AssemblerMethod
    {
        public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
        {
            XS.LiteralCode($"mov eax, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.eax))}]");
            XS.LiteralCode($"mov ebx, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.ebx))}]");
            XS.LiteralCode($"mov ecx, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.ecx))}]");
            XS.LiteralCode($"mov edx, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.edx))}]");
            XS.LiteralCode($"mov edi, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.edi))}]");
        }
    }


    [Plug(Target = typeof(Invoker))]
    public class PlugCall : AssemblerMethod
    {
        public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
        {
            //laden der pseudo esp & ebp
            XS.LiteralCode($"mov [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.esp))}], esp");
            XS.LiteralCode($"mov [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.ebp))}], ebp");

            XS.LiteralCode($"mov eax, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.Stack))}]");
            XS.LiteralCode("add eax, 50"); //hier sind die komischen 50 wieder
            XS.LiteralCode("mov esp, eax");
            XS.LiteralCode("mov ebp, eax");
            XS.LiteralCode($"mov eax, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.Offset))}]");
            XS.LiteralCode("Call eax"); //wir callen den entry point der method?

            XS.LiteralCode($"mov ecx, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.Stack))}]");
            XS.LiteralCode("mov dword [ecx], eax");

            //speichern der register esp & ebp
            XS.LiteralCode($"mov esp, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.esp))}]");
            XS.LiteralCode($"mov ebp, [{LabelName.GetStaticFieldName(typeof(Invoker), nameof(Invoker.ebp))}]");
        }
    }
}
