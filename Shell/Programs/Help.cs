using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    public class Help : BuiltIn
    {
        public override string Name => "help";

        public override string Description => "Prints this list.";

        public override CommandDescription ArgumentDescription => new()
        /*{
            Arguments = [
                new(
                    valueName: "command",
                    description: "commandify yourself",
                    argsPosition: 0,
                    valueDefault: "null",
                    type: typeof(string))
            ]
        }*/;

        protected override ExitCode Execute()
        {

            Kernel.PrintDebug("Help command executed");
            int longestName = "Command".Length;
            /*string command = (string)GetArgument(0);
            if(command == "love")
            {
                WriteLine(
                    @"   please love yourself <3

   _______________                        |*\_/*|________
  |  ___________  |     .-.     .-.      ||_/-\_|______  |
  | |           | |    .****. .****.     | |           | |
  | |   0   0   | |    .*****.*****.     | |   0   0   | |
  | |     -     | |     .*********.      | |     -     | |
  | |   \___/   | |      .*******.       | |   \___/   | |
  | |___     ___| |       .*****.        | |___________| |
  |_____|\_/|_____|        .***.         |_______________|
    _|__|/ \|_|_.............*.............._|________|_
   / ********** \                          / ********** \
 /  ************  \                      /  ************  \
--------------------                    --------------------");
                return ExitCode.Success;
            }*/
            foreach (BuiltIn builtIn in Sokolsh.AvailableBuiltIns)
            {
                if (builtIn.Name.Length > longestName)
                {
                    longestName = builtIn.Name.Length;
                }
            }

            // Apply some padding
            longestName += 2;

            int descriptionSize = Console.WindowWidth - longestName;
            string[] cmds = new string[Sokolsh.AvailableBuiltIns.Count + 1];
            for (int i = 0; i < cmds.Length - 1; i++)
            {
                BuiltIn cbi = Sokolsh.AvailableBuiltIns[i];
                cmds[i] = new string(cbi.Name.Pad(longestName) + cbi.Description.Pad(descriptionSize));
            }
            cmds[cmds.Length - 1] = new string("exit".Pad(longestName) + "Exits the current shell.".Pad(descriptionSize));

            Write("Command".Pad(longestName) + "Description".Pad(descriptionSize));

            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Write("-");
            }

            int cI = 0;
            bool r = true;
            Kernel.PrintDebug("Press Enter to exit help");

            while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Enter)
            {


                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;

                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            if (cI > 0)
                            {
                                cI--;
                                r = true;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (!(cI + Console.WindowHeight > cmds.Length))
                            {
                                cI++;
                                r = true;
                            }
                            break;
                    }
                }
                if (r)
                {
                    Kernel.PrintDebug("cI: " + cI);
                    r = false;
                    Console.Clear();
                    for (int i = cI; i < Console.WindowHeight + cI - 1; i++)
                    {
                        //Kernel.PrintDebug($"i: {i} @ {cmds[i]}");
                        Write(cmds[i]);
                    }
                }

            }

            return ExitCode.Success;
        }
    }
}
