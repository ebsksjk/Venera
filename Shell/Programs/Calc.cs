using System;
using System.IO;
using Venera.stasi;

namespace Venera.Shell.Programs
{
    public class Calc : BuiltIn
    {
        public override string Name => "calc";

        public override string Description => "The world's most expensive and inaccurate calculator. Powered by AI.";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "text",
                    description: "Mathematical notation or natural language.",
                    type: typeof(string[]),
                    required: true,
                    argsPosition: -1
                )
            ]
        };

        private static bool firstRun = true;

        private string lastPrompt;

        protected override ExitCode Execute()
        {
            string mathTerm = "";

            foreach (string i in (string[])GetArgument(-1))
            {
                mathTerm += i + ' ';
            }

            if (firstRun)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Write("Note on first run: ");
                Console.ForegroundColor = ConsoleColor.White;
                WriteLine("LLMs are good for language, not for algebra. Results may look good or be close to the real result without being correct. LLMs hallucinate.\n");
                firstRun = false;
            }

            string response = Sputnik.QuickPrompt(mathTerm, Sputnik.TalkingStyle.Calc);

            if (response == null)
            {
                return ExitCode.Error;
            }

            WriteLine(response);

            return ExitCode.Success;
        }
    }
}
