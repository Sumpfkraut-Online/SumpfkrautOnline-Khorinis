using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using Gothic.zTypes;
using Gothic.zClasses;
using WinApi;

namespace GUC.Tests
{
    public class Keytest : InputReceiver
    {

        public void KeyReleased(int key)
        {
            
        }

        public void KeyPressed(int key)
        {

            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Key: " + key + ", 0x" + key.ToString("X") + " ," + (DIK_Keys)key + " , " + ((DIK_Keys)key).getVirtualString(), 0, "Program.cs", 0);

        }

        public void wheelChanged(int steps)
        {
            
        }
    }
}
