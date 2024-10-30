using System;
using Venera.Shell;
using System.IO;
using CosmosELFCore;
using XSharp.x86.Params;
using System.Collections.Generic;

namespace Venera.VoPo
{
    public class RunApp : BuiltIn
    {
        public override string Name => "runapp";

        public override string Description => "Runs an entry point of a given application. Trys to run main, if no entry point is given.";

        public override ExitCode Execute(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: runapp <args> [-e <eps>]");
                return ExitCode.Error;
            }

            string path;
            List<string> entrypoints = new List<string>();

            if (args[0].StartsWith(@"\"))
            {
                //if it is an absolute path
                path = $"0:{args[0]}";
            }
            else
            {
                //if it is a relative path
                //convert it into the corresponding absolute path
                path = $"{Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory).EnsureBackslash()}{args[0]}";
            }

            if(args.Length > 1 && args[1] == "-e") {
                for (int i = 2; i < args.Length; i++)
                {
                    entrypoints.Add(args[i].Trim());
                }
            }

            foreach(string v in entrypoints)
            {
                ApplicationRunner.runApplicationEntryPoint("runapp", File.ReadAllBytes(path), null, v);
            }

            return ExitCode.Success;
        }

    }
}
