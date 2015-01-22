using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    /**
    *   Enumeration with flags for all keystrokes done by client client.
    */
    public enum GothicKeys
    {
        DIK_ESCAPE = 0x01, /**< Escape key (main keyboard). */
        DIK_1 =     0x02, /**< 1 key (main keyboard) */
        DIK_2 =     0x03, /**< 2 key (main keyboard) */
        DIK_3 =     0x04, /**< 3 key (main keyboard) */
        DIK_4 =     0x05, /**< 4 key (main keyboard) */
        DIK_5 =     0x06, /**< 5 key (main keyboard) */
        DIK_6 =     0x07, /**< 6 key (main keyboard) */
        DIK_7 =     0x08, /**< 7 key (main keyboard) */
        DIK_8 =     0x09, /**< 8 key (main keyboard) */
        DIK_9 =     0x0A, /**< 9 key (main keyboard) */
        DIK_0 =     0x0B, /**< 0 key (main keyboard) */
        DIK_MINUS = 0x0C, /**< substract key (main keyboard). */
        DIK_EQUALS = 0x0D, /**< ???. */
        DIK_BACK =  0x0E, /**< Backspace key (main keyboard). */
        DIK_TAB =   0x0F, /**< Tab key (main keyboard). */
        DIK_Q =     0x10, /**< Q key (main keyboard). */
        DIK_W =     0x11, /**< W key (main keyboard). */
        DIK_E =     0x12, /**< E key (main keyboard). */
        DIK_R =     0x13, /**< R key (main keyboard). */
        DIK_T =     0x14, /**< T key (main keyboard). */
        DIK_Y =     0x15, /**< Y key (main keyboard). */
        DIK_U =     0x16, /**< U key (main keyboard). */
        DIK_I =     0x17, /**< I key (main keyboard). */
        DIK_O =     0x18, /**< O key (main keyboard). */
        DIK_P =     0x19, /**< P key (main keyboard). */
        DIK_LBRACKET = 0x1A, /**< Left bracket key (main keyboard). */
        DIK_RBRACKET = 0x1B, /**< Right bracket key (main keyboard). */
        DIK_RETURN =   0x1C, /**< Enter key (main keyboard). */
        DIK_LCONTROL = 0x1D, /**< Left Control key (main keyboard). */
        DIK_A =     0x1E, /**< A key (main keyboard). */
        DIK_S =     0x1F, /**< S key (main keyboard). */
        DIK_D =     0x20, /**< D key (main keyboard). */
        DIK_F =     0x21, /**< F key (main keyboard). */
        DIK_G =     0x22, /**< G key (main keyboard). */
        DIK_H =     0x23, /**< H key (main keyboard). */
        DIK_J =     0x24, /**< J key (main keyboard). */
        DIK_K =     0x25, /**< K key (main keyboard). */
        DIK_L =     0x26, /**< L key (main keyboard). */
        DIK_SEMICOLON =  0x27, /**< semicolon key (main keyboard). */
        DIK_APOSTROPHE = 0x28, /**< apostrophe key (main keyboard). */
        DIK_GRAVE = 0x29, /**< accentgrave key (main keyboard). */
        DIK_LSHIFT = 0x2A, /**< Left Shift key (main keyboard). */
        DIK_BACKSLASH = 0x2B, /**< Backslash key (main keyboard). */
        DIK_Z =     0x2C, /**< Z key (main keyboard). */
        DIK_X =     0x2D, /**< X key (main keyboard). */
        DIK_C =     0x2E, /**< C key (main keyboard). */
        DIK_V =     0x2F, /**< V key (main keyboard). */
        DIK_B =     0x30, /**< B key (main keyboard). */
        DIK_N =     0x31, /**< N key (main keyboard). */
        DIK_M =     0x32, /**< M key (main keyboard). */
        DIK_COMMA = 0x33, /**< comma key (main keyboard). */
        DIK_PERIOD = 0x34, /**< period key (main keyboard). */
        DIK_SLASH = 0x35, /**< slash key (main keyboard). */
        DIK_RSHIFT = 0x36, /**< Right Shift key (main keyboard). */
        DIK_MULTIPLY = 0x37, /**< multiply key (numpad). */
        DIK_LMENU = 0x38, /**< Left Alt key (main keyboard). */
        DIK_SPACE = 0x39, /**< Space key (main keyboard). */
        DIK_CAPITAL = 0x3A, /**< ???. */
        DIK_F1 =    0x3B, /**< F1 key (main keyboard). */
        DIK_F2 =    0x3C, /**< F2 key (main keyboard). */
        DIK_F3 =    0x3D, /**< F3 key (main keyboard). */
        DIK_F4 =    0x3E, /**< F4 key (main keyboard). */
        DIK_F5 =    0x3F, /**< F5 key (main keyboard). */
        DIK_F6 =    0x40, /**< F6 key (main keyboard). */
        DIK_F7 =    0x41, /**< F7 key (main keyboard). */
        DIK_F8 =    0x42, /**< F8 key (main keyboard). */
        DIK_F9 =    0x43, /**< F9 key (main keyboard). */
        DIK_F10 =   0x44, /**< F10 key (main keyboard). */
        DIK_NUMLOCK = 0x45, /**< Numlock key (numpad). */
        DIK_SCROLL = 0x46, /**< Scroll Lock key. */
        DIK_NUMPAD7 = 0x47, /**< 7 key (numpad). */
        DIK_NUMPAD8 = 0x48, /**< 8 key (numpad). */
        DIK_NUMPAD9 = 0x49, /**< 9 key (numpad). */
        DIK_SUBTRACT = 0x4A, /**< substract key (numpad). */
        DIK_NUMPAD4 = 0x4B, /**< 4 key (numpad). */
        DIK_NUMPAD5 = 0x4C, /**< 5 key (numpad). */
        DIK_NUMPAD6 = 0x4D, /**< 6 key (numpad). */
        DIK_ADD =     0x4E, /**< add key (numpad). */
        DIK_NUMPAD1 = 0x4F, /**< 1 key (numpad). */
        DIK_NUMPAD2 = 0x50, /**< 2 key (numpad). */
        DIK_NUMPAD3 = 0x51, /**< 3 key (numpad). */
        DIK_NUMPAD0 = 0x52, /**< 0 key (numpad). */
        DIK_DECIMAL = 0x53, /**< decimal key (numpad). */
        DIK_OEM_102 = 0x56, /**< Smaller As key (main keyboard). */
        DIK_F11 =   0x57, /**< F11 key (main keyboard). */
        DIK_F12 =   0x58, /**< F12 key (main keyboard). */
        DIK_F13 =   0x64, /**< F13 key (main keyboard) (NEC PC98). */
        DIK_F14 =   0x65, /**< F14 key (main keyboard) (NEC PC98). */
        DIK_F15 =   0x66, /**< F15 key (main keyboard) (NEC PC98). */
        DIK_KANA =      0x70, /**< Equals key (main keyboard) (Japanese). */
        DIK_ABNT_C1 =   0x73, /**< Slash key (main keyboard) (Portugese, Brazilian). */
        DIK_CONVERT =   0x79, /**< ??? (Japanese). */
        DIK_NOCONVERT = 0x7B, /**< ??? (Japanese). */
        DIK_YEN =       0x7D, /**< ??? (Japanese). */
        DIK_ABNT_C2 = 0x7E, /**< dot key (numpad) (Portugese, Brazilian). */
        DIK_NUMPADEQUALS = 0x8D, /**< Equals key (numpad) (NEC PC98). */
        DIK_PREVTRACK = 0x90, /**< Previous Track key (main keyboard) (Japanese DIK_CIRCUMFLEX). */
        DIK_AT =    0x91, /**< ??? (NEC PC98). */   /* = = (NEC PC98) */
        DIK_COLON = 0x92, /**< ??? (NEC PC98). */   /* = = (NEC PC98) */
        DIK_UNDERLINE = 0x93, /**< ??? (NEC PC98). */   /* = = (NEC PC98) */
        DIK_KANJI = 0x94, /**< ??? (Japanese). */
        DIK_STOP = 0x95, /**< ??? (NEC PC98). */   /* = = (NEC PC98) */
        DIK_AX = 0x96, /**< ??? (Japan AX). */   /* = = (Japan AX) */
        DIK_UNLABELED = 0x97, /**< ??? (J3100). */   /* = =    (J3100) */
        DIK_NEXTTRACK = 0x99, /**< Next Track key (main keyboard). */
        DIK_NUMPADENTER = 0x9C, /**< Enter key (numpad). */
        DIK_RCONTROL = 0x9D, /**< Right Control key (main keyboard). */
        DIK_MUTE =     0xA0, /**< Mute key (main keyboard). */
        DIK_CALCULATOR =  0xA1, /**< Calculator key (main keyboard). */
        DIK_PLAYPAUSE =   0xA2, /**< Play/Pause key (main keyboard). */
        DIK_MEDIASTOP =   0xA4, /**< Media Stop key (main keyboard). */
        DIK_VOLUMEDOWN =  0xAE, /**< Decrease Volume key (main keyboard). */
        DIK_VOLUMEUP =    0xB0, /**< Increase Volume key (main keyboard). */
        DIK_WEBHOME =     0xB2, /**< Web Home key (main keyboard). */
        DIK_NUMPADCOMMA = 0xB3, /**< comma key (numpad) (NEC PC98). */
        DIK_DIVIDE = 0xB5, /**< divide key (numpad). */
        DIK_SYSRQ = 0xB7, /**< ???. */
        DIK_RMENU = 0xB8, /**< Right Alt (main keyboard). */
        DIK_PAUSE = 0xC5, /**< Pause key. */
        DIK_HOME =  0xC7, /**< Home key (arrow keypad). */
        DIK_UP =    0xC8, /**< Up Arrow key (arrow keypad). */
        DIK_PRIOR = 0xC9, /**< Page Up key (arrow keypad). */
        DIK_LEFT =  0xCB, /**< Left Arrow key (arrow keypad). */
        DIK_RIGHT = 0xCD, /**< Right Arrow key (arrow keypad). */
        DIK_END =   0xCF, /**< End key (arrow keypad). */
        DIK_DOWN =  0xD0, /**< Down Arrow key (arrow keypad). */
        DIK_NEXT =  0xD1, /**< Page Down key (arrow keypad). */
        DIK_INSERT = 0xD2, /**< Insert key (arrow keypad). */
        DIK_DELETE = 0xD3, /**< Delete key (arrow keypad). */
        DIK_LWIN =  0xDB, /**< Left Windows key. */
        DIK_RWIN =  0xDC, /**< Right Windows key. */
        DIK_APPS =  0xDD, /**< AppMenu key. */
        DIK_POWER = 0xDE, /**< System Power key. */
        DIK_SLEEP = 0xDF, /**< System Sleep key. */
        DIK_WAKE =  0xE3, /**< System Wake key. */
        DIK_WEBSEARCH =     0xE5, /**< Web Search key. */
        DIK_WEBFAVORITES =  0xE6, /**< Web Favorites key. */
        DIK_WEBREFRESH =    0xE7, /**< Web Refresh key. */
        DIK_WEBSTOP =       0xE8, /**< Web Stop key. */
        DIK_WEBFORWARD =    0xE9, /**< Web Forward key. */
        DIK_WEBBACK =       0xEA, /**< Web Back key. */
        DIK_MYCOMPUTER =    0xEB, /**< My Computer key. */
        DIK_MAIL =          0xEC, /**< Mail key. */
        DIK_MEDIASELECT =   0xED, /**< Media Select key. */

        /* Alternate names for keys, to facilitate transition from DOS. */
        DIK_BACKSPACE = DIK_BACK, /**< Backspace key (main keyboard) (alternate name). */
        DIK_NUMPADSTAR = DIK_MULTIPLY, /**< multiply key (numpad) (alternate name). */
        DIK_LALT =      DIK_LMENU, /**< Left Alt key (main keyboard) (alternate name). */
        DIK_CAPSLOCK = DIK_CAPITAL, /**< Capslock key (main keyboard) (alternate name). */
        DIK_NUMPADMINUS = DIK_SUBTRACT, /**< Substract key (numpad) (alternate name). */
        DIK_NUMPADPLUS = DIK_ADD, /**< add key (numpad) (alternate name). */
        DIK_NUMPADPERIOD = DIK_DECIMAL, /**< dot key (numpad) (alternate name). */
        DIK_NUMPADSLASH = DIK_DIVIDE, /**< divide key (numpad) (alternate name). */
        DIK_RALT =      DIK_RMENU, /**< Right Alt key (main keyboard) (alternate name). */
        DIK_UPARROW =   DIK_UP, /**< Up Arrow key (arrow keypad) (alternate name). */
        DIK_PGUP =      DIK_PRIOR, /**< Page Up key (arrow keypad) (alternate name). */
        DIK_LEFTARROW = DIK_LEFT, /**< Left Arrow key (arrow keypad) (alternate name). */
        DIK_RIGHTARROW = DIK_RIGHT, /**< Right Arrow key (arrow keypad) (alternate name). */
        DIK_DOWNARROW = DIK_DOWN, /**< Down Arrow key (arrow keypad) (alternate name). */
        DIK_PGDN =      DIK_NEXT, /**< Page Down key (arrow keypad) (alternate name). */

        /* Alternate names for keys originally not used on US keyboards. */
        DIK_CIRCUMFLEX = DIK_PREVTRACK /**< ??? (Japanese). */      /* Japanese keyboard */





    }
}
