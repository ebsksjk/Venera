using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.stasi
{
    class useradd : Shell.BuiltIn
    {
        public override string Name => "useradd";

        public override string Description => "Adds a user";

        public override ExitCode Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: useradd -u username [-n name of user]");
                return ExitCode.Error;
            }

            string username = null;
            string name = null;
            foreach (string arg in args)
            {
                if (arg == "-u")
                {
                    username = args[Array.IndexOf(args, arg) + 1];
                }
                else if (arg == "-n")
                {
                    name = args[Array.IndexOf(args, arg) + 1];
                }
            }
            if(username == null)
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
