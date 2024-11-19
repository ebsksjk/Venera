using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Kosmovim
{
    class kosmovim : Shell.BuiltIn
    {
        public override string Name => "kosmovim";

        public override string Description => "edit plaintext files";

        public override ExitCode Execute(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine($"Usage: kosmovim <file>");
                return ExitCode.Error;
            }

            string path = args[0].AbsoluteOrRelativePath();
            FileStream file = null;
            if (!File.Exists(path))
            {
                file = File.Create(path);
            } else
            {
                file = File.Open(path, FileMode.Open);
            }
            if(file == null)
            {
                Console.WriteLine($"kosmovim: cannot open file {path}");
                return ExitCode.Error;
            }

            File.ReadAllText(path);
            file.Close();

            return ExitCode.Success;
        }
    }
}
