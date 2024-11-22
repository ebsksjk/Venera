using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.stasi
{
    public class usermod : Shell.BuiltIn
    {
        public override string Name => "usermod";

        public override string Description => "Modifies a user";

        public override ExitCode Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: usermod <username>");
                return ExitCode.Error;
            }

            if (!User.Exists(args[0]))
            {
                Console.WriteLine("This user does not exist!");
                return ExitCode.Error;
            }

            string newName = null;
            string newPassword = null;
            string newUsername = null;
            
            Console.Write("Enter new username for user ([ENTER] to keep): ");
            newUsername = Console.ReadLine();
            Console.Write("Enter new name for user ([ENTER] to keep): ");
            newName = Console.ReadLine();
            Console.Write("Enter new password for user ([ENTER] to keep): ");
            newPassword = Console.ReadLine();


            if (!string.IsNullOrWhiteSpace(newName) || !string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(newUsername))
            {
                UserObj o = User.getUser(args[0]);
                User.deleteUser(args[0]);
                User.createUser((newUsername != o.Username ? newUsername : o.Username), (newName != o.Name ? newName : o.Name), (newPassword != o.Password ? newPassword : o.Password));
                if(newUsername != o.Username)
                {
                    Console.WriteLine($"Username changed from {o.Username} to {newUsername}");

                    UserObj n = User.getUser(newUsername);

                    if(Login.curUser.Username == o.Username)
                    {
                        Login.curUser = n;
                        Login.curHome = Login.curUser.Home;
                    }
                    if (Directory.Exists(o.Home))
                    {
                        //Directory.Move(o.Home, Login.curHome); //this is heartbreaking qwq
                        Directory.Delete(o.Home, true);
                        Directory.CreateDirectory(n.Home);
                    }
                }
            }

            return ExitCode.Success;
        }
    }
}

