using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell;

namespace Venera.VoPo
{
    internal class Faketop : BuiltIn
    {
        public override string Name => "faketop";

        public override string Description => "get all runned programs with info";

        public override CommandDescription ArgumentDescription => new();

        protected override ExitCode Execute()
        {
            string processes = File.ReadAllText("0:\\Venera\\Sys\\PT");
            List<string> processList = processes.Split("\n").ToList();

            if (processList.Count == 0)
            {
                Console.WriteLine("No processes were running");
                return ExitCode.Success;
            }

            foreach (string process in processList)
            {
                if (string.IsNullOrWhiteSpace(process)) continue;

                List<string> processInfo = process.Split(" ").ToList();
                Console.WriteLine($"PID: {processInfo[0]} | Name: {processInfo[1]} | entrypoint: {processInfo[2]} | path: {processInfo[3]} | start: {processInfo[4]}");
            }

            return ExitCode.Success;
        }
    }
}
