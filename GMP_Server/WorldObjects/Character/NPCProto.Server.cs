using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Character
{
    internal abstract partial class NPCProto
    {

        public NPCProto()
            : base()
        {
            this.type = (int)VobTypes.Npc;
            this.visual = "HUMANS.MDS";

            this.Attributes[(int)NPCAttributeFlags.ATR_HITPOINTS] = 1;
            this.Attributes[(int)NPCAttributeFlags.ATR_HITPOINTS_MAX] = 1;
        }

        public virtual ulong Guid { get { return 0; } }
        public virtual RakNet.RakNetGUID GUID { get { return new RakNet.RakNetGUID(0); } }


        public Server.Scripting.Objects.Character.NPCProto ScriptingNPC
        {
            get { return (Server.Scripting.Objects.Character.NPCProto)ScriptingVob; }
            set { ScriptingVob = value; }
        }


        protected override VobSendFlags getSendInfo()
        {
            VobSendFlags b = base.getSendInfo();
            
            

            return b;
        }

        public override VobSendFlags Write(BitStream stream)
        {
            VobSendFlags sendInfo = base.Write(stream);

            stream.Write(Name);
            stream.Write(BodyMesh);
            stream.Write(BodyTex);
            stream.Write(SkinColor);
            stream.Write(HeadMesh);
            stream.Write(HeadTex);
            stream.Write(TeethTex);

            stream.Write(this.Attributes);
            stream.Write(this.TalentSkills);
            stream.Write(this.TalentValues);
            stream.Write(this.Hitchances);

            stream.Write(itemList.Count);
            foreach (Item it in itemList)
            {
                stream.Write(it.ID);
            }

            stream.Write(EquippedList.Count);
            foreach (Item it in EquippedList)
            {
                stream.Write(it.ID);
            }

            if (Armor == null)
                stream.Write(0);
            else
                stream.Write(Armor.ID);

            if (Weapon == null)
                stream.Write(0);
            else
                stream.Write(Weapon.ID);

            if (RangeWeapon == null)
                stream.Write(0);
            else
                stream.Write(RangeWeapon.ID);


            stream.Write(Overlays.Count);
            foreach (String str in Overlays)
            {
                stream.Write(str);
            }

            stream.Write(this.IsInvisible);
            stream.Write(this.hideName);

            return sendInfo;
        }

    }
}
