using Cosmos.HAL.Drivers.Video;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using CosmosELF;
using CosmosELFCore;
using System;
using System.Drawing;
using System.Text;
using System.Threading;
using Venera.Graphics;
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
        public static Chromat Chromat;


        protected override void BeforeRun()
        {
            unsafe
            {
                fixed (byte* ptr = TestFile.test_so)
                {
                    var exe = new UnmanagedExecutable(ptr);
                    exe.Load();
                    exe.Link();

                    Console.WriteLine("Executing");
                    new ArgumentWriter();
                    exe.Invoke("tty_clear");

                    new ArgumentWriter()
                        .Push(5)  //fg
                        .Push(15); //bg
                    exe.Invoke("tty_set_color");

                    fixed (byte* str = Encoding.ASCII.GetBytes("Hello World"))
                    {
                        new ArgumentWriter()
                            .Push((uint)str);
                        exe.Invoke("tty_puts");
                    }


                }
            }

            FileSystem = new Cosmos.System.FileSystem.CosmosVFS();
            VFSManager.RegisterVFS(FileSystem);

            _environment = new();
            _environment.Set(DefaultEnvironments.CurrentWorkingDirectory, @"0:\");

            Chromat = new Chromat();
        }

        protected override void Run()
        {
            Chromat.Display();
            Chromat.DrawText("Hello world GUI!");

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
