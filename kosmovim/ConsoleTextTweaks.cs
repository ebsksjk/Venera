using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venera.Shell.Programs;

namespace Venera.Kosmovim
{
    public class ConsoleTextTweaks
    {
        public static void PutChar(int line, int col, byte c, int color = 0x1F)
        {
            unsafe
            {
                byte* xAddress = (byte*)0xB8000;

                xAddress += (line * 80 + col) * 2;

                xAddress[0] = c;
                xAddress[1] = (byte)color;
            }
        }

        public static byte GetScreenChar(uint x, uint y)
        {
            byte ret = 0x0;

            // Bounds check: x should be in range [0, 79] and y should be in range [0, 24]
            if (x > 80 || y > 25)
            {
                return ret;
            }

            unsafe
            {
                byte* screenBuffer = (byte*)0xB8000;

                // Each character cell occupies 2 bytes, so we multiply by 2
                uint offset = (y * 80 + x) * 2;

                // The character byte is at the even index (first byte in the pair)
                ret = screenBuffer[offset];
            }
            //Kernel.PrintDebug($"getScreenChar({x}, {y}) = {ret} {(char)ret}");

            return ret;
        }

        public static void PutString(int line, int startCol, string msg, int color = 0x1F, int wwidth = 80, int wheight = 25)
        {
            int wX = startCol;
            int wY = line;
            for (int i = 0; i < msg.Length; i++)
            {
                if (wX >= wwidth)
                {
                    wX = 1;
                    wY++;
                }
                if (wY >= wheight)
                {
                    return; //not implemented yet :) but eventually, we do need to scroll...
                }
                PutChar(wY, wX, (byte)msg[i], color);
                wX++;
            }
        }

        public static void PutCharAtCursor(char c, int color = 0x1F)
        {
            (int x, int y) pos = Console.GetCursorPosition();

            PutChar(pos.x + 1, pos.y + 1, (byte)c, color);
            Console.SetCursorPosition(pos.x - 1, pos.y);
        }

        public static void ClearScreen(int color = 0x1F)
        {
            for (int i = 0; i < 80; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    PutChar(j, i, (byte)' ', color);
                }
            }
        }

