using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;

namespace GUC.WorldObjects
{
    internal partial class Spell
    {
        protected static int idCount = 100;//starts with 101
        public Spell()
        {
            idCount++;
            this.id = idCount;
        }


        protected Server.Scripting.Objects.Spell scriptingProto;

        public Server.Scripting.Objects.Spell ScriptingProto
        {
            get
            {
                if (this.scriptingProto == null)
                {
                    this.scriptingProto = new Server.Scripting.Objects.Spell(this);
                }
                return this.scriptingProto;
            }
            set
            {
                if (this.scriptingProto != null)
                    throw new ArgumentException("Scripting Proto can only be set 1 time!");
                this.scriptingProto = value;
            }
        }

        public void Write(BitStream stream)
        {
            stream.Write( this.id);
            stream.Write( this.Name);
            stream.Write( this.FXName);
            stream.Write( this.AniName);

            stream.Write(processMana.Length);
            for (int i = 0; i < processMana.Length; i++)
                stream.Write(processMana[i]);

            SpellParameters paramI = getParams();
            BitStreamExtension.Write(stream, (ushort)paramI);

            if (paramI.HasFlag(SpellParameters.TimePerMana))
                stream.Write(this.TimePerMana);
            if (paramI.HasFlag(SpellParameters.DamagePerLevel))
                stream.Write(this.DamagePerLevel);
            if (paramI.HasFlag(SpellParameters.DamageType))
                stream.Write((byte)DamageType);
            if (paramI.HasFlag(SpellParameters.SpellType))
                stream.Write((byte)SpellType);
            if (paramI.HasFlag(SpellParameters.CanTurnDuringInvest))
                stream.Write(this.CanTurnDuringInvest);
            if (paramI.HasFlag(SpellParameters.CanChangeTargetDuringInvest))
                stream.Write(this.CanChangeTargetDuringInvest);
            if (paramI.HasFlag(SpellParameters.isMultiEffect))
                stream.Write(this.isMultiEffect);
            if (paramI.HasFlag(SpellParameters.TargetCollectionAlgo))
                stream.Write((byte)TargetCollectionAlgo);
            if (paramI.HasFlag(SpellParameters.TargetCollectType))
                stream.Write((byte)TargetCollectType);
            if (paramI.HasFlag(SpellParameters.TargetCollectRange))
                stream.Write(this.TargetCollectRange);
            if (paramI.HasFlag(SpellParameters.TargetCollectAzi))
                stream.Write(this.TargetCollectAzi);
            if (paramI.HasFlag(SpellParameters.TargetCollectElev))
                stream.Write(this.TargetCollectElev);

        }
    }
}
