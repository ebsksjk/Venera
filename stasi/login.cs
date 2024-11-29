using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Venera.Shell;

namespace Venera.stasi
{
    public static class Login
    {
        public static UserObj curUser;
        public static string curHome;
        public static uint curUID;

        private static bool login(string username, string password)
        {
            Kernel.PrintDebug($"checking for {username}+{password}");
            if (!File.Exists(User.db))
            {
                Console.WriteLine("ERROR: users file is non existent. This is not expected behaviour and you should not see this error. This is unrecoverable.");
                return false;
            }
            string[] lines = File.ReadAllLines(User.db);
            //Kernel.PrintDebug(File.ReadAllText(User.db));
            if (lines == null || lines.Length == 0 || lines[0].Length <= 1)
            {
                Console.WriteLine("ERROR: users file is empty. This is not expected behaviour and you should not see this error. This is unrecoverable.");
                return false;
            }
            foreach (string line in lines)
            {
                if (line == null)
                {
                    Console.WriteLine("ERROR: users file is empty. This is not expected behaviour and you should not see this error. This is unrecoverable.");
                    return false;
                }


                string[] parts = line.Split(';');

                if (line == null || parts.Length == 0)
                {
                    Console.WriteLine("ERROR: users file is empty. This is not expected behaviour and you should not see this error. This is unrecoverable.");
                    return false;
                }

                //Kernel.PrintDebug($"lline: {line} with len: {line.Length}");

                if (parts[0] == null || parts[1] == null || parts[2] == null || parts[3] == null || parts[4] == null)
                {
                    Console.WriteLine("ERROR: users file is corrupted and/or unreadable. This is not expected behaviour and you should not see this error. This is unrecoverable.");
                    return false;
                }

                if (parts[1] == username && parts[3] == password)
                {
                    curUID = uint.Parse(parts[0]);
                    curUser = User.getUser(username);
                    if (curUser == null)
                    {
                        Console.WriteLine("ERROR: users file is corrupted and/or unreadable. This is not expected behaviour and you should not see this error. This is unrecoverable.");
                        return false;
                    }
                    curHome = parts[4];
                    Kernel.PrintDebug($"logged in as {curUser.Username} with home {curHome}");
                    return true;
                }
            }

            return false;
        }

        public static string getConsoleString(bool password=false)
        {
            string ret = "";
            while(true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                
                if (key.Key == ConsoleKey.Enter)
                {
                    return ret;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (ret.Length > 0)
                    {
                        ret = ret.Substring(0, ret.Length - 1);
                        int x; int y;
                        (x, y) = Console.GetCursorPosition();
                        Console.SetCursorPosition(x - 1, y);
                        Console.Write(' '); 
                        Console.SetCursorPosition(x - 1, y);
                    }
                }
                else
                {
                    ret += key.KeyChar;
                    if(password)
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(key.KeyChar);
                    }
                }
            }
        }

        public static void loop()
        {
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine();
                Console.WriteLine(" ░▒▓█▓▒░░▒▓█▓▒░▒▓████████▓▒░▒▓██████▓▒░░▒▓████████▓▒░▒▓███████▓▒░ ░▒▓██████▓▒░ ");
                Console.WriteLine(" ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine("  ░▒▓█▓▒▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine("  ░▒▓█▓▒▒▓█▓▒░░▒▓██████▓▒░ ░▒▓█▓▒░▒▓█▓▒░▒▓██████▓▒░ ░▒▓███████▓▒░░▒▓████████▓▒░");
                Console.WriteLine("   ░▒▓█▓▓█▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine("   ░▒▓█▓▓█▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine("    ░▒▓██▓▒░  ░▒▓████████▓▒░▒▓█▓▒░▒▓█▓▒░▒▓████████▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░");
                Console.WriteLine();

                Console.SetCursorPosition(Console.WindowWidth / 2 - (37 / 2), 10);
                Console.WriteLine("Welcome on Venera (type quit to exit)");
                Console.SetCursorPosition(Console.WindowWidth / 2 - (48 / 2), 11);
                Console.WriteLine("╔══════════════════════════════════════════════╗");
                Console.SetCursorPosition(Console.WindowWidth / 2 - (48 / 2), 12);
                Console.WriteLine("║ Username:                                    ║");
                Console.SetCursorPosition(Console.WindowWidth / 2 - (48 / 2), 13);
                Console.WriteLine("║ Password:                                    ║");
                Console.SetCursorPosition(Console.WindowWidth / 2 - (48 / 2), 14);
                Console.WriteLine("╚══════════════════════════════════════════════╝");


                Console.SetCursorPosition(Console.WindowWidth / 2 - (48 / 2) + 12, 12);
                string username = getConsoleString();
                if (username == "quit")
                {
                    break;
                }
                if (username == "PANIC")
                {
                    Console.WriteLine("Dropping in an emergency shell... Come back when you fixed this mess...");
                    curUser = null;
                    Kernel.SokolshInstance.Loop("0:\\");
                    continue;
                }
                Console.SetCursorPosition(Console.WindowWidth / 2 - (48 / 2) + 12, 13);
                string password = getConsoleString(true);
                

                if (!User.Exists("root"))
                {
                    User.createUser("root", "Rooti McRootikus", "root");
                }

                if (login(username, password))
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 - (16 / 2), 14);
                    Console.WriteLine("Login successful");
                    Kernel.PrintDebug(curHome);
                    Kernel.SokolshInstance.Loop(curHome);
                    Console.WriteLine("exited loop!!!");
                }
                else
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 - (26 / 2), 14);
                    Console.WriteLine("Login failed. Try again!!!!");
                    Thread.Sleep(500);
                }
            }

            Console.WriteLine("shutting down uwu!");
        }
    }
}
