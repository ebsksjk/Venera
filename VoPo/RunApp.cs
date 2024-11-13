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
                Console.WriteLine("Usage: runapp [-e <ep>] -a [args] <args> ");
                return ExitCode.Error;
            }

            string path;
            string entrypoints = "main";
            List<Object> arguments = new List<Object>();

            if (args[^1].StartsWith(@"\"))
            {
                //if it is an absolute path
                path = $"0:{args[^1]}";
            }
            else
            {
                //if it is a relative path
                //convert it into the corresponding absolute path
                path = $"{Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory).EnsureBackslash()}{args[^1]}";
            }

            if(args.Length > 1 && args[0] == "-e") {
                entrypoints = args[1];
            }

            if ((args.Length > 1 || args.Length > 3) && (args[2] == "-a" || args[0] == "-a"))
            {
                int index = 0;
                index = (args[0] == "-a") ? 1 : 3;

                for (int i = index; i < args.Length -1 ; i++)
                {
                    if (int.TryParse(args[i], out int result))
                    {
                        Kernel.PrintDebug($"Pushing int: {result}");
                        arguments.Add((object)result);
                    }
                    else
                    {
                        Kernel.PrintDebug($"Pushing string: {args[i]}");
                        arguments.Add((object)args[i]);
                    }
                }
            }

            foreach (var arg in arguments)
            {
                Kernel.PrintDebug($"Argument: {arg}");
            }

            ApplicationRunner.runApplicationEntryPoint($"runapp call", File.ReadAllBytes(path), arguments, entrypoints, path);

            return ExitCode.Success;
        }

    }
}
