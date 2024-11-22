using Cosmos.HAL.BlockDevice;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.stasi
{
    static class User
    {
        public static uint UID = 1; //iterator über die ID aller benutzer
        public const string db = "0:\\Venera\\users.db"; //` der Pfad zur users.db.

        public static UserObj createUser(string username, string name, string password)
        {

            UserObj user = new UserObj(UID, username, name, password, $"0:\\Users\\{username}", DateTime.Now);

            if (!File.Exists(db))
            {
                File.Create(db);
            }
            File.AppendAllText(db, $"{user.cUID};{user.Username};{user.Name};{user.Password};{user.Home}\n");
            if (!Directory.Exists(user.Home))
            {
                Directory.CreateDirectory(user.Home);
            }

            return user;
        }

        public static UserObj getUser(string username)
        {
            string Username; //` der Nutzername des Benutzers.
            string Name; //` der Klarname des Benutzers.
            string Password; //` das Passwort des Benutzers.
            string Home;

            string[] lines = File.ReadAllLines(db);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts[1] == username)
                {
                    Username = parts[1];
                    Name = parts[2];
                    Password = parts[3];
                    Home = parts[4];
                    return new UserObj(uint.Parse(parts[0]), Username, Name, Password, Home, DateTime.Now);
                }
            }

            throw new Exception("User not found!!");
        }

        public static bool Exists(string username)
        {
            if (!File.Exists(db))
            {
                return false;
            }
            if (new FileInfo(db).Length == 0)
            {
                return false;
            }

            string[] lines = File.ReadAllLines(db);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts[1] == username)
                {
                    return true;
                }
            }

            return false;
        }

        public static void deleteUser(string username)
        {
            string[] lines = File.ReadAllLines(db);
            string[] newLines = new string[lines.Length - 1];
            int i = 0;
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts[1] != username)
                {
                    newLines[i] = line;
                    i++;
                }
            }
            File.WriteAllLines(db, newLines);
        }
    }

    public class UserObj
    {
        public uint cUID;
        public string Username; //` der Nutzername des Benutzers.
        public string Name; //` der Klarname des Benutzers.
        public string Password; //` das Passwort des Benutzers.
        public string Home; //` der Pfad zum Home-Verzeichnis des Benutzers.
        public DateTime LastLogin; //` das Datum des letzten Logins des Benutzers.

        public UserObj(uint cUID, string username, string name, string password, string home, DateTime lastLogin)
        {
            this.cUID = cUID;
            this.Username = username;
            this.Name = name;
            this.Password = password;
            this.Home = home;
            this.LastLogin = lastLogin;
        }
    }
}

