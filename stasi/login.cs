using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;
using System.Threading;

namespace Venera.stasi
{
    public static class Login
    {
        public static UserObj curUser;
        public static string curHome;
        public static uint curUID;

        private static bool login(string username, string password)
        {
            string[] lines = File.ReadAllLines(User.db);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts[1] == username && parts[3] == password)
                {
                    curUID = uint.Parse(parts[0]);
                    curUser = User.getUser(curUID);
                    curHome = curUser.Home;
                    return true;
                }
            }

            return false;
        }

        public static void loop()
        {
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine();
                Console.WriteLine("░▒▓█▓▒░░▒▓█▓▒░▒▓████████▓▒░▒▓███████▓▒░░▒▓████████▓▒░▒▓███████▓▒░ ░▒▓██████▓▒░ ");
                Console.WriteLine("░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine(" ░▒▓█▓▒▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine(" ░▒▓█▓▒▒▓█▓▒░░▒▓██████▓▒░ ░▒▓█▓▒░░▒▓█▓▒░▒▓██████▓▒░ ░▒▓███████▓▒░░▒▓████████▓▒░");
                Console.WriteLine("  ░▒▓█▓▓█▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine("  ░▒▓█▓▓█▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine("   ░▒▓██▓▒░  ░▒▓████████▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓████████▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine();

                if (!User.Exists("root"))
                {
                    File.AppendAllText(User.db, "0;root;rooti McRootikus;root;0:\\Users\\root");
                }

                if (!Directory.Exists("0:\\Users\\root"))
                {
                    Directory.CreateDirectory("0:\\Users\\root");
                }

                Console.SetCursorPosition(Console.WindowWidth / 2 - (17 / 2), 10);
                Console.WriteLine("Welcome to Venera");
                Console.WriteLine(" ╔══════════════════════════════════════════════╗ ");
                Console.Write(" ║  Username: ");
                string username = Console.ReadLine();
                Console.Write(" ║  Password: ");
                string password = Console.ReadLine();
                Console.WriteLine(" ╚══════════════════════════════════════════════╝ ");

                if (login(username, password))
                {
                    Console.WriteLine("Login successful");
                    Sokolsh sokolsh = new Sokolsh();
                    Kernel.PrintDebug(curHome);
                    sokolsh.Loop(curHome);
                    Console.WriteLine("exited loop!!!");
                }
                else
                {
                    Console.WriteLine("Login failed");
                    Thread.Sleep(500);
                }
            }
        }
    }
}
