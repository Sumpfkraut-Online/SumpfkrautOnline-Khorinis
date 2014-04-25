using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using RakNet;

namespace GUC.Server.Scripting.Objects.Character
{
    public class NPC : NPCProto
    {
        
        internal NPC(GUC.WorldObjects.Character.NPCProto proto)
            : base(proto)
        {
            
        }

        public NPC(String name)
            : this(name, 10, 10, 0, 0, 10, 10, 0, 0)
        { }

        public NPC(String name, int hp, int hpmax, int mp, int mpmax, int strength, int dexterity, int guild, int voice)
            : this(name, new int[(int)NPCAttributeFlags.ATR_MAX] { hp, hpmax, mp, mpmax, strength, dexterity, 0, 0 }, new int[(int)NPCTalents.MaxTalents], new int[(int)NPCTalents.MaxTalents], new int[5], guild, voice, "HUMANS.MDS", "hum_body_Naked0", 9, 0, "Hum_Head_Pony", 18, 0)
        {}

        public NPC(String name, int hp, int hpmax, int mp, int mpmax, int strength, int dexterity, int guild, int voice, String visual, String bodyMesh, int bodyTex, int skinColor, String headMesh, int headTex, int TeethTex)
            : this(name, new int[(int)NPCAttributeFlags.ATR_MAX] { hp, hpmax, mp, mpmax, strength, dexterity, 0, 0 }, new int[(int)NPCTalents.MaxTalents], new int[(int)NPCTalents.MaxTalents], new int[5], guild, voice, visual, bodyMesh, bodyTex, skinColor, headMesh, headTex, TeethTex)
        {}

        public NPC(String name, int[] attributes, int[] talentValues, int[] talentSkills, int[] hitChances, int guild, int voice, String visual, String bodyMesh, int bodyTex, int skinColor, String headMesh, int headTex, int TeethTex)
            : this(name, attributes, talentValues, talentSkills, hitChances, guild, voice, visual, bodyMesh, bodyTex, skinColor, headMesh, headTex, TeethTex, true)
        { }

        protected NPC(String name, int[] attributes, int[] talentValues, int[] talentSkills, int[] hitChances, int guild, int voice,  String visual, String bodyMesh, int bodyTex, int skinColor, String headMesh, int headTex, int TeethTex, bool useCreate)
            : base(new GUC.WorldObjects.Character.NPC())
        {
            this.proto.ScriptingNPC = this;

            Name = name;
            this.HP = attributes[(int)NPCAttributeFlags.ATR_HITPOINTS];
            this.HPMax = attributes[(int)NPCAttributeFlags.ATR_HITPOINTS_MAX];
            this.MP = attributes[(int)NPCAttributeFlags.ATR_MANA];
            this.MPMax = attributes[(int)NPCAttributeFlags.ATR_MANA_MAX];
            this.Strength = attributes[(int)NPCAttributeFlags.ATR_STRENGTH];
            this.Dexterity = attributes[(int)NPCAttributeFlags.ATR_DEXTERITY];

            this.setVisual(visual, bodyMesh, bodyTex, skinColor, headMesh, headTex, TeethTex);

            if (useCreate)
                CreateVob();
        }

        internal WorldObjects.Character.NPC ProtoNPC { get { return (WorldObjects.Character.NPC)proto; } }

        public Player NPCController { get { return (ProtoNPC.NpcController == null) ? null : (Player)ProtoNPC.NpcController.ScriptingNPC; } }
        

    }
}
