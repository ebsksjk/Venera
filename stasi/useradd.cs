using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;

namespace Venera.stasi
{
    class UserAdd : Shell.BuiltIn
    {
        public override string Name => "useradd";

        public override string Description => "Adds a user";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "username",
                    description: "System/Login name of new user",
                    type: typeof(string),
                    required: true,
                    shortForm: 'u',
                    longForm: "username"
                ),
                new(
                    valueName: "name",
                    description: "Full name of new user.",
                    type: typeof(string),
                    shortForm: 'n',
                    longForm: "name"
                ),
            ]
        };

        protected override ExitCode Execute()
        {
            if (Args.Length == 0)
            {
                Console.WriteLine("Usage: useradd -u username [-n name of user]");
                return ExitCode.Error;
            }

            string username = (string)GetArgument("username");
            string name = (string)GetArgument("name");

            if (username == null)
            {
                Console.WriteLine("Usage: useradd -u username [-n name of user]");
                return ExitCode.Error;
            }

            if (User.Exists(username))
            {
                Console.WriteLine("This user does already exist!");
                return ExitCode.Error;
            }

            Console.Write($"Enter password for {username}: ");
            string password = Console.ReadLine();

            User.createUser(username, (name ?? ""), password);
            Console.WriteLine($"User {username} created!");
            return ExitCode.Success;
        }
    }
}
