using CosmosELFCore;
using System;
using System.Collections.Generic;
using System.IO;
using Venera.Shell;
using XSharp.x86.Params;

namespace Venera.VoPo
{
    public class RunApp : BuiltIn
    {
        public override string Name => "runapp";

        public override string Description => "Runs an entry point of a given application. Trys to run main, if no entry point is given.";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "args",
                    description: "List of arguments to pass to the program.",
                    type: typeof(string[]),
                    argsPosition: 0
                ),
                new(
                    valueName: "entrypoints",
                    description: "List of entrypoints to execute",
                    type: typeof(string[]),
                    shortForm: 'e',
                    longForm: "entrypoints"
                ),
            ]
        };

        protected override ExitCode Execute()
        {

            if (Args.Length == 0)
            {
                Console.WriteLine("Usage: runapp [-e <ep>] -a [args] <args> ");
                return ExitCode.Error;
            }

            string path;
            string entrypoints = "main";
            List<Object> arguments = new List<Object>();

            if (Args[^1].StartsWith(@"\"))
            {
                //if it is an absolute path
                path = $"0:{Args[^1]}";
            }
            else
            {
                //if it is a relative path
                //convert it into the corresponding absolute path
                path = $"{Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory).EnsureBackslash()}{Args[^1]}";
            }

            if (Args.Length > 1 && Args[0] == "-e")
            {
                entrypoints = Args[1];
            }

            if ((Args.Length > 1 || Args.Length > 3) && (Args[2] == "-a" || Args[0] == "-a"))
            {
                int index = 0;
                index = (Args[0] == "-a") ? 1 : 3;

                for (int i = index; i < Args.Length - 1; i++)
                {
                    if (int.TryParse(Args[i], out int result))
                    {
                        Kernel.PrintDebug($"Pushing int: {result}");
                        arguments.Add((object)result);
                    }
                    else
                    {
                        Kernel.PrintDebug($"Pushing string: {Args[i]}");
                        arguments.Add((object)Args[i]);
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
