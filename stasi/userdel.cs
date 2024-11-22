using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.stasi
{
    public class userdel : Shell.BuiltIn
    {
        public override string Name => "userdel";

        public override string Description => "Deletes a user";

        public override ExitCode Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: userdel <username>");
                return ExitCode.Error;
            }
            if (!User.Exists(args[0]))
            {
                Console.WriteLine("This user does not exist!");
                return ExitCode.Error;
            }
            User.deleteUser(args[0]);
            return ExitCode.Success;
        }
    }
}
