using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting;
using GUC.Types;

namespace GUC.Server.Scripts.Items
{
    public class SPELL_SHRINK : Spell
    {
        static SPELL_SHRINK shrink;
        public static SPELL_SHRINK get()
        {
            if (shrink == null)
                shrink = new SPELL_SHRINK();
            return shrink;
        }

        protected SPELL_SHRINK()
            : base()
        {
            TimePerMana = 0;
            SpellType = Enumeration.SPELL_TYPE.NEUTRAL;
            TargetCollectionAlgo = Enumeration.SPELL_TARGET_COLLECT.TARGET_COLLECT_FOCUS;
            TargetCollectRange = 1000;

            Mana = 5;

            Name = "Schrumpfen";
            AniName = "SLE";
            FXName = "Shrink";

            this.OnCastSpell += new Events.CastSpell(cast);

            CreateSpell();


            
        }

        protected void cast(NPCProto caster, Spell spell, Vob target)
        {
            if (target == null || !(target is NPCProto))
                return;

            NPCProto targetNPC = (NPCProto)target;

            //targetNPC.setScale(new Types.Vec3f(0.2f, 0.2f, 0.2f));

            ShrinkTimer st = new ShrinkTimer(targetNPC);
        }

    }

    public class ShrinkTimer : Timer
    {
        NPCProto npcToShrink = null;
        Vec3f initalScale = null;

        public ShrinkTimer(NPCProto proto)
            : base(10000*500)
        {
            npcToShrink = proto;
            initalScale = npcToShrink.Scale;
            OnTick += new Events.TimerEvent(tick);

            Start();
        }

        protected void tick()
        {
            if (npcToShrink.Scale.Y > 0.7f)
                npcToShrink.setScale(new Vec3f(0.699f, 0.699f, 0.699f));
            else if (npcToShrink.Scale.Y > 0.5f)
                npcToShrink.setScale(new Vec3f(0.499f, 0.499f, 0.499f));
            else if (npcToShrink.Scale.Y > 0.3f)
                npcToShrink.setScale(new Vec3f(0.299f, 0.299f, 0.299f));
            else
            {
                OnTick -= new Events.TimerEvent(tick);
                OnTick += new Events.TimerEvent(tickUnShrink);
                this.End();
                this.TimeSpan = 10000 * 1000 * 25;
                this.Start();
            }
        }

        protected void tickUnShrink()
        {
            npcToShrink.setScale(initalScale);
            this.End();
        }
    }


    public class ITSC_SHRINK : ItemInstance
    {
        static ITSC_SHRINK shrink;
        public static ITSC_SHRINK get()
        {
            if (shrink == null)
                shrink = new ITSC_SHRINK();
            return shrink;
        }


        protected ITSC_SHRINK()
            : base("ITSC_SHRINK_2")
        {
            Name = "Spruchrolle";
            MainFlags = Enumeration.MainFlags.ITEM_KAT_RUNE;
            Flags = Enumeration.Flags.ITEM_MULTI;

            Value = 25;

            Visual = "ItSc_Shrink.3DS";
            Materials = Enumeration.MaterialTypes.MAT_LEATHER;
            Wear = Enumeration.ArmorFlags.WEAR_EFFECT;

            Effect = "SPELLFX_WEAKGLIMMER";
            Description = "Schrumpfen";
            //SPELL_SHRINK.get();
            Spell = SPELL_SHRINK.get();

            CreateItemInstance();
        }
    }
}
