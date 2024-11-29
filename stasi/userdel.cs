using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;

namespace Venera.stasi
{
    public class UserDel : Shell.BuiltIn
    {
        public override string Name => "userdel";

        public override string Description => "Deletes a user";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "username",
                    description: "System/Login name of user to kill.",
                    type: typeof(string),
                    required: true,
                    shortForm: 'u',
                    longForm: "username"
                )
            ]
        };

        protected override ExitCode Execute()
        {
            if (Args.Length == 0)
            {
                return ExitCode.Usage;
            }

            string username = (string)GetArgument("username");
            if (!User.Exists(username))
            {
                Console.WriteLine("This user does not exist!");
                return ExitCode.Error;
            }
            User.deleteUser(username);

            return ExitCode.Success;
        }
    }
}
