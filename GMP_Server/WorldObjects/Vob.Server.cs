using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Types;
using GUC.Network;
using GUC.WorldObjects.Character;
using GUC.WorldObjects.Mobs;

namespace GUC.WorldObjects
{
    internal partial class Vob
    {
        static int idCount = 0;


        public Vob()
        {
            idCount++;
            this._id = idCount;

            this.VobType = Enumeration.VobTypes.Vob;

            Position = new Vec3f();
        }

        protected Server.Scripting.Objects.Vob scriptingProto;

        public Server.Scripting.Objects.Vob ScriptingVob
        {
            get
            {
                if (this.scriptingProto == null)
                {
                    if (this is Player)
                        this.scriptingProto = new Server.Scripting.Objects.Character.Player((Player)this);
                    else if(this is NPCProto)
                        this.scriptingProto = new Server.Scripting.Objects.Character.NPC((NPC)this);
                    else if(this is Item)
                        this.scriptingProto = new Server.Scripting.Objects.Item((Item)this);
                    else if (this is MobSwitch)
                        this.scriptingProto = new Server.Scripting.Objects.Mob.MobSwitch((MobSwitch)this);
                    else if (this is MobBed)
                        this.scriptingProto = new Server.Scripting.Objects.Mob.MobBed((MobBed)this);
                    else if (this is MobDoor)
                        this.scriptingProto = new Server.Scripting.Objects.Mob.MobDoor((MobDoor)this);
                    else if (this is MobContainer)
                        this.scriptingProto = new Server.Scripting.Objects.Mob.MobContainer((MobContainer)this);
                    else if (this is MobInter)
                        this.scriptingProto = new Server.Scripting.Objects.Mob.MobInter((MobInter)this);
                    else if (this is Vob)
                        this.scriptingProto = new Server.Scripting.Objects.Vob((Vob)this);
                    
                }
                return this.scriptingProto;
            }

            set { if (this.scriptingProto != null) throw new Exception("Can only be set one time!"); this.scriptingProto = value; }
        }






        protected virtual VobSendFlags getSendInfo()
        {
            VobSendFlags a = 0;
            if (Visual.Length != 0)
                a |= VobSendFlags.Visual;
            if (!CDDyn)
                a |= VobSendFlags.CDDyn;
            if (!CDStatic)
                a |= VobSendFlags.CDStatic;
            return a;
        }
        public virtual VobSendFlags Write(BitStream stream)
        {
            VobSendFlags sendInfo = getSendInfo();
            stream.Write(ID);
            stream.Write((int)sendInfo);

            if (sendInfo.HasFlag(VobSendFlags.Visual))
                stream.Write(Visual);
            if (sendInfo.HasFlag(VobSendFlags.CDDyn))
                stream.Write(CDDyn);
            if (sendInfo.HasFlag(VobSendFlags.CDStatic))
                stream.Write(CDStatic);
            return sendInfo;
        }
    }
}
