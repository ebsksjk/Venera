using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Venera.VoPo
{
    internal class faketop : Shell.BuiltIn
    {
    public override string Name => "faketop";

    public override string Description => "get all runned programms with info";

        public override ExitCode Execute(string[] args)
        {
            string processes = File.ReadAllText("0:\\Sys\\PT");
            List<string> processList = processes.Split("\n").ToList();

            if (processList.Count == 0)
            {
                Console.WriteLine("No processes were running");
                return ExitCode.Success;
            }

            foreach (string process in processList)
            {
                List<string> processInfo = process.Split(" ").ToList();
                Console.WriteLine($"PID: {processInfo[0]} | Name: {processInfo[1]} | entrypoint: {processInfo[2]} | path: {processInfo[3]} | start: {processInfo[4]}");
            }

            return ExitCode.Success;
        }
     }
}
