using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;

namespace GUC.WorldObjects
{
    internal partial class ItemInstance
    {
        protected static int idCount = 0;
        public ItemInstance()
        {
            idCount++;
            this.id = idCount;
        }


        protected Server.Scripting.Objects.ItemInstance scriptingProto;

        public Server.Scripting.Objects.ItemInstance ScriptingProto
        {
            get
            {
                if (this.scriptingProto == null)
                {
                    this.scriptingProto = new Server.Scripting.Objects.ItemInstance(this);
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
            stream.Write(this.ID);
            stream.Write(Name);


            BitStreamExtension.Write(stream, (ulong)getParams());
            //stream.Write((long)getParams());
            if (ScemeName.Length > 0)
                stream.Write(ScemeName);

            for (int i = 0; i < Protection.Length; i++)
                if (Protection[i] != 0)
                    stream.Write(Protection[i]);

            if (DamageType != Enumeration.DamageType.DAM_INVALID)
                stream.Write((byte)DamageType);
            if (TotalDamage != 0)
                stream.Write(TotalDamage);

            for (int i = 0; i < Damages.Length; i++)
                if (Damages[i] != 0)
                    stream.Write(Damages[i]);

            if (Range != 0)
                stream.Write(Range);

            for (int i = 0; i < this.ConditionAttributes.Length; i++)
                if (ConditionAttributes[i] != 0)
                    stream.Write(ConditionAttributes[i]);
            for (int i = 0; i < this.ConditionValues.Length; i++)
                if (ConditionValues[i] != 0)
                    stream.Write(ConditionValues[i]);

            if (Value != 0)
                stream.Write(Value);

            if (MainFlags != 0)
                stream.Write((int)MainFlags);

            if (Flags != 0)
                stream.Write((int)Flags);

            if (Wear != 0)
                stream.Write((byte)Wear);
            if (Materials != 0)
                stream.Write((byte)Materials);

            if (Description.Length > 0)
                stream.Write(Description);

            for (int i = 0; i < this.Text.Length; i++)
                if (Text[i].Length > 0)
                    stream.Write(Text[i]);
            for (int i = 0; i < this.Count.Length; i++)
                if (Count[i] != 0)
                    stream.Write(Count[i]);

            if (Visual.Length > 0)
                stream.Write(Visual);
            if (Visual_Change.Length > 0)
                stream.Write(Visual_Change);
            if (Effect.Length > 0)
                stream.Write(Effect);

            if (Visual_skin != 0)
                stream.Write(Visual_skin);
            if (munition != null)
                stream.Write(munition.ID);
            if (isKeyInstance)
                stream.Write(isKeyInstance);
        }
    }
}
