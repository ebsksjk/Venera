using System;

namespace Venera.Shell.Programs
{
    public class ArgTest : BuiltIn
    {
        public override string Name => "argtest";

        public override string Description => "(Test only) Has some weird arguments to test our command parsing.";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "host",
                    description: "Remote host to connect to. Accepts either a IP address or hostname.",
                    type: typeof(string),
                    required: true,
                    argsPosition: 0
                ),
                new(
                    valueName: "port",
                    description: "Remote port to connect to.",
                    type: typeof(int),
                    shortForm: 'p',
                    longForm: "port",
                    valueDefault: "9000"
                ),
                new(
                    valueName: "log_file",
                    description: "Specify a log file path.",
                    type: typeof(string),
                    shortForm: 'L',
                    longForm: "log-file",
                    valueDefault: "0:\\Venera\\dummy.log"
                ),
                new(
                    valueName: "human_readable",
                    description: "Format output to be human-readable.",
                    type: typeof(bool),
                    shortForm: 'h',
                    valueDefault: "true"
                ),
                new(
                    valueName: "yes",
                    description: "You must give explicit consent to run this command.",
                    type: typeof(bool),
                    longForm: "consent",
                    required: true
                )
            ]
        };

        protected override ExitCode Execute()
        {
            Console.WriteLine($"Parsed command line arguments:");
            Console.WriteLine($"host: {(string)GetArgument(0)}");
            Console.WriteLine($"port: {(int)GetArgument("port")} | {(int)GetArgument("p")}");
            Console.WriteLine($"log-file: {(string)GetArgument("log-file")} | {(string)GetArgument("L")}");
            Console.WriteLine($"humanreadable: {((bool)GetArgument("h") ? "true" : "false")}");
            Console.WriteLine($"consent: {((bool)GetArgument("consent") ? "true" : "false")}");
            return ExitCode.Success;
        }
    }
}
