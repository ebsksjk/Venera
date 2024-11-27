using System;
using System.Collections.Generic;
using System.IO;
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
                new Sputnik(),
                new Man(),
                new ArgTest(),
                new VoPo.ELFInfo(),
                new VoPo.RunApp(),
                new VoPo.Faketop(),
                new VoPo.Interrupts.manint(),
                new Kosmovim.KosmoVim(),
                new Kosmovim.GenJunk(),
                new stasi.UserAdd(),
                new stasi.UserDel(),
                new stasi.UserMod(),
                new stasi.UserInfo(),
                new Rm(),
            };

        public static List<BuiltIn> AvailableBuiltIns { get { return _availableBuiltIns; } }

        public Sokolsh()
        {
        }

        /// <summary>
        /// Helper function to take any command line and parses it into a string array.
        /// </summary>
        /// <param name="cmdLine">Raw string entered by user.</param>
        public static string[] SplitArgs(string cmdLine)
        {
            Kernel.PrintDebug($"Split args from: {cmdLine}");
            List<string> args = new();

            bool insideQuotes = false;
            string currentArgs = string.Empty;

            for (int i = 0; i < cmdLine.Length; i++)
            {
                char c = cmdLine[i];

                // If we hit the last char, we add it to the list and break instantly.
                if (i == cmdLine.Length - 1)
                {
                    if (c != '"')
                    {
                        currentArgs += c;
                    }

                    args.Add(currentArgs);
                    break;
                }

                if (c == ' ' && !insideQuotes)
                {
                    args.Add(currentArgs);
                    currentArgs = string.Empty;
                    continue;
                }

                if (c == '"')
                {
                    if (insideQuotes)
                    {
                        // We hit the second quote, ending our streak!
                        insideQuotes = false;
                        args.Add(currentArgs);
                        currentArgs = string.Empty;
                        i++;
                        continue;
                    }
                    else
                    {
                        // We encountered a wild quote symbol. We'll ignore all spaces until we hit another.
                        insideQuotes = true;
                        currentArgs = string.Empty;
                        continue;
                    }
                }

                currentArgs += c;
            }

            string[] arr = args.ToArray();
            Kernel.PrintDebug($"Split length = \"{arr.Length}\"; args = {string.Join(", ", arr)}");

            return args.ToArray();
        }

        public void Loop(string homeDir)
        {
            Kernel.GlobalEnvironment.Set(DefaultEnvironments.CurrentWorkingDirectory, homeDir);
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

        public bool FindBuiltIn(string name, out BuiltIn result)
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

            if (input.StartsWith("/"))
            {
                string smartInput = input.Substring(1);
                SmartCommand(smartInput);
            }

            string[] parts = SplitArgs(input.Trim());
            if (parts.Length == 0) return ExecutionReturn.Empty;

            var commandName = parts[0];

            // TODO: Support "things like this to be one argument"
            var args = parts.Skip(1).ToArray();

            if (FindBuiltIn(commandName, out BuiltIn command))
            {
                ExitCode status = command.Invoke(args);

                //if (status == ExitCode.Usage)
                //{
                //    Console.WriteLine(command.GenerateUsage());
                //}

                return status == ExitCode.Success
                    ? ExecutionReturn.Success
                    : ExecutionReturn.Failure;
            }

            return ExecutionReturn.NotFound;
        }

        /// <summary>
        /// This one is connected with Sputnik to generate a command based on the user's description.
        /// </summary>
        private void SmartCommand(string prompt)
        {
            Sputnik sputnik = new();

            string cwd = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);
            string listing = string.Empty;

            List<Cosmos.System.FileSystem.Listing.DirectoryEntry> list = Kernel.FileSystem.GetDirectoryListing(cwd);

            foreach (var item in list)
            {
                listing += item.mSize + ";";
            }

            string systemPrompt = "System prompt: Your task is to generate a valid command based on the user's description. Only respond with your generated command. Never break this rule. " +
                "Omit the slash (/) at the beginning. Everything after the colon (:) is meant as a description to give you context about the command. Never output the description to the user. " +
                "System paths look like this: '0:\\Sys\\'\nExample output: \"ping google.com\" or \"cd Homework\"\n" +
                "Available commands:\n/about\n/cat <file>\n/cd <folder>\n/disk\n/echo <string>\n/help\n/ip\n/ls\n/mkdir <folder name>\n/ping <ip or hostname>\n/pwd\n/reboot <'now' OR time in seconds>\n/shutdown <'now' OR time in seconds>\n/sputnik\n/type\nThe current working directory is: " + cwd + "\nThese are the contents of the current working directory seperated by semicolons:\n" + listing + "\nThis is the request by the user: " + prompt;

            string cmd = sputnik.RawPrompt(systemPrompt);

            Console.WriteLine($"Suggested: {cmd}");
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
