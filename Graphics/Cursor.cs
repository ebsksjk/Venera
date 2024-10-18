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
        private int _cursorPosition;

        public BlinkingStyle Style { get; set; }

        /// <summary>
        /// Get the (X, Y) coordinate of the current cursur on the grid.
        /// </summary>
        public (int X, int Y) Position
        {
            get
            {
                return (
                    X: (int)(_cursorPosition / _chromat.Width),
                    Y: (int)(_cursorPosition % _chromat.Height)
                );
            }
        }

        public Cursor(Chromat chromat, BlinkingStyle blinkingStyle)
        {
            Style = blinkingStyle;

            _chromat = chromat;
        }
    }
}
