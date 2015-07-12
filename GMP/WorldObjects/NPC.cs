using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Enumeration;
using GUC.Types;
using Gothic.zTypes;
using RakNet;
using GUC.Network;

namespace GUC.Client.WorldObjects
{
    class NPC : Vob
    {
        public NPC(uint id)
            : base(id, oCObjectFactory.GetFactory(Program.Process).CreateNPC("OTHERS_NPC"))
        {
        }

        public NPC(uint id, oCNpc npc)
            : base(id, npc)
        {
        }

        public string Name
        {
            set
            {
                using (zString z = zString.Create(Program.Process, value))
                    gNpc.SetName(z);
            }
            get
            {
                return gNpc.Name.Value;
            }
        }

        public oCNpc gNpc
        {
            get
            {
                return new oCNpc(Program.Process, gVob.Address);
            }
        }

        #region Visual
        public string BodyMesh
        {
            get
            {
                return gNpc.BodyVisualString.ToString();
            }
        }

        public int BodyTex
        {
            get
            {
                return gNpc.BodyTex;
            }
        }

        public string HeadMesh
        {
            get
            {
                return gNpc.HeadVisualString.ToString();
            }
        }

        public int HeadTex
        {
            get
            {
                return gNpc.HeadTex;
            }
        }

        public float Fatness
        {
            get
            {
                return gNpc.Fatness;
            }
            set
            {
                gNpc.SetFatness(value);
            }
        }

        public float BodyHeight
        {
            get
            {
                return gNpc.Scale.Y;
            }
            set
            {
                using (zVec3 scale = zVec3.Create(Program.Process))
                {
                    scale.X = gNpc.Scale.X;
                    scale.Y = value;
                    scale.Z = gNpc.Scale.Z;
                    gNpc.SetModelScale(scale);
                }
            }
        }

        //x & z together
        public float BodyWidth
        {
            get
            {
                return gNpc.Scale.X;
            }
            set
            {
                using (zVec3 scale = zVec3.Create(Program.Process))
                {
                    scale.X = value;
                    scale.Y = gNpc.Scale.Y;
                    scale.Z = value;
                    gNpc.SetModelScale(scale);
                }
            }
        }
        #endregion

        #region Animation
        public List<String> Overlays = new List<string>();
        public short Animation = short.MaxValue;
        public long AnimationStartTime = 0;
        #endregion

        public int WeaponMode
        {
            get
            {
                return gNpc.WeaponMode;
            }
            set
            {
                gNpc.SetWeaponMode(value);
            }
        }

        //protected MobInter mobInter = null;
        //public MobInter MobInter { get { return mobInter; } set { mobInter = value; } }

        public bool IsDead
        {
            get
            {
                return gNpc.IsDead() > 0;
            }
        }
        public bool IsUnconcious
        {
            get
            {
                return gNpc.IsUnconscious() > 0;
            }
        }
    }
}
