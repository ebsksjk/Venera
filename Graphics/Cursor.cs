using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSharp.Assembler.x86;

namespace Venera.Graphics
{
    /// <summary>
    /// Style on how the cursor is blinking on the screen.
    /// </summary>
    public enum BlinkingStyle
    {
        Default,
        Static,
    }

    /// <summary>
    /// The blinking cursor on the screen.
    /// </summary>
    public class Cursor
    {
        private Chromat _chromat;

        public BlinkingStyle Style { get; set; }

        public int LinearCursorPosition { get; private set; }

        /// <summary>
        /// Get the (X, Y) coordinate of the current cursur <b>on the grid</b>.
        /// </summary>
        public (int X, int Y) Position
        {
            get
            {
                return (
                    X: Math.Abs((int)(LinearCursorPosition % _chromat.Screen.Width) + 1),
                    Y: Math.Abs((int)(LinearCursorPosition / _chromat.Screen.Height) + 1)
                );
            }
        }

        /// <summary>
        /// Get the (X, Y) coordinate of the current cursur <b>on screen space</b>.
        /// </summary>
        public (int X, int Y) RealPosition
        {
            get
            {
                return (
                    X: Math.Abs(Position.X * _chromat.TextWidth),
                    Y: Math.Abs(Position.Y * _chromat.TextHeight)
                );
            }
        }

        /// <summary>
        /// Move the cursor to the right.
        /// </summary>
        /// <param name="cells">How many cells on the horizontal. Positive values for the right and negative to move to the left.</param>
        public void MoveHorizontal(int cells)
        {
            // If we reached the bottom right corner.
            if (LinearCursorPosition > (_chromat.Screen.Width * _chromat.Screen.Height))
            {
                // TODO: Scroll screen
                LinearCursorPosition = _chromat.Grid.Width * (_chromat.Grid.Height - 1);
            }
            LinearCursorPosition += cells;
        }

        public Cursor(Chromat chromat, BlinkingStyle blinkingStyle = BlinkingStyle.Default)
        {
            Style = blinkingStyle;

            _chromat = chromat;
            LinearCursorPosition = 0;
        }
    }
}
