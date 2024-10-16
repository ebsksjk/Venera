using System;

namespace Venera.Shell.Programs
{
    public class Echo : BuiltIn
    {
        public override string Name => "echo";

        public override string Description => "Prints the args";

        public override ExitCode Execute(string[] args)
        {
            string ret = "";

            foreach (string i in args)
            {
                ret += i + ' ';
            }

            Console.WriteLine(ret);

            return ExitCode.Success;
        }
    }
}
