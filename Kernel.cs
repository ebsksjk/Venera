using Cosmos.HAL;
using Cosmos.System.FileSystem.VFS;
using CosmosELF;
using System;
using System.IO;
using System.Text;
using System.Threading;
using Venera.Shell;
using XSharp.x86.Params;
using Sys = Cosmos.System;

namespace Venera
{
    public /*unsafe*/ class Kernel : Sys.Kernel
    {
        public static string OS_NAME = "Venera";
        public static string OS_VERSION = "0.1";

        private static Environment<string> _environment;

        public static Environment<string> GlobalEnvironment { get => _environment; }

        public static Sys.FileSystem.CosmosVFS FileSystem;

        protected override void BeforeRun()
        {

            //ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\comp.so"), null, "tty_clear", "1:\\comp.so");
            //ApplicationRunner.runApplicationEntryPoint("test", TestFile.test_so, ["a"], "tty_puts");

            FileSystem = new Cosmos.System.FileSystem.CosmosVFS();
            VFSManager.RegisterVFS(FileSystem);

            Console.WriteLine("Welcome on Venera");
            // in BeforeRun() or when user calls a "command"

            _environment = new();
            _environment.Set(DefaultEnvironments.CurrentWorkingDirectory, @"0:\");
            if (!Directory.Exists("0:\\Sys"))
            {
                Directory.CreateDirectory("0:\\Sys");
            }
            if (!Directory.Exists("0:\\Sys\\proc"))
            {
                Directory.CreateDirectory("0:\\Sys\\proc");
            } else if (Directory.GetFiles("0:\\Sys\\proc").Length != 0 )
            {
                string[] pList = Directory.GetFiles("0:\\Sys\\proc");
                if (!(pList.Length == 0 || pList == null))
                {
                    foreach (string p in pList)
                    {
                        if (p == null) continue;

                        Console.WriteLine($"Deleting 0:\\Sys\\proc\\{p}...");
                        File.Delete($"0:\\Sys\\proc\\{p}");
                    }
                }
            }
            if(File.Exists("0:\\Sys\\PT"))
            {
                File.Delete("0:\\Sys\\PT");
            }

            ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\comp.so"), ["affeaffeaffe"], "tty_puts", "1:\\comp.so");
            //ApplicationRunner.runApplicationEntryPoint("test", TestFile.test_so, null, "tty_clear");
            //ApplicationRunner.runApplication("ctest", TestFile.test_c, null);
            SerialPort.Enable(COMPort.COM1, BaudRate.BaudRate115200);

            VoPo.Interrupts.InterruptHandler.Initialize();
        }

        protected override void Run()
        {
            ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\comp.so"), null, "tty_clear", "1:\\comp.so");
            //ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\test(1).so"), null, "this_does_not_exist");
            //ApplicationRunner.runApplication("ctest", File.ReadAllBytes("1:\\test.so"), null);
            //ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\test(1).so"), ["hallo"], "tty_puts");

            Sokolsh sokolsh = new Sokolsh();
            sokolsh.Loop();

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("System is powering off ...");
            Console.WriteLine("Closing Processes....");
            File.Delete("0:\\Sys\\PT");
            string[] pList = Directory.GetFiles("0:\\Sys\\proc");
            if(!(pList.Length == 0 || pList == null))
            {
                foreach (string p in pList)
                {
                    if (p == null) continue;

                    Console.WriteLine($"Deleting 0:\\Sys\\proc\\{p}...");
                    File.Delete($"0:\\Sys\\proc\\{p}");
                }
            } else
            {
                Console.WriteLine("No processes were running");
            }
            

            Thread.Sleep(500);
            Sys.Power.Shutdown();
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
