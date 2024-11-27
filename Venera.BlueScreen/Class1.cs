using Cosmos.System;
using IL2CPU.API.Attribs;
using static Cosmos.Core.INTs;
using Console = System.Console;

[Plug(Target = typeof(Cosmos.Core.INTs))]
internal class INTs
{
    static string[] qr2 = new string[]
{
    "█▀▀▀▀▀█ ▀▀    ▄ █ █▀▀▀▀▀█",
    "█ ███ █ █▄ █████  █ ███ █",
    "█ ▀▀▀ █ ▀█▀▀▄█ ▀█ █ ▀▀▀ █",
    "▀▀▀▀▀▀▀ ▀▄▀▄█ ▀▄█ ▀▀▀▀▀▀▀",
    "█▀▀▀▄ ▀ ▀▄   ▄ ▀▀▀ ▄▀▀▀▄▀",
    "▀ █▄ ▀▀▀▀  ▀▄█▄▄█▀▄▀▀▄▄",
    "▀█▄▄▄▀▀▀ ▀▄▄█ ▀▄▄▀█▄█ ▀▀█",
    "▀▄▄▀ ▀▀ ███▄▀ ▄ ▀▄ ▄█ ▀▀▄",
    "▀▀▀  ▀▀ █▀  ▄▄▄▀█▀▀▀█▀█▀█",
    "█▀▀▀▀▀█  ▀▀   █▄█ ▀ █ ▀█▀",
    "█ ███ █ ▄█   █▀ ███▀██▄██",
    "█ ▀▀▀ █ ██ ▀ █▄▄ ▀█ █ █▀",
    "▀▀▀▀▀▀▀ ▀▀▀   ▀  ▀ ▀▀▀▀▀▀"
};
    public static void HandleException(uint eip, string desc, string name, ref IRQContext context, uint lastKnownAddressValue = 0)
    {
        ClearScreen();
        for(int i = 0; i < qr2.Length; i++)
        {
            PutErrorString(i, 0, qr2[i].Replace('█', (char)0xDB).Replace('▄', (char)0xDC).Replace('▀', (char)0xDF));
        }

        int qr_len = qr2[0].Length + 5;
        PutErrorString(3, qr_len, ":( OOPSIE WOOPSIE!!");
        PutErrorString(4, qr_len, "Venera has crashed unexpectedly.");

        PutErrorString(6, qr_len, "UwU We made a fucky wucky!!");
        PutErrorString(7, qr_len, "A wittle fucko boingo!");
        PutErrorString(8, qr_len, "The code furrys are working VEWY HAWD to fix it!");

        PutErrorString(9, qr_len, "Name: " + name);
        PutErrorString(10, qr_len, "Description: " + desc);
        PutErrorString(14, Console.WindowWidth / 2 - 33, "Please visit https://ebsksjk.gay/STOPCODE for further information.");

        Console.CursorVisible = false;
        int x = 0;
        while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Enter)
        {
            if (x < 1000)
            {
                PutErrorString(Console.WindowHeight - 1, Console.WindowWidth - 4, "OwO");
            } else if(x > 1000 && x < 2000)
            {
                PutErrorString(Console.WindowHeight - 1, Console.WindowWidth - 4, "owo");
            }

            if(x < 500)
            {
                PutErrorString(Console.WindowHeight - 5, Console.WindowWidth / 2 - 5, "| 0%");
            } else if(x > 500 && x < 1000)
            {
                PutErrorString(Console.WindowHeight - 5, Console.WindowWidth / 2 - 5, "/ 0%");
            }
            else if (x > 1000 && x < 1500)
            {
                PutErrorString(Console.WindowHeight - 5, Console.WindowWidth / 2 - 5, "- 0%");
            }
            else if (x > 1500 && x < 2000)
            {
                PutErrorString(Console.WindowHeight - 5, Console.WindowWidth / 2 - 5, "\\ 0%");
            }

            x++;
            if(x > 2000)
            {
                x = 0;
            }
        }
        Power.Reboot();
    }

    private static void PutErrorChar(int line, int col, char c, int color)
    {
        unsafe
        {
            byte* xAddress = (byte*)0xB8000;

            xAddress += (line * 80 + col) * 2;

            xAddress[0] = (byte)c;
            xAddress[1] = (byte)color;
        }
    }

    private static void PutErrorString(int line, int startCol, string error, int color=0x1F)
    {
        for (int i = 0; i < error.Length; i++)
        {
            PutErrorChar(line, startCol + i, error[i], color);
        }
    }

    private static void ClearScreen()
    {
        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 25; j++)
            {
                PutErrorChar(j, i, ' ', 0x1F);
            }
        }
    }

    public static void HandleInterrupt_80(ref IRQContext aContext)
    {
        Console.WriteLine("Interrupt 0x80 called");
        if (aContext.Interrupt == 0x80) // Example system call interrupt
        {
            uint syscallNumber = aContext.Interrupt;
            //Syscalls.HandleSyscall(syscallNumber, args.Skip(1).ToArray());
            Kernel.PrintDebug("handleInterrupt Syscall " + syscallNumber + " called");
        }
        else
        {
            // Handle other interrupts
        }
    }
}