using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.stasi
{
    internal class userinfo : Shell.BuiltIn
    {
        public override string Name => "userinfo";

        public override string Description => "Prints information about a user";

        public override ExitCode Execute(string[] args)
        {
            bool self = false;
            if (args.Length == 0)
            {
                self = true;
            }
            else {
                if (!User.Exists(args[0]))
                {
                    Console.WriteLine("This user does not exist!");
                    return ExitCode.Error;
                }
            }
            

            UserObj o;
            if (self)
            {
                o = Login.curUser;
                if(o == null)
                {
                    Console.WriteLine("PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC PANIC");
                    return ExitCode.Error;
                }
            }
            else
            {
                o = User.getUser(args[0]);
            }
            Console.WriteLine($"Username: {o.Username}");
            Console.WriteLine($"Name: {o.Name}");
            Console.WriteLine($"Home: {o.Home}");
            return ExitCode.Success;
        }
    }
}
