﻿using System;

namespace Venera.Shell.Programs
{
    public class Echo : BuiltIn
    {
        public override string Name => "echo";

        public override string Description => "Prints the args";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "text",
                    description: "String to output",
                    type: typeof(string[]),
                    argsPosition: -1,
                    required: true
                )
            ]
        };

        protected override ExitCode Execute()
        {
            string ret = "";

            foreach (string i in (string[])GetArgument(-1))
            {
                ret += i + ' ';
            }

            WriteLine(ret);

            return ExitCode.Success;
        }
    }
}
