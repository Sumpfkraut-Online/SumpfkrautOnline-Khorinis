using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface ITriggerListener
    {
        void OnTrigger(Player pl, int vobType, String vobName, float[] pos);
        void OnUnTrigger(Player pl, int vobType, String vobName, float[] pos);
        void OnStartInteraction(Player pl, int vobType, String vobName, float[] pos);
        void OnStopInteraction(Player pl, int vobType, String vobName, float[] pos);

        void OnPickLock(Player pl, int vobType, String vobName, float[] pos, int ch);

        void OnOpenContainer(Player pl, int vobType, String vobName, float[] pos);
        void OnCloseContainer(Player pl, int vobType, String vobName, float[] pos, bool hasKey);
        void OnInsertItemToContainer(Player pl, int vobType, String vobName, float[] pos, String item, int amount);
        void OnRemoveItemToContainer(Player pl, int vobType, String vobName, float[] pos, String item, int amount);
    }
}
