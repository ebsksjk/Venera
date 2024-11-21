using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Venera.Shell
{
    public abstract class BuiltIn
    {
        #region Properties

        /// <summary>
        /// Specifies the which is used to invoke this built-in command.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Small description used to display along side the name in the help command.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Holds our current parsed command line arguments. Only set during execution.
        /// </summary>
        protected string[] Args { get; set; }

        /// <summary>
        /// This argument description is used to generate a useful help text for the user,
        /// as well as to generate full man pages and feed into Sputnik.
        /// </summary>
        public abstract CommandDescription ArgumentDescription { get; }

        #endregion

        #region Public methods

        /// <summary>
        /// Invoke execution of this command.
        /// </summary>
        /// <param name="args">Parsed command line with all arguments.</param>
        /// <returns>Exit code of the invoked command.</returns>
        public ExitCode Invoke(string[] args)
        {
            Kernel.PrintDebug($"Invoke {Name} ...");
            Args = args;

            // TODO: Future error handling prior to execution.

            ExitCode exitCode;

            try
            {
                exitCode = Execute();
            }
            catch (Exception ex)
            {
                Kernel.PrintDebug($"Something is wrong, I can feel it: {ex.Message}");
                exitCode = ExitCode.Error;
            }

            // TODO: Future code after execution.
            args = null;

            return exitCode;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// This is the actual logic behind this command. It cannot be directly called from the outside since
        /// <see cref="Invoke(string[])"/> has to do some default actions prior and after this function is called.
        /// </summary>
        protected abstract ExitCode Execute();

        protected string GetApplicationStorage()
        {
            return "";
        }

        /// <summary>
        /// Try to extract any given argument out of the command line.
        /// </summary>
        /// <param name="argsName">Long or short form of the target argument. <b>Do not use - or -- prefix.</b></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">May be thrown if <paramref name="argsName"/> cannot be found within the <see cref="ArgumentDescription"/>.</exception>
        protected object GetArgument(string argsName)
        {
            Kernel.PrintDebug($"Get argument {argsName}");

            // Check if this command even has arguments defined.
            if (!ArgumentDescription.HasArguments)
            {
                return null;
            }

            CommandArgument cmdArg = ArgumentDescription.Arguments.First(x => x.LongForm == argsName || x.ShortForm == (argsName.Length == 1 ? argsName[0] : ' '));

            // Make sure the queried argument is defined.
            if (cmdArg == null)
            {
                throw new ArgumentException($"Argument \"{argsName}\" has not been defined in the argument description by command {Name}.");
            }

            // Try to locate the target argumnt inside the argument list.
            int location = IndexOfArgument(cmdArg);

            // If our target argument is type of boolean, then return true if it exists, false if not.
            if (cmdArg.Type == typeof(bool))
            {
                return location >= 0;
            }

            // At this point we don't deal with booleans but rather with strings or integers.
            // So if our target argument doesn't exist, we return null.
            if (location == -1)
            {
                return GetDefaultArgument(cmdArg);
            }

            // Check if there is no more value after our argument but we expect one to exist.
            // myapp -v --output-path [MISSING HERE]
            if ((Args.Length - 1) == location)
            {
                return null;
            }

            string argValue = Args[location + 1];

            // A argument value may not start with another - symbol.
            if (argValue.StartsWith("-"))
            {
                return null;
            }

            // Handle integers
            if (cmdArg.Type == typeof(int))
            {
                if (!int.TryParse(argValue, out int argInt))
                {
                    Console.WriteLine($"Sokolsh: Expected integer for argument {cmdArg} but got \"{argValue}\" instead.");
                    return null;
                }

                return argInt;
            }

            return argValue;
        }

        protected object GetArgument(int argsIndex)
        {
            Kernel.PrintDebug($"Get argument at {argsIndex}");

            foreach (var arg in ArgumentDescription.Arguments)
            {
                Kernel.PrintDebug($"Arg {arg.ArgsPosition}: {arg.ValueName}");
            }

            //CommandArgument cmdArg = ArgumentDescription.Arguments.ToList().FindAll(x => x.ArgsPosition == argsIndex).First();
            CommandArgument cmdArg = ArgumentDescription.Arguments.First(x => x.ArgsPosition == argsIndex);

            // Make sure the queried argument is defined.
            if (cmdArg == null)
            {
                Kernel.PrintDebug("Hell nah, found nothing");
                throw new ArgumentException($"Argument at position \"{argsIndex}\" has not been defined in the argument description by command {Name}.");
            }
            Kernel.PrintDebug($"Identified argument {cmdArg.ValueName}");

            // Try to locate the target argument inside the argument list.
            int location = IndexOfArgument(cmdArg);

            Kernel.PrintDebug($"Found location: {location}");

            // At this point we don't deal with booleans but rather with strings or integers.
            // So if our target argument doesn't exist, we return null.
            if (location == -1)
            {
                if (cmdArg.ValueDefault != null)
                {
                    if (cmdArg.Type == typeof(int))
                    {
                        return int.Parse(cmdArg.ValueDefault);
                    }

                    return cmdArg.ValueDefault;
                }

                return null;
            }

            if (cmdArg.ArgsPosition == -1)
            {
                return Args.Skip(location).ToArray();
            }

            string argValue = Args[location];

            // Handle integers
            if (cmdArg.Type == typeof(int))
            {
                if (!int.TryParse(argValue, out int argInt))
                {
                    Console.WriteLine($"Sokolsh: Expected integer for argument {cmdArg} but got \"{argValue}\" instead.");
                    return null;
                }

                return argInt;
            }

            return argValue;
        }

        public string GenerateUsage()
        {
            // A command has commands if there are possible arguments that aren't required.
            CommandArgument[] options = ArgumentDescription.Arguments.Where(x =>
                x.ShortForm != CommandArgument.ShortFormDefault || x.LongForm != null
            ).ToArray();

            CommandArgument[] mandatoryArguments = ArgumentDescription.Arguments.Where(x =>
                x.Required // && x.ArgsPosition == null
            ).ToArray();

            // Nested function because I need to do this at least twice and only inside this function.
            string GenerateLeftHand(CommandArgument cmd)
            {
                string leftForms = "  ";
                if (cmd.ShortForm != CommandArgument.ShortFormDefault && cmd.LongForm != null)
                {
                    leftForms += $"-{cmd.ShortForm}, --{cmd.LongForm}";
                }
                else if (cmd.ShortForm != CommandArgument.ShortFormDefault && cmd.LongForm == null)
                {
                    leftForms += $"-{cmd.ShortForm}";
                }
                else if (cmd.ShortForm == CommandArgument.ShortFormDefault && cmd.LongForm != null)
                {
                    leftForms += $"--{cmd.LongForm}";
                }

                return leftForms;
            }

            string result = $"{Description}\n\nUsage: " +
                $"{Name} " +
                $"{(options.Length > 0 ? "[OPTIONS]" : "")} " +
                $"{(mandatoryArguments.Length > 0 ? string.Join(" ", mandatoryArguments.Select(x => x.ToString())) : "")}";

            if (options.Length > 0)
            {
                result += "\n\nOptions:\n";

                // Calculate max length of the left side to get the required padding.
                int padding = 0;
                foreach (CommandArgument cmd in options)
                {
                    int length = GenerateLeftHand(cmd).Length;

                    if (length > padding)
                    {
                        padding = length;
                    }
                }

                // Add some more padding for better readability.
                padding += 2;

                // Print it for real.
                foreach (CommandArgument cmd in options)
                {
                    result += $"{GenerateLeftHand(cmd).Pad(padding)}{cmd.Description}\n";
                }
            }

            return result;
        }


        #endregion

        private object GetDefaultArgument(CommandArgument cmdArg)
        {
            if (cmdArg.ValueDefault != null)
            {
                if (cmdArg.Type == typeof(int))
                {
                    return int.Parse(cmdArg.ValueDefault);
                }

                return cmdArg.ValueDefault;
            }

            return null;
        }

        /// <summary>
        /// Private helper function to find any argument within our argument list.
        /// </summary>
        /// <param name="arg">Target defined argument</param>
        /// <returns>Index of argument on the command line. <b>-1 if not found.</b></returns>
        private int IndexOfArgument(CommandArgument arg)
        {
            /*
             * If our target argument is expected at a indexed position.
             * 
             * Example: ssh -v -i .ssh/id_ed25519 user@example.com -p 1337
             *    Args: -v -> true
             *          -i -> .ssh/id_ed25519
             *          [0]-> user@example.com  // That's what we want.
             *          -p -> 1337 (int)
             */
            if (arg.ArgsPosition != int.MinValue)
            {
                /*
                 * This one is kind of special. This is a "endless" argument at the end of the
                 * command and can be endlessly long.
                 * 
                 * Example: echo -n This is a very long text which is actually just one argument.
                 *    Args: -n        -> true
                 *          [...args] -> "This is a very long text which is actually just one argument."
                 */
                if (arg.ArgsPosition == -1)
                {
                    Kernel.PrintDebug("Reverse search ...");

                    // i > 0 because we don't want the program name.
                    for (int i = Args.Length - 1; i > 0; i--)
                    {
                        if (Args[i].StartsWith("-"))
                        {
                            // We stompled opon a argument with -- or - so we take a step back and return
                            // the index where our endless argument starts.
                            return i + 1;
                        }
                    }

                    // If we searched from the back and didn't hit anything "-"ish, then we can assume that
                    // the second element is the starting point of our argument.
                    // Example: echo This is a bunch of text.
                    return 0;
                }

                // This is essentially our counter how many non --xyz or -x arguments we spotted.
                // If this counter == arg.ArgsPosition, then we found our target.
                int nonQuoteArgs = 0;
                for (int i = 0; i < Args.Length; i++)
                {
                    string sArg = Args[i];

                    CommandArgument tmpArg = null;

                    if (sArg.StartsWith("-"))
                    {
                        // For arguments like --output, --listen
                        if (sArg.StartsWith("--"))
                        {
                            // Find defined argument
                            tmpArg = ArgumentDescription.Arguments.First(x => x.LongForm == sArg.Substring(3));
                        }
                        else
                        {
                            // Find defined argument
                            tmpArg = ArgumentDescription.Arguments.First(x => x.ShortForm == sArg[1]);
                        }

                        if (tmpArg == null)
                        {
                            throw new Exception($"Spotted undefined argument {sArg} for {Name}.");
                        }

                        if (tmpArg.Type == typeof(bool))
                        {
                            // Just advance as normal though the array and check next argument.
                            continue;
                        }

                        // We skip one more ahead because the next argument must be a value.
                        i++;
                    }
                    else
                    {
                        if (arg.ArgsPosition == nonQuoteArgs)
                        {
                            return i;
                        }

                        // Doesn't start with -- or - so it must be a indexed argument.
                        nonQuoteArgs++;
                    }
                }

                return -1;
            }

            // Find non-indexed argument with -- or -
            for (int i = 0; i < Args.Length; i++)
            {
                string sArg = Args[i];

                // For arguments like --output, --listen
                if (sArg.StartsWith("--"))
                {
                    if (arg.LongForm == sArg.Substring(3))
                    {
                        return i;
                    }
                }

                // For arguments like -o, -S
                if (sArg.StartsWith("-") && sArg.Length == 2)
                {
                    if (arg.ShortForm == sArg[1])
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
