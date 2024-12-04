using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell.Programs
{
    class froeh : BuiltIn
    {
        public override string Name => "froeh";

    public override string Description => "froeht itself";

    public override CommandDescription ArgumentDescription => new();

    protected override ExitCode Execute()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        for(int i = 0; i < Console.WindowHeight; i++)
            {
                for(int j = 0; j < Console.WindowWidth; j++)
                {
                    if(j % 2 == 0) Write((char)0x01);
                    else Write((char)0x02);
                }
        }

        return ExitCode.Success;
    }
}
}
