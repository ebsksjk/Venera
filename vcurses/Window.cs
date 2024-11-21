using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.vcurses
{
    //A window is a rectangular area of the screen
    class Window
    {
        public bool isTouched;
        public char background;
        public int xPos, yPos, xSize, ySize;
        public int curX, curY;
        public char[] content;
        public Window parent = null;
        public Window[] children;

        public Window(char _background, int _xSize, int _ySize, int _xPos, int _yPos)
        {
            background = _background;
            xSize = _xSize;
            ySize = _ySize;
            xPos = _xPos;
            yPos = _yPos;
            curX = 0;
            curY = 0;
            content = new char[xSize * ySize];
            Array.Fill(content, background);
            isTouched = true;
        }

        public void addch(char s) {
            if(curX + 1 >= xSize)
            {
                curX = 0;
                if(curY + 1 >= ySize)
                {
                    //perform scroll
                    for (int i = 1; i < ySize; i++)
                    {
                        for (int j = 0; j < xSize; j++)
                        {
                            content[((i - 1) * xSize) + j] = content[(i * xSize) + j];
                        }
                    }
                } else
                {
                    curY++;
                }
            }

            content[(curX * xSize) + curY] = s;

            isTouched = true;
        }

        public void mvaddchar(int x, int y, char s)
        {
            move(x, y);
            addch(s);
        }

        public void addstr(string s)
        {
            foreach (char c in s)
            {
                addch(c);
                curX++;
            }
        }

        public void move(int x, int y)
        {
            curX = x;
            curY = y;
        }

        public Window subwin(int sxSize, int sySize, int sxPos, int syPos, char sBackground)
        {
            Window sub = new Window(sBackground, sxSize, sySize, xPos + sxPos, yPos + syPos);  //pos is relative to parent, a subwindow is only allowed to be inside its parent
            sub.parent = this;
            children.Append(sub);
            return sub;
        }

        public void clear()
        {
            for(int i = 0; i < xSize * ySize; i++)
            {
                content[i] = background;
            }
        }

        public void box()
        {
            content[0] = '╔';
            content[xSize - 1] = '╗';
            content[(ySize-1) * xSize] = '╚';
            content[(ySize-1) * xSize + xSize - 1] = '╝';
            for (int i = 1; i < xSize - 1; i++)
            {
                content[i] = '═';
                content[(ySize-1) * xSize + i] = '═';
            }
            for (int i = 1; i < ySize-1; i++)
            {
                content[i * xSize] = '║';
                content[i * xSize + xSize - 1] = '║';
            }
        }

        public void refresh()
        {
            if(parent == null)
            {
                Console.SetCursorPosition(0, 0);
                string s = new string(content);
                s.Remove(s.Length - 2);
                Console.Write(s);
                return;
            }
            if(children.Length > 0)
            {
                foreach (Window w in children)
                {
                   w.refresh();
                }
            } else
            {
                for(int i = 0; i < xSize * ySize; i++)
                {
                    parent.content[((xPos * parent.xSize) + yPos) + i] = (content[i]);
                }
                return;
            }
        }


    }
}
