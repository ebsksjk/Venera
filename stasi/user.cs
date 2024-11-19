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
        public static uint UID = 0; //iterator über die ID aller benutzer
        public const string db = "0:\\Venera\\users.db"; //` der Pfad zur users.db.

        public static UserObj createUser(string username, string name, string password)
        {
            UserObj user = new UserObj(UID, username, name, password, $"0:\\Users\\{username}", DateTime.Now);

            if (!File.Exists(db))
            {
                File.Create(db);
            }
            File.AppendAllText(db, $"{user.cUID};{user.Username};{user.Name};{user.Password};{user.Home}\n");
            if(!Directory.Exists(user.Home))
            {
                Directory.CreateDirectory(user.Home);
            }

            return user;
        }

        public static UserObj getUser(uint uid) {
            string Username; //` der Nutzername des Benutzers.
            string Name; //` der Klarname des Benutzers.
            string Password; //` das Passwort des Benutzers.
            string Home;

            string[] lines = File.ReadAllLines(db);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts[0] == uid.ToString())
                {
                    Username = parts[1];
                    Name = parts[2];
                    Password = parts[3];
                    Home = parts[4];
                    return new UserObj(uid, Username, Name, Password, Home, DateTime.Now);
                }
            }

            throw new Exception("User not found!!");
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

