using Cosmos.HAL;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Security.Cryptography;
using System.Threading;
using Venera.Shell;
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

            //ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\test.so"), null, "tty_clear");
            //ApplicationRunner.runApplicationEntryPoint("test", TestFile.test_so, ["a"], "tty_puts");

            FileSystem = new Cosmos.System.FileSystem.CosmosVFS();
            VFSManager.RegisterVFS(FileSystem);

            Console.WriteLine("Welcome on Venera");
            // in BeforeRun() or when user calls a "command"

            _environment = new();
            _environment.Set(DefaultEnvironments.CurrentWorkingDirectory, @"0:\");

            //ApplicationRunner.runApplicationEntryPoint("test", TestFile.test_so, ["affeaffeaffe"], "tty_puts");
            //ApplicationRunner.runApplicationEntryPoint("test", TestFile.test_so, null, "tty_clear");
            //ApplicationRunner.runApplication("ctest", TestFile.test_c, null);
            SerialPort.Enable(COMPort.COM1, BaudRate.BaudRate115200);

            using (var xClient = new DHCPClient())
            {
                /** Send a DHCP Discover packet **/
                //This will automatically set the IP config after DHCP response
                xClient.SendDiscoverPacket();
            }
        }

        protected override void Run()
        {
            //ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\test(1).so"), null, "tty_clear");
            //ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\test(1).so"), null, "this_does_not_exist");
            //ApplicationRunner.runApplication("ctest", File.ReadAllBytes("1:\\test.so"), null);
            //ApplicationRunner.runApplicationEntryPoint("test", File.ReadAllBytes("1:\\test(1).so"), ["hallo"], "tty_puts");

            Sokolsh sokolsh = new Sokolsh();
            sokolsh.Loop();

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("System is powering off ...");
            Thread.Sleep(500);
            Sys.Power.Shutdown();
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
