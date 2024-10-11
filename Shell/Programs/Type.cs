using System;

namespace Venera.Shell.Programs {
    public class Type : BuiltIn {
        public override string Name => "type";

        public override string Description => "type out a file";

        public override ExitCode Execute(string[] args) {
            foreach (string i in args) {
                Console.WriteLine(i);
            }
            
            return ExitCode.Success;
        }
    }
}