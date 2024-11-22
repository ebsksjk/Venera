using System;
using Venera.Shell;

namespace Venera.stasi
{
    internal class UserInfo : BuiltIn
    {
        public override string Name => "userinfo";

        public override string Description => "Prints information about a user";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "username",
                    description: "System/Login name of new user",
                    type: typeof(string),
                    argsPosition: 0
                )
            ]
        };

        protected override ExitCode Execute()
        {
            bool self = false;
            if (Args.Length == 0)
            {
                self = true;
            }
            else
            {
                if (!User.Exists((string)GetArgument(0)))
                {
                    Console.WriteLine("This user does not exist!");
                    return ExitCode.Error;
                }
            }


            UserObj o;
            if (self)
            {
                o = Login.curUser;
                if (o == null)
                {
                    Console.WriteLine("PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC");
                    return ExitCode.Error;
                }
            }
            else
            {
                o = User.getUser((string)GetArgument(0));
            }
            Console.WriteLine($"Username: {o.Username}");
            Console.WriteLine($"Name: {o.Name}");
            Console.WriteLine($"Home: {o.Home}");
            return ExitCode.Success;
        }
    }
}
