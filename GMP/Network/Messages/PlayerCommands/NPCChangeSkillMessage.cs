using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.Enumeration;
using WinApi;
using Gothic.zClasses;

namespace GUC.Network.Messages.PlayerCommands
{
    class NPCChangeSkillMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int plID, value;
            byte changeType;
            byte talentType;

            stream.Read(out plID);
            stream.Read(out changeType);
            stream.Read(out talentType);
            stream.Read(out value);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPC!");

            NPCProto proto = (NPCProto)vob;

            if (changeType == (byte)ChangeSkillType.Hitchances)
                proto.Hitchances[talentType] = value;
            else if (changeType == (byte)ChangeSkillType.Skill)
                proto.TalentSkills[talentType] = value;
            else if (changeType == (byte)ChangeSkillType.Value)
                proto.TalentValues[talentType] = value;
            else
                throw new Exception("Does not know type: "+changeType);

            if (vob.Address == 0)
                return;

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, vob.Address);
            if (changeType == (byte)ChangeSkillType.Hitchances)
                npc.SetHitChances(talentType, value);
            else if (changeType == (byte)ChangeSkillType.Skill)
                npc.SetTalentSkill(talentType, value);
            else if (changeType == (byte)ChangeSkillType.Value)
                npc.SetTalentValue(talentType, value);
            
        }
    }
}
