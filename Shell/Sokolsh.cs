using System;
using System.Collections.Generic;
using System.Linq;
using Venera.Shell.Programs;

namespace Venera.Shell
{

    public class Sokolsh
    {
        // Built-ins must be defined explicitly here because reflections are not available
        // in a COSMOS environment and this is the most easy solution.
        private static readonly List<BuiltIn> _availableBuiltIns = new()
            {
                new Pwd(),
                new About(),
                new Help(),
                new Clear(),
                new Programs.Type(),
                new Echo(),
                new Disk(),
                new Ls(),
                new Mkdir(),
                new Cd(),
                new Reboot(),
                new Shutdown(),
                new Cat(),
                new Ip(),
                new Ping(),
                new Programs.Sputnik(),
                new VoPo.ELFInfo(),
                new VoPo.RunApp(),
            };

        public static List<BuiltIn> AvailableBuiltIns { get { return _availableBuiltIns; } }

        public Sokolsh()
        {
        }

        public void Loop()
        {
            while (true)
            {
                PrintPrefix();

                string input = Console.ReadLine();

                switch (Execute(input))
                {
                    case ExecutionReturn.Empty:
                        // Do nothing, just print next line.
                        break;
                    case ExecutionReturn.NotFound:
                        Console.WriteLine("Command not found. Enter \"help\" or \"?\" to get a list of built-in commands.");
                        break;
                    case ExecutionReturn.Success:
                    case ExecutionReturn.Failure:
                        // TODO: Implement something, idk.
                        break;
                    case ExecutionReturn.Exit:
                        // Exit current shell. This will end the operating system, ideally.
                        return;
                }
            }
        }

        private bool FindBuiltIn(string name, out BuiltIn result)
        {
            foreach (BuiltIn builtIn in _availableBuiltIns)
            {
                if (builtIn.Name == name)
                {
                    result = builtIn;
                    return true;
                }
            }

            result = null;
            return false;
        }

        private ExecutionReturn Execute(string input)
        {
            if (input.StartsWith("exit"))
            {
                return ExecutionReturn.Exit;
            }

            string[] parts = input.Split(' ');
            if (parts.Length == 0) return ExecutionReturn.Empty;

            var commandName = parts[0];

            // TODO: Support "things like this to be one argument"
            var args = parts.Skip(1).ToArray();

            if (FindBuiltIn(commandName, out BuiltIn command))
            {
                ExitCode status = command.Execute(args);

                return status == ExitCode.Success
                    ? ExecutionReturn.Success
                    : ExecutionReturn.Failure;
            }

            return ExecutionReturn.NotFound;
        }

        private void PrintPrefix()
        {
            string cwd = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(cwd);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" $ ");
        }
    }
}
