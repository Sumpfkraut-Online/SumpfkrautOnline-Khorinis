using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.User.Enumeration
{
    /// <summary>
    /// Enumeration containing the type device the raw input is coming from.
    /// </summary>
    public enum RawInputType
    {
        /// <summary>
        /// Mouse input.
        /// </summary>
        Mouse = 0,
        /// <summary>
        /// Keyboard input.
        /// </summary>
        Keyboard = 1,
        /// <summary>
        /// Another device that is not the keyboard or the mouse.
        /// </summary>
        HID = 2
    }
}
