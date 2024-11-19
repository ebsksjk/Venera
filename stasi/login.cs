using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            File.WriteAllText(User.db, "0;root;rooti mc rootikus;root;0:\\Users\\root");
            if(!Directory.Exists("0:\\Users\\root"))
            {
                Directory.CreateDirectory("0:\\Users\\root");
            }
            Console.WriteLine("Welcome to Venera");
            Console.WriteLine("Please login");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

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
            }
        }
    }
}
