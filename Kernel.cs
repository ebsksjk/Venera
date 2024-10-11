using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Venera.Shell;
using Sys = Cosmos.System;

namespace Venera
{
    public class Kernel : Sys.Kernel
    {
        public static string OS_NAME = "Venera";
        public static string OS_VERSION = "1.0";

        private static Environment<string> _environment;

        public static Environment<string> GlobalEnvironment { get => _environment; }

        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully. Type a line of text to get it echoed back.");

            _environment = new();
            _environment.Set(DefaultEnvironments.CurrentWorkingDirectory, "/");
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
