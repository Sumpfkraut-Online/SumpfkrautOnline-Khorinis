using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.WorldObjects.Character;
using WinApi;
using Gothic.zClasses;
using GUC.Enumeration;

namespace GUC.WorldObjects.Mobs
{
    internal partial class MobInter
    {
        public override void Spawn(String map, Vec3f position, Vec3f direction)
        {
            this.Map = map;
            this.Position = position;
            this.Direction = direction;

            spawned = true;

            if (this.Address != 0)
                return;

            if (this.Map != Player.Hero.Map)
                return;

            Process process = Process.ThisProcess();

            oCMobInter gVob = oCMobInter.Create(process);
            gVob.VobType = zCVob.VobTypes.MobInter;


            this.Address = gVob.Address;
            sWorld.SpawnedVobDict.Add(this.Address, this);
            
            setVobData(process, gVob);
            setMobInterData(process, gVob);

            oCGame.Game(process).World.AddVob(gVob);

            triggerMobInter(process, gVob);

            
        }

        protected void setMobInterData(Process process, oCMobInter mobinter){
            if (this.FocusName != null && this.FocusName.Length != 0)
            {
                mobinter.SetName(this.FocusName);
                mobinter.Name.Set(this.FocusName);
            }

            if (this.UseWithItem != null)
                mobinter.SetUseWithItem("ITGUC_" + this.UseWithItem.ID);
            mobinter.Rewind = false;
            if (this.TriggerTarget != null && this.TriggerTarget.Length != 0)
                mobinter.TriggerTarget.Set(this.TriggerTarget);
        }

        public void setFocusName(String focusName)
        {
            this.FocusName = focusName;

            if (this.Address == 0)
                return;

            oCMobInter mobInter = new oCMobInter(Process.ThisProcess(), this.Address);
            if (this.FocusName != null && this.FocusName.Length != 0)
            {
                mobInter.SetName(this.FocusName);
                mobInter.Name.Set(this.FocusName);
            }
            else
            {
                mobInter.SetName("");
                mobInter.Name.Set("");
            }
        }

        public void setTriggerTarget(String triggerTarget)
        {
            this.TriggerTarget = triggerTarget;

            if (this.Address == 0)
                return;

            oCMobInter mobInter = new oCMobInter(Process.ThisProcess(), this.Address);
            if (this.TriggerTarget != null && this.TriggerTarget.Length != 0)
                mobInter.TriggerTarget.Set(this.TriggerTarget);
            else
                mobInter.TriggerTarget.Set("");
        }

        public void setUseWithItem(ItemInstance useWithItem)
        {
            this.UseWithItem = useWithItem;

            if (this.Address == 0)
                return;

            oCMobInter mobInter = new oCMobInter(Process.ThisProcess(), this.Address);
            if (this.UseWithItem != null)
                mobInter.SetUseWithItem("ITGUC_" + this.UseWithItem.ID);
            else
                mobInter.SetUseWithItem("");
        }

        protected void triggerMobInter(Process process, oCMobInter v)
        {
            if (v.State == 0 && this.State != 0)
            {
                v.OnTrigger(new zCVob(process, 0), new zCVob(process, 0));
                if (this.VobType == VobType.MobDoor)
                {
                    v.GetModel().StartAnimation("T_S0_2_S1");
                }
            }
            else if (v.State != 0 && this.State == 0)
            {
                v.OnUnTrigger(new zCVob(process, 0), new zCVob(process, 0));
                if (this.VobType == VobType.MobDoor)
                {
                    v.GetModel().StartAnimation("T_S1_2_S0");
                }
            }
        }

        public override VobSendFlags Read(RakNet.BitStream stream)
        {
            VobSendFlags sendFlags = base.Read(stream);

            if (sendFlags.HasFlag(VobSendFlags.FocusName))
                stream.Read(out focusName);
            if (sendFlags.HasFlag(VobSendFlags.State))
                stream.Read(out state);
            if (sendFlags.HasFlag(VobSendFlags.UseWithItem))
            {
                int instanceID = 0;
                stream.Read(out instanceID);
                ItemInstance ii = ItemInstance.ItemInstanceDict[instanceID];
                this.useWithItem = ii;
            }
            if (sendFlags.HasFlag(VobSendFlags.TriggerTarget))
                stream.Read(out triggerTarget);

            return sendFlags;
        }
    }
}
