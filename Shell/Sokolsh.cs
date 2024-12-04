using Cosmos.System.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Venera.Shell.Pipes;
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
                new Calc(),
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
                new RunChromat(),
                new ReadStdin(),
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
                new Klier(),
                new froeh(),
                new Pride(),
                new SadFace(),
            };

        public static List<BuiltIn> AvailableBuiltIns { get { return _availableBuiltIns; } }
        public static List<string> lastCommands;
        public static int curLine = 0;

        public static string UsageText { get; private set; }
        public Sokolsh()
        {
            Console.Write("Sokolsh: Prepare smart command ...");
            UsageText = string.Empty;
            foreach (BuiltIn builtIn in AvailableBuiltIns)
            {
                UsageText += $"---\n{builtIn.GenerateUsage()}\n";
            }
            Console.WriteLine(" Done.");
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
            lastCommands = new List<string>();

            while (true)
            {
                PrintPrefix();
                (int x, curLine) = Console.GetCursorPosition();

                string input = LoopInput();

                if (input.Trim() != string.Empty)
                {
                    Kernel.PrintDebug($"input: {input}");
                    lastCommands.Add(input);
                    Kernel.PrintDebug($"count: {lastCommands.Count}");
                }

                //Kernel.PrintDebug($"Last commands: {string.Join(", ", lastCommands)}");
                switch (Execute(input).StatusCode)
                {
                    case ShellResult.Empty:
                        // Do nothing, just print next line.
                        break;
                    case ShellResult.NotFound:
                        Console.WriteLine("Command not found. Enter \"help\" or \"?\" to get a list of built-in commands.");
                        break;
                    case ShellResult.Success:
                    case ShellResult.Failure:
                        // TODO: Implement something, idk.
                        break;
                    case ShellResult.Exit:
                        // Exit current shell. This will end the operating system, ideally.
                        return;
                }
            }
        }

        private string LoopInput()
        {
            string command = "";
            int lastCommandIndex = lastCommands.Count - 1;
            Kernel.PrintDebug($"Last command index: {lastCommandIndex}");
            while (true)
            {
                if (Console.KeyAvailable)
                {

                    ConsoleKeyInfo key = Console.ReadKey(true); // Non-blocking key read

                    // Handle Enter key (submit action)
                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        lastCommandIndex = lastCommands.Count - 1;
                        return command;
                    }
                    // Handle Backspace key (remove last character)
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (command.Length > 0)
                        {
                            command = command.Substring(0, command.Length - 1);
                            int x, y;
                            (x, y) = Console.GetCursorPosition();
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write(' ');
                            Console.SetCursorPosition(x - 1, y);
                        }
                    }
                    else if (key.Key == ConsoleKey.UpArrow && lastCommandIndex >= 0)
                    {
                        Kernel.PrintDebug("upppp!!!");
                        Kernel.PrintDebug($"Last command index: {lastCommandIndex}");
                        Kernel.PrintDebug($"Last command count: {lastCommands.Count}");
                        Kernel.PrintDebug($"command: {command}");
                        Kernel.PrintDebug($"Last command: {lastCommands[lastCommandIndex]}");

                        command = lastCommands[lastCommandIndex];
                        Console.SetCursorPosition(GetPrefix().Length, curLine);
                        Console.Write("                                                                         ");
                        Console.SetCursorPosition(GetPrefix().Length, curLine);
                        Console.Write(command);
                        lastCommandIndex--;
                        if (lastCommandIndex < 0)
                        {
                            lastCommandIndex = lastCommands.Count - 1;
                        }
                    }
                    else
                    {
                        command += key.KeyChar;
                        Console.Write(key.KeyChar); // Otherwise, print the actual character
                    }
                }

                Kosmovim.ConsoleTextTweaks.Sparkle();
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

        private ExecutionResult Execute(string input, byte[] stdin = null, bool pipe = false)
        {
            #region Shell handled commands

            // Exit is not really a built-in command is directly handled by the shell.
            if (input.StartsWith("exit"))
            {
                return new ExecutionResult(ShellResult.Exit, null);
            }

            // Smart command is, again, no built-in and is handled directly.
            if (input.StartsWith("/"))
            {
                string smartInput = input.Substring(1);
                SmartCommand(smartInput);

                return new ExecutionResult(ShellResult.Success, null);
            }

            #endregion

            string[] pipedSegments = input.Split('|');

            // Example: echo something something cool | stdin_read
            if (pipedSegments.Length > 1)
            {
                byte[] lastStdout = null;

                // This could be solved using recursion, but no.
                for (int i = 0; i < pipedSegments.Length; i++)
                {
                    bool usePipe = i != pipedSegments.Length - 1;
                    ExecutionResult executionResult = Execute(pipedSegments[i], lastStdout, usePipe);

                    if (executionResult.StatusCode != ShellResult.Success)
                    {
                        return new ExecutionResult(executionResult.StatusCode, null);
                    }

                    if (usePipe)
                    {
                        lastStdout = executionResult.Stdout.Buffer;
                    }
                }

                return new ExecutionResult(ShellResult.Success, null);
            }

            string[] parts = SplitArgs(input.Trim());
            if (parts.Length == 0)
            {
                return new ExecutionResult(ShellResult.Empty, null);
            }

            var commandName = parts[0];

            // TODO: Support "things like this to be one argument"
            var args = parts.Skip(1).ToArray();

            if (FindBuiltIn(commandName, out BuiltIn command))
            {
                OutputBuffer stdout;
                ExitCode status;

                if (pipe)
                {
                    stdout = new PipedBuffer();
                }
                else
                {
                    stdout = new ConsoleBuffer();
                }

                status = command.Invoke(args, stdin, stdout);

                return status == ExitCode.Success
                    ? new ExecutionResult(ShellResult.Success, stdout)
                    : new ExecutionResult(ShellResult.Failure, stdout);
            }

            return new ExecutionResult(ShellResult.NotFound, null);
        }

        /// <summary>
        /// This one is connected with Sputnik to generate a command based on the user's description.
        /// </summary>
        private void SmartCommand(string prompt)
        {
            string cwd = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);
            string listing = string.Empty;

            List<Cosmos.System.FileSystem.Listing.DirectoryEntry> list = Kernel.FileSystem.GetDirectoryListing(cwd);

            foreach (var item in list)
            {
                listing += item.mSize + ";";
            }

            string systemPrompt = "System prompt: Your task is to generate a valid command based on the user's description. Only respond with your generated command. If you can't generate a fitting command, retun \"unknown\". " +
                "Never output the description to the user. System paths look like this: '0:\\Sys\\'\nExample output: \"ping google.com\" or \"cd Homework\"\n" +
                $"Available commands are listed below in a man page like format. Each command is seperated by \"---\":\n{UsageText}\n\n--- END OF COMMANDS---\n" +
                $"The current working directory is:{cwd}\nThese are the contents of the current working directory seperated by semicolons:\n{listing}\nThis is the request by the user: {prompt}";

            Kernel.PrintDebug(systemPrompt);
            string cmd = Sputnik.QuickPrompt(systemPrompt);

            if (cmd == null)
            {
                return;
            }

            Console.Write($"Sputnik suggests:\n $ ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(cmd);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nType ENTER to accept and execute, or ANY KEY to reject: ");
            ConsoleKeyInfo key = Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            if (key.Key == ConsoleKey.Enter)
            {
                Execute(cmd);
            }

        }

        private void PrintPrefix()
        {
            string cwd = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(cwd);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" $ ");
        }

        private string GetPrefix()
        {
            string cwd = Kernel.GlobalEnvironment.GetFirst(DefaultEnvironments.CurrentWorkingDirectory);
            string ret = "";
            ret += cwd;
            ret += " $ ";

            return ret;
        }
    }
}
