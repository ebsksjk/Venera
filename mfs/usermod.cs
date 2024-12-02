using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;

namespace Venera.stasi
{
    public class UserMod : Shell.BuiltIn
    {
        public override string Name => "usermod";

        public override string Description => "Modifies a user";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "username",
                    description: "System/Login name of user to modify.",
                    type: typeof(string),
                    argsPosition: 0
                )
            ]
        };

        protected override ExitCode Execute()
        {
            if (Args.Length == 0)
            {
                return ExitCode.Usage;
            }

            string username = (string)GetArgument(0);

            if (!User.Exists(username))
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
                UserObj o = User.getUser(username);
                User.deleteUser(username);
                User.createUser((newUsername != o.Username ? newUsername : o.Username), (newName != o.Name ? newName : o.Name), (newPassword != o.Password ? newPassword : o.Password));
                if (newUsername != o.Username)
                {
                    Console.WriteLine($"Username changed from {o.Username} to {newUsername}");

                    UserObj n = User.getUser(newUsername);

                    if (Login.curUser.Username == o.Username)
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

