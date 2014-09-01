using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripting.Objects
{
    public class Spell
    {
        internal WorldObjects.Spell spell;

        bool created = false;


        internal Spell(WorldObjects.Spell ii){
            spell = ii;
        }


        protected Spell()
        {
            spell = new WorldObjects.Spell();
            spell.ScriptingProto = this;
        }

        public Spell(String name, String fxName, String animName,
            float timePerMana, int DamagePerLevel, DamageTypes DamgeType,
            int mana)
            : this(name, fxName, animName, timePerMana, DamagePerLevel, DamgeType, true,
            new int[] { mana })
        {}

        public Spell(String name, String fxName, String animName,
            float timePerMana, int DamagePerLevel, DamageTypes DamgeType,
            bool CanTurnDuringInvest, int[] chargeMana)
                : this(name, fxName, animName, timePerMana, DamagePerLevel, DamgeType, SPELL_TYPE.BAD, 
                    CanTurnDuringInvest, true, false, SPELL_TARGET_COLLECT.TARGET_COLLECT_FOCUS_FALLBACK_NONE,
                    SPELL_TARGET_TYPES.TARGET_TYPE_NPCS, 10000, 60, 60, chargeMana)

        { }

        public Spell(String name, String fxName, String animName,
            float timePerMana, int DamagePerLevel, DamageTypes DamgeType, SPELL_TYPE SpellType,
            bool CanTurnDuringInvest, bool CanChangeTargetDuringInvest, bool isMultiEffect,
            SPELL_TARGET_COLLECT TargetCollectAlgo, SPELL_TARGET_TYPES TargetCollectType,
            int TargetCollectRange, int  TargetCollectAzi, int TargetCollectElev,
            int[] chargeMana)
            : this()
        {
            if (chargeMana == null)
                throw new ArgumentException("chargeMana can not be null!");
            spell.processMana = chargeMana;
            spell.Name = name;
            spell.FXName = fxName;
            spell.AniName = animName;

            spell.TimePerMana = timePerMana;
            spell.DamagePerLevel = DamagePerLevel;
            spell.DamageType = DamgeType;
            spell.SpellType = SpellType;
            spell.CanTurnDuringInvest = CanTurnDuringInvest;
            spell.CanChangeTargetDuringInvest = CanChangeTargetDuringInvest;
            spell.isMultiEffect = isMultiEffect;
            spell.TargetCollectionAlgo = TargetCollectAlgo;
            spell.TargetCollectType = TargetCollectType;
            spell.TargetCollectRange = TargetCollectRange;
            spell.TargetCollectAzi = TargetCollectAzi;
            spell.TargetCollectElev = TargetCollectElev;
            
            

            CreateSpell();
        }


        protected void CreateSpell()
        {
            if (created)
                return;

            GUC.WorldObjects.Spell.addItemInstance(spell);


            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.CreateSpellMessage);
            spell.Write(stream);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            created = true;
        }



        public String Name { get { return spell.Name; } protected set { spell.Name = value; } }
        public String FXName { get { return spell.FXName; } protected set { spell.FXName = value; } }
        public String AniName { get { return spell.AniName; } protected set { spell.AniName = value; } }

        public float TimePerMana { get { return spell.TimePerMana; } protected set { spell.TimePerMana = value; } }
        public int DamagePerLevel { get { return spell.DamagePerLevel; } protected set { spell.DamagePerLevel = value; } }
        public DamageTypes DamageType { get { return spell.DamageType; } protected set { spell.DamageType = value; } }
        public SPELL_TYPE SpellType { get { return spell.SpellType; } protected set { spell.SpellType = value; } }

        public bool CanTurnDuringInvest { get { return spell.CanTurnDuringInvest; } protected set { spell.CanTurnDuringInvest = value; } }
        public bool CanChangeTargetDuringInvest { get { return spell.CanChangeTargetDuringInvest; } protected set { spell.CanChangeTargetDuringInvest = value; } }
        public bool IsMultiEffect { get { return spell.isMultiEffect; } protected set { spell.isMultiEffect = value; } }

        public SPELL_TARGET_COLLECT TargetCollectionAlgo { get { return spell.TargetCollectionAlgo; } protected set { spell.TargetCollectionAlgo = value; } }
        public SPELL_TARGET_TYPES TargetCollectType { get { return spell.TargetCollectType; } protected set { spell.TargetCollectType = value; } }

        public int TargetCollectRange { get { return spell.TargetCollectRange; } protected set { spell.TargetCollectRange = value; } }
        public int TargetCollectAzi { get { return spell.TargetCollectAzi; } protected set { spell.TargetCollectAzi = value; } }
        public int TargetCollectElev { get { return spell.TargetCollectElev; } protected set { spell.TargetCollectElev = value; } }

        public int[] ChargeMana { get { return spell.processMana; } protected set { spell.processMana = value; } }
        public int Mana { get { return spell.processMana[0]; } protected set { spell.processMana = new int[]{value}; } }


        #region Events
        #region OnCastSpell
        public event GUC.Server.Scripting.Events.CastSpell OnCastSpell;
        internal void iOnCastSpell(NPCProto caster, Spell spell, Vob target)
        {
            if (OnCastSpell != null)
                OnCastSpell(caster, spell, target);
        }

        #endregion
        #endregion
    }
}
