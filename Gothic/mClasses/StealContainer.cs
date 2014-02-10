using System;
using System.Collections.Generic;
using System.Text;
using WinApi.User.Enumeration;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using System.Windows.Forms;

namespace Gothic.mClasses
{
    public class StealContainer : InputReceiver
    {
        public Process process;
        public StealContainer(Process process)
        {
            this.process = process;
        }


        public void Enable()
        {
            InputHooked.receivers.Add(this);
        }

        public void Disable()
        {
            InputHooked.receivers.Remove(this);
        }

        public event EventHandler<EventArgs> ButtonPressed;
        public void KeyPressed(int key)
        {
            try
            {
                if (key != (int)VirtualKeys.LeftButton)
                    return;

                oCNpc player = oCNpc.Player(process);
                if (player.Address == 0)
                    return;
                oCAniCtrl_Human aniCtrl = player.AniCtrl;
                if (aniCtrl.Address == 0)
                    return;
                zString str = aniCtrl.GetWalkModeString();
                if (str == null || str.Address == 0)
                    return;
                
                if (str.Value.Trim().ToLower() != "SNEAK".ToLower() || player.GetTalentSkill(6) == 0 ||  player.Enemy.Address == 0 ||
                    player.Enemy.CanSee(new zCVob(process, player.Address), 0) == 1 || process.ReadInt(0x00AB27D0) == 2)
                    return;

                player.OpenSteal();
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message+" StealContainer failure");
            }
        }
        public void wheelChanged(int steps) { }

        public void KeyReleased(int key)
        {

        }
    }
}
