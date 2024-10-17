using Cosmos.System.FileSystem.VFS;
using CosmosELF;
using CosmosELFCore;
using System;
using System.Text;
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

            ApplicationRunner.runApplicationEntryPoint("test", TestFile.test_so, null, "tty_clear");
            ApplicationRunner.runApplicationEntryPoint("test", TestFile.test_so, ["a"], "tty_puts");

            FileSystem = new Cosmos.System.FileSystem.CosmosVFS();
            VFSManager.RegisterVFS(FileSystem);

            Console.WriteLine("Welcome on Venera");
            // in BeforeRun() or when user calls a "command"

            _environment = new();
            _environment.Set(DefaultEnvironments.CurrentWorkingDirectory, @"0:\");
        }

        protected override void Run()
        {
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
