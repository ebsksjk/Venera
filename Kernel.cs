using Cosmos.HAL;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Venera.Graphics;
using Venera.Shell;
using Venera.stasi;
using System;
using XSharp.x86.Params;
using Sys = Cosmos.System;
using System.Text;
using Cosmos.System.ExtendedASCII;

namespace Venera
{
    public /*unsafe*/ class Kernel : Sys.Kernel
    {
        public static string OS_NAME = "Venera";
        public static string OS_VERSION = "0.1";

        private static Environment<string> _environment;
        public static Sokolsh SokolshInstance;

        public static Environment<string> GlobalEnvironment { get => _environment; }

        public static Sys.FileSystem.CosmosVFS FileSystem;
        public static Chromat Chromat;

        protected override void BeforeRun()
        {
            SerialPort.Enable(COMPort.COM1, BaudRate.BaudRate115200);
            //Cosmos.System.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.DEStandardLayout());

            //ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\comp.so"), null, "tty_clear", "1:\\comp.so");
            //ApplicationRunner.runApplicationEntryPoint("test", TestFile.test_so, ["a"], "tty_puts");

            FileSystem = new Cosmos.System.FileSystem.CosmosVFS();
            VFSManager.RegisterVFS(FileSystem);

            Console.WriteLine("Welcome on Venera");
            Kernel.PrintDebug("Welcome on Venera");
            // in BeforeRun() or when user calls a "command"

            _environment = new();
            _environment.Set(DefaultEnvironments.CurrentWorkingDirectory, @"0:\");
            Kernel.PrintDebug("Set cwd! to 0:\\");
            if (!Directory.Exists("0:\\Venera\\Sys"))
            {
                Directory.CreateDirectory("0:\\Venera\\Sys");
            }
            if (!Directory.Exists("0:\\Venera\\Sys\\proc"))
            {
                Directory.CreateDirectory("0:\\Venera\\Sys\\proc");
            }
            else if (Directory.GetFiles("0:\\Venera\\Sys\\proc").Length != 0)
            {
                Kernel.PrintDebug("Process dir not empty, clearing....");
                string[] pList = Directory.GetFiles("0:\\Venera\\Sys\\proc");
                if (!(pList.Length == 0 || pList == null))
                {
                    foreach (string p in pList)
                    {
                        if (p == null) continue;

                        Console.WriteLine($"Deleting 0:\\Venera\\Sys\\proc\\{p}...");
                        File.Delete($"0:\\Venera\\Sys\\proc\\{p}");
                    }
                }
            }
            if (File.Exists("0:\\Venera\\Sys\\PT"))
            {
                File.Delete("0:\\Venera\\Sys\\PT");
            }
            if (!Directory.Exists("0:\\Venera"))
            {
                Directory.CreateDirectory("0:\\Venera");
            }
            if (!Directory.Exists("0:\\Users"))
            {
                Directory.CreateDirectory("0:\\Users");
            }
            Kernel.PrintDebug("Created Venera dirs!");

            Kernel.PrintDebug("trying to print welcome message from test elf");
            //ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\comp.so"), ["Welcome on Venera!"], "tty_puts", "1:\\comp.so");
            Kernel.PrintDebug("ran test elf!!!");
            SerialPort.Enable(COMPort.COM1, BaudRate.BaudRate115200);
            Kernel.PrintDebug("Set COM speed");

            //VoPo.Interrupts.InterruptHandler.Initialize();
            //CPU.UpdateIDT(true);
            //CPU.EnableInterrupts();
            //Console.WriteLine(CPU.GetAmountOfRAM() + " MB");
            //GetInterruptHandler((byte)0x80);
            //VoPo.Interrupts.InterruptHandler.getVenIntHandler(); 
            using (var xClient = new DHCPClient())
            {
                /** Send a DHCP Discover packet **/
                //This will automatically set the IP config after DHCP response
                xClient.SendDiscoverPacket();
            }
            Kernel.PrintDebug("Created DHCP client!");
            Encoding.RegisterProvider(Cosmos.System.ExtendedASCII.CosmosEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.GetEncoding(437);
            Console.InputEncoding = Encoding.GetEncoding(437);
            Kernel.PrintDebug("Set System encoding!");
            Sys.KeyboardManager.SetKeyLayout(new Cosmos.System.ScanMaps.DEStandardLayout());
            Kernel.PrintDebug("Set Keyboard layout!");
        }

        protected override void Run()
        {
            SokolshInstance = new Sokolsh();
            Kernel.PrintDebug("Created Shell instance! - dropping into login shell:");
            Login.loop();

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("System is powering off ...");
            File.Delete("0:\\Venera\\Sys\\PT");
            string[] pList = Directory.GetFiles("0:\\Venera\\Sys\\proc");
            if (!(pList.Length == 0 || pList == null))
                if (!(pList.Length == 0 || pList == null))
                {
                    foreach (string p in pList)
                    {
                        if (p == null) continue;

                        Console.WriteLine($"Deleting 0:\\Venera\\Sys\\proc\\{p}...");
                        File.Delete($"0:\\Venera\\Sys\\proc\\{p}");
                    }
                }
                else
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
