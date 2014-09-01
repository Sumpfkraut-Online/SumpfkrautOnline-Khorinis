using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using Gothic.zClasses;
using WinApi;

namespace GUC.WorldObjects
{
    internal partial class Spell
    {
        public void Read(BitStream stream)
        {
            stream.Read(out this.id);
            stream.Read(out this.Name);
            stream.Read(out this.FXName);
            stream.Read(out this.AniName);

            int chargeCount = 0;
            stream.Read(out chargeCount);
            this.processMana = new int[chargeCount];
            for (int i = 0; i < chargeCount; i++)
                stream.Read(out processMana[i]);

            ushort param = 0;
            BitStreamExtension.Read(stream, out param);
            SpellParameters paramI = (SpellParameters)param;

            if (paramI.HasFlag(SpellParameters.TimePerMana))
                stream.Read(out this.TimePerMana);
            if (paramI.HasFlag(SpellParameters.DamagePerLevel))
                stream.Read(out this.DamagePerLevel);
            if (paramI.HasFlag(SpellParameters.DamageType))
            {
                byte dt = 0;
                stream.Read(out dt);
                DamageType = (DamageTypes)dt;
            }
            if (paramI.HasFlag(SpellParameters.SpellType))
            {
                byte dt = 0;
                stream.Read(out dt);
                SpellType = (SPELL_TYPE)dt;
            }
            if (paramI.HasFlag(SpellParameters.CanTurnDuringInvest))
                stream.Read(out this.CanTurnDuringInvest);
            if (paramI.HasFlag(SpellParameters.CanChangeTargetDuringInvest))
                stream.Read(out this.CanChangeTargetDuringInvest);
            if (paramI.HasFlag(SpellParameters.isMultiEffect))
                stream.Read(out this.isMultiEffect);
            if (paramI.HasFlag(SpellParameters.TargetCollectionAlgo))
            {
                byte dt = 0;
                stream.Read(out dt);
                TargetCollectionAlgo = (SPELL_TARGET_COLLECT)dt;
            }
            if (paramI.HasFlag(SpellParameters.TargetCollectType))
            {
                byte dt = 0;
                stream.Read(out dt);
                TargetCollectType = (SPELL_TARGET_TYPES)dt;
            }
            if (paramI.HasFlag(SpellParameters.TargetCollectRange))
                stream.Read(out this.TargetCollectRange);
            if (paramI.HasFlag(SpellParameters.TargetCollectAzi))
                stream.Read(out this.TargetCollectAzi);
            if (paramI.HasFlag(SpellParameters.TargetCollectElev))
                stream.Read(out this.TargetCollectElev);

        }

        public void toSpell(oCSpell spell)
        {
            spell.TimePerMana = TimePerMana;
            spell.DamagePerLevel = DamagePerLevel;
            spell.DamageType = (int)DamageType;
            spell.SpellType = (int)SpellType;
            spell.CanTurnDuringInvest = CanTurnDuringInvest ? 1 : 0;
            spell.CanChangeTargetDuringInvest = CanChangeTargetDuringInvest ? 1 : 0;
            spell.IsMultiEffect = isMultiEffect ? 1 : 0 ;
            spell.TargetCollectionAlgo = (int)TargetCollectionAlgo;
            spell.TargetCollectType = (int)TargetCollectType;
            spell.TargetCollectRange = TargetCollectRange;
            spell.TargetCollectAzi = TargetCollectAzi;
            spell.TargetCollectElev = TargetCollectElev;
        }
    }
}
