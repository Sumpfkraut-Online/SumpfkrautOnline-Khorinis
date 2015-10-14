using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Enumeration;
using GUC.WorldObjects.Mobs;

namespace GUC.Network.Messages.VobCommands
{
    public class SetVobChangeMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int vobID = 0;
            byte changeID = 0;

            stream.Read(out vobID);
            stream.Read(out changeID);


            int intParam = 0;
            String stringParam = "";
            bool boolParam = false;
            ItemInstance itemInstance = null;

            VobChangeID cID = (VobChangeID)changeID;
            if (cID == VobChangeID.CDDyn || cID == VobChangeID.CDStatic || cID == VobChangeID.IsLocked)
            {
                stream.Read(out boolParam);
            }
            else if (cID == VobChangeID.FocusName || cID == VobChangeID.TriggerTarget || cID == VobChangeID.PickLockStr)
            {
                stream.Read(out stringParam);
            }
            else if (cID == VobChangeID.KeyInstance || cID == VobChangeID.UseWithItem )
            {
                stream.Read(out intParam);
                if (intParam != 0)
                {
                    if (!ItemInstance.ItemInstanceDict.ContainsKey(intParam))
                    {
                        throw new Exception("ItemInstance not found!");
                    }
                    itemInstance = ItemInstance.ItemInstanceDict[intParam];
                }
            }



            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];
            switch (cID)
            {
                case VobChangeID.CDDyn:
                    vob.setCDDyn( boolParam );
                    break;
                case VobChangeID.CDStatic:
                    vob.setCDStatic(boolParam);
                    break;
                case VobChangeID.FocusName:
                    if (!(vob is MobInter))
                        throw new Exception("Vob is not a MobInter!");
                    ((MobInter)vob).setFocusName(stringParam);

                    break;
                case VobChangeID.TriggerTarget:
                    if (!(vob is MobInter))
                        throw new Exception("Vob is not a MobInter!");
                    ((MobInter)vob).setTriggerTarget(stringParam);

                    break;
                case VobChangeID.UseWithItem:
                    if (!(vob is MobInter))
                        throw new Exception("Vob is not a MobInter!");
                    ((MobInter)vob).setUseWithItem(itemInstance);

                    break;
                case VobChangeID.IsLocked:
                    if (!(vob is MobLockable))
                        throw new Exception("Vob is not a MobLockable!");
                    ((MobLockable)vob).setIsLocked(boolParam);

                    break;
                case VobChangeID.PickLockStr:
                    if (!(vob is MobLockable))
                        throw new Exception("Vob is not a MobLockable!");
                    ((MobLockable)vob).setPickLockStr(stringParam);

                    break;
                case VobChangeID.KeyInstance:
                    if (!(vob is MobLockable))
                        throw new Exception("Vob is not a MobLockable!");
                    ((MobLockable)vob).setKeyInstance(itemInstance);

                    break;
            }

        }
    }
}
