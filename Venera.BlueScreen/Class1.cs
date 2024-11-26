using IL2CPU.API.Attribs;
using System.Xml.Linq;
using static Cosmos.Core.INTs;

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

        PutErrorString(12, Console.WindowWidth /2 - ((6 + name.Length)/2), "Name: " + name);
        PutErrorString(13, Console.WindowWidth / 2 - ((6 + desc.Length) / 2), "Description: " + desc);
        PutErrorString(14, Console.WindowWidth / 2 - 33, "Please visit https://ebsksjk.gay/STOPCODE for further information.");

        Console.CursorVisible = false;
        while (true)
        {
            PutErrorString(Console.WindowHeight - 1, Console.WindowWidth - 4, "   ");
            PutErrorString(Console.WindowHeight - 1, Console.WindowWidth - 4, "OwO");
            int x = 0;
            for(int i = 0; i < 1000000; i++)
            {
                x += Console.WindowHeight - i;
            }
            PutErrorString(Console.WindowHeight - 1, Console.WindowWidth - 4, ">w<");
            int y = 0;
            for (int i = 0; i < 1000000; i++)
            {
                y += Console.WindowHeight - i;
            }
        }
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
}