        public static string GetConsoleString(bool password = false)
        {
            string ret = "";
            while (true)
            {
                // Check for any input
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true); // Non-blocking key read

                    // Handle Enter key (submit action)
                    if (key.Key == ConsoleKey.Enter)
                    {
                        return ret;
                    }
                    // Handle Backspace key (remove last character)
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (ret.Length > 0)
                        {
                            ret = ret.Substring(0, ret.Length - 1);
                            int x, y;
                            (x, y) = Console.GetCursorPosition();
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write(' ');
                            Console.SetCursorPosition(x - 1, y);
                        }
                    }
                    // Handle other keys (append character to 'ret')
                    else
                    {
                        ret += key.KeyChar;
                        if (password)
                        {
                            Console.Write("*"); // Mask character if it's a password
                        }
                        else
                        {
                            Console.Write(key.KeyChar); // Otherwise, print the actual character
                        }
                    }
                }

                Sparkle();
            }
        }

        //ich bin nicht stolz auf diese abnormnimation.
        //aber, quelle: https://en.wikipedia.org/wiki/Code_page_437
        public static byte GetAsciiFromChar(char x)
        {
            #region Abomination of switch-case

            #region DO NOT OPEN

            switch (x)
            {
                /*case '\0': return 0;*/
                case '☺': return 1;
                case '☻': return 2;
                case '♥': return 3;
                case '♦': return 4;
                case '♣': return 5;
                case '♠': return 6;
                case '•': return 7;
                case '◘': return 8;
                case '○': return 9;
                case '◙': return 10;
                case '♂': return 11;
                case '♀': return 12;
                case '♪': return 13;
                case '♫': return 14;
                case '☼': return 15;
                case '►': return 16;
                case '◄': return 17;
                case '↕': return 18;
                case '‼': return 19;
                case '¶': return 20;
                case '§': return 21;
                case '▬': return 22;
                case '↨': return 23;
                case '↑': return 24;
                case '↓': return 25;
                case '→': return 26;
                case '←': return 27;
                case '∟': return 28;
                case '↔': return 29;
                case '▲': return 30;
                case '▼': return 31;
                case ' ': return 32;
                case '!': return 33;
                case '"': return 34;
                case '#': return 35;
                case '$': return 36;
                case '%': return 37;
                case '&': return 38;
                case '\'': return 39;
                case '(': return 40;
                case ')': return 41;
                case '*': return 42;
                case '+': return 43;
                case ',': return 44;
                case '-': return 45;
                case '.': return 46;
                case '/': return 47;
                case '0': return 48;
                case '1': return 49;
                case '2': return 50;
                case '3': return 51;
                case '4': return 52;
                case '5': return 53;
                case '6': return 54;
                case '7': return 55;
                case '8': return 56;
                case '9': return 57;
                case ':': return 58;
                case ';': return 59;
                case '<': return 60;
                case '=': return 61;
                case '>': return 62;
                case '?': return 63;
                case '@': return 64;
                case 'A': return 65;
                case 'B': return 66;
                case 'C': return 67;
                case 'D': return 68;
                case 'E': return 69;
                case 'F': return 70;
                case 'G': return 71;
                case 'H': return 72;
                case 'I': return 73;
                case 'J': return 74;
                case 'K': return 75;
                case 'L': return 76;
                case 'M': return 77;
                case 'N': return 78;
                case 'O': return 79;
                case 'P': return 80;
                case 'Q': return 81;
                case 'R': return 82;
                case 'S': return 83;
                case 'T': return 84;
                case 'U': return 85;
                case 'V': return 86;
                case 'W': return 87;
                case 'X': return 88;
                case 'Y': return 89;
                case 'Z': return 90;
                case '[': return 91;
                case '\\': return 92;
                case ']': return 93;
                case '^': return 94;
                case '_': return 95;
                case '`': return 96;
                case 'a': return 97;
                case 'b': return 98;
                case 'c': return 99;
                case 'd': return 100;
                case 'e': return 101;
                case 'f': return 102;
                case 'g': return 103;
                case 'h': return 104;
                case 'i': return 105;
                case 'j': return 106;
                case 'k': return 107;
                case 'l': return 108;
                case 'm': return 109;
                case 'n': return 110;
                case 'o': return 111;
                case 'p': return 112;
                case 'q': return 113;
                case 'r': return 114;
                case 's': return 115;
                case 't': return 116;
                case 'u': return 117;
                case 'v': return 118;
                case 'w': return 119;
                case 'x': return 120;
                case 'y': return 121;
                case 'z': return 122;
                case '{': return 123;
                case '|': return 124;
                case '}': return 125;
                case '~': return 126;
                case '⌂': return 127;
                case 'Ç': return 128;
                case 'ü': return 129;
                case 'é': return 130;
                case 'â': return 131;
                case 'ä': return 132;
                case 'à': return 133;
                case 'å': return 134;
                case 'ç': return 135;
                case 'ê': return 136;
                case 'ë': return 137;
                case 'è': return 138;
                case 'ï': return 139;
                case 'î': return 140;
                case 'ì': return 141;
                case 'Ä': return 142;
                case 'Å': return 143;
                case 'É': return 144;
                case 'æ': return 145;
                case 'Æ': return 146;
                case 'ô': return 147;
                case 'ö': return 148;
                case 'ò': return 149;
                case 'û': return 150;
                case 'ù': return 151;
                case 'ÿ': return 152;
                case 'Ö': return 153;
                case 'Ü': return 154;
                case '¢': return 155;
                case '£': return 156;
                case '¥': return 157;
                case '₧': return 158;
                case 'ƒ': return 159;
                case 'á': return 160;
                case 'í': return 161;
                case 'ó': return 162;
                case 'ú': return 163;
                case 'ñ': return 164;
                case 'Ñ': return 165;
                case 'ª': return 166;
                case 'º': return 167;
                case '¿': return 168;
                case '⌐': return 169;
                case '¬': return 170;
                case '½': return 171;
                case '¼': return 172;
                case '¡': return 173;
                case '«': return 174;
                case '»': return 175;
                case '░': return 176;
                case '▒': return 177;
                case '▓': return 178;
                case '│': return 179;
                case '┤': return 180;
                case '╡': return 181;
                case '╢': return 182;
                case '╖': return 183;
                case '╕': return 184;
                case '╣': return 185;
                case '║': return 186;
                case '╗': return 187;
                case '╝': return 188;
                case '╜': return 189;
                case '╛': return 190;
                case '┐': return 191;
                case '└': return 192;
                case '┴': return 193;
                case '┬': return 194;
                case '├': return 195;
                case '─': return 196;
                case '┼': return 197;
                case '╞': return 198;
                case '╟': return 199;
                case '╚': return 200;
                case '╔': return 201;
                case '╩': return 202;
                case '╦': return 203;
                case '╠': return 204;
                case '═': return 205;
                case '╬': return 206;
                case '╧': return 207;
                case '╨': return 208;
                case '╤': return 209;
                case '╥': return 210;
                case '╙': return 211;
                case '╘': return 212;
                case '╒': return 213;
                case '╓': return 214;
                case '╫': return 215;
                case '╪': return 216;
                case '┘': return 217;
                case '┌': return 218;
                case '█': return 219;
                case '▄': return 220;
                case '▌': return 221;
                case '▐': return 222;
                case '▀': return 223;
                case 'α': return 224;
                case 'ß': return 225;
                case 'Γ': return 226;
                case 'π': return 227;
                case 'Σ': return 228;
                case 'σ': return 229;
                case 'µ': return 230;
                case 'τ': return 231;
                case 'Φ': return 232;
                case 'Θ': return 233;
                case 'Ω': return 234;
                case 'δ': return 235;
                case '∞': return 236;
                case 'φ': return 237;
                case 'ε': return 238;
                case '∩': return 239;
                case '≡': return 240;
                case '±': return 241;
                case '≥': return 242;
                case '≤': return 243;
                case '⌠': return 244;
                case '⌡': return 245;
                case '÷': return 246;
                case '≈': return 247;
                case '°': return 248;
                case '∙': return 249;
                case '·': return 250;
                case '√': return 251;
                case 'ⁿ': return 252;
                case '²': return 253;
                case '■': return 254;
                default:
                    Kernel.PrintDebug($"Can't find char {x}");
                    return 63;
            }

            #endregion

            #endregion
        }

        public static List<byte> GetAsciiFromString(string x)
        {
            List<byte> ret = new List<byte>();
            foreach (char y in x)
            {
                ret.Add(GetAsciiFromChar(y));
            }

            return ret;

        }

        public static void Sparkle()
        {
            int c, d, g;
            int rx, ry;
            Random r = new Random();
            d = r.Next(1000);
            g = r.Next(100);
            c = r.Next(100000);
            rx = r.Next(80);
            ry = r.Next(25);


            if (GetScreenChar((uint)rx, (uint)ry) == 0x20 && c == 42)
            {
                //Kernel.PrintDebug($"sparkle at {rx}, {ry}");
                PutChar(ry, rx, 15, 0x0E);
            }
            else if (GetScreenChar((uint)rx, (uint)ry) == 15 && d <= 10)
            {
                //Kernel.PrintDebug($"found old sparkle at {rx}, {ry}");
                PutChar(ry, rx, 0xF8, 0x0E);
            }
            else if (GetScreenChar((uint)rx, (uint)ry) == 0xF8 && g <= 70)
            {
                //Kernel.PrintDebug($"found decayed sparkle at {rx}, {ry}");
                PutChar(ry, rx, 0x20, 0x7C);
            }
        }
    }

}
