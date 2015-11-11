using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Server.WorldObjects;
using GUC.Server.Scripts.Sumpfkraut.Database;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    /**
     *   Class from which all npcs are instatiated (which are handled by the serverscript).
     */
    class NPCDef : VobDef
    {

        #region dictionaries

        protected static Dictionary<int, NPCDef> defById = new Dictionary<int, NPCDef>();
        protected static Dictionary<string, NPCDef> defByCodeName = new Dictionary<string, NPCDef>();

        new protected readonly Dictionary<String, SQLiteGetTypeEnum> defTab_GetTypeByColumn =
            new Dictionary<String, SQLiteGetTypeEnum>
            {
                {"NpcDefId",                SQLiteGetTypeEnum.GetInt32},
                {"ChangeDate",              SQLiteGetTypeEnum.GetString},
                {"CreationDate",            SQLiteGetTypeEnum.GetString},
            };

        #endregion



        #region standard attributes

        new public static readonly String _staticName = "NPCDef (static)";
        new protected String _objName = "NPCDef (default)";

        new protected static Type _type = typeof(NPCDef);

        protected int ID;
        public int getID () { return this.ID; }
        public void setID (int ID) { this.ID = ID; }

        protected string Name;
        public string getName () { return this.Name; }
        public void setName (string Name) { this.Name = Name; }

        protected int[] Attributes;
        public int[] getAttributes () { return this.Attributes; }
        public void setAttributes (int[] Attributes) { this.Attributes = Attributes; }

        protected int[] TalentValues;
        public int[] getTalentValues () { return this.TalentValues; }
        public void setTalentValues (int[] TalentValues) { this.TalentValues = TalentValues; }

        protected int[] TalentSkills;
        public int[] getTalentSkills () { return this.TalentSkills; }
        public void setTalentSkills (int[] TalentSkills) { this.TalentSkills = TalentSkills; }

        protected int[] HitChances;
        public int[] getHitChances () { return this.HitChances; }
        public void setHitChances (int[] HitChances) { this.HitChances = HitChances; }

        protected int Guild;
        public int getGuild () { return this.Guild; }
        public void setGuild (int Guild) { this.Guild = Guild; }

        protected int Voice;
        public int getVoice () { return this.Voice; }
        public void setVoice (int Voice) { this.Voice = Voice; }

        protected string Visual;
        public string getVisual () { return this.Visual; }
        public void setVisual (string Visual) { this.Visual = Visual; }

        protected string BodyMesh;
        public string getBodyMesh () { return this.BodyMesh; }
        public void setBodyMesh (string BodyMesh) { this.BodyMesh = BodyMesh; }

        protected int BodyTex;
        public int getBodyTex () { return this.BodyTex; }
        public void setBodyTex (int BodyTex) { this.BodyTex = BodyTex; }

        protected int SkinColor;
        public int getSkinColor () { return this.SkinColor; }
        public void setSkinColor (int SkinColor) { this.SkinColor = SkinColor; }

        protected string HeadMesh;
        public string getHeadMesh () { return this.HeadMesh; }
        public void setHeadMesh (string HeadMesh) { this.HeadMesh = HeadMesh; }

        protected int HeadTex;
        public int getHeadTex () { return this.HeadTex; }
        public void setHeadTex (int HeadTex) { this.HeadTex = HeadTex; }

        protected int TeethTex;
        public int getTeethTex () { return this.TeethTex; }
        public void setTeethTex (int TeethTex) { this.TeethTex = TeethTex; }

        #endregion



        #region constructors

        public NPCDef(String name, int[] attributes, int[] talentValues, int[] talentSkills, int[] hitChances, 
            int guild, int voice, String visual, String bodyMesh, int bodyTex, int skinColor, 
            String headMesh, int headTex, int teethTex)
        {
            this._objName = "NPCDef (default)";
            this.Name = name;
            this.Attributes = attributes;
            this.TalentValues = talentValues;
            this.TalentSkills = talentSkills;
            this.HitChances = hitChances;
            this.Guild = guild;
            this.Voice = voice;
            this.Visual = visual;
            this.BodyMesh = bodyMesh;
            this.BodyTex = bodyTex;
            this.SkinColor = skinColor;
            this.HeadMesh = headMesh;
            this.HeadTex = headTex;
            this.TeethTex = teethTex;
        }

        #endregion



        #region dictionary-methods

        public static bool Add(NPCDef def)
        {
            return Add(_type, def);
        }

        public static bool ContainsCodeName(String codeName)
        {
            return ContainsCodeName(_type, codeName);
        }

        public static bool ContainsId(int id)
        {
            return ContainsId(_type, id);
        }

        public static bool ContainsDefinition(VobDef def)
        {
            return ContainsDefinition(_type, def);
        }

        public static bool RemoveCodeName(String codeName)
        {
            return RemoveCodeName(_type, codeName);
        }

        public static bool RemoveId(int id)
        {
            return RemoveId(_type, id);
        }

        public static bool TryGetValueByCodeName(String codeName, out NPCDef def)
        {
            VobDef tempDef;
            bool result = TryGetValueByCodeName(_type, codeName, out tempDef);
            def = (NPCDef)tempDef;
            return result;
        }

        public static bool TryGetValueById(int id, out NPCDef def)
        {
            VobDef tempDef;
            bool result = TryGetValueById(_type, id, out tempDef);
            def = (NPCDef)tempDef;
            return result;
        }

        #endregion

    }
}
