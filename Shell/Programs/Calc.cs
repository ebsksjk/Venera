using System;

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

        private string lastPrompt;

        protected override ExitCode Execute()
        {
            string mathTerm = "";

            foreach (string i in (string[])GetArgument(-1))
            {
                mathTerm += i + ' ';
            }

            string response = Sputnik.QuickPrompt(mathTerm, Sputnik.TalkingStyle.Calc);
            Console.WriteLine(response);

            return ExitCode.Success;
        }
    }
}
