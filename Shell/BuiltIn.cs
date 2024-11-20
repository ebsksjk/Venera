using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Args = args;

            // TODO: Future error handling prior to execution.
            ExitCode exitCode = Execute();

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
            CommandArgument cmdArg = ArgumentDescription.Arguments.First(x => x.LongForm == argsName || x.ShortForm == (argsName.Length == 1 ? argsName[0] : ' '));

            if (cmdArg == null)
            {
                throw new ArgumentException($"Argument \"{argsName}\" has not been defined in the argument description by command {Name}.");
            }

            if (cmdArg.Type == typeof(bool))
            {
                if (Args.Any(x => x.))
            }
        }

        protected string GenerateUsage()
        {
            // An option is a argument which is not mandatory and has a short or long form.
            bool hasOptions = ArgumentDescription.Arguments.Any(x =>
                !x.Required && (x.ShortForm != null || x.LongForm != null)
            );

            CommandArgument[] mandatoryArguments = ArgumentDescription.Arguments.Select(x =>
                x.Required
            ).ToArray();

            string result = $"Usage: {Name} {(hasOptions ? "[OPTIONS]" : "")}";

            return result;
        }

        #endregion
    }
}
