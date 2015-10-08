using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using Gothic.zTypes;
using Gothic.zClasses;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace GUC.Client.WorldObjects
{
    class MobInstance : AbstractInstance
    {
        protected new static string fileName = "Data3.pak";

        MobType type;

        //Vob:
        public zString visual;
        public bool cdDyn;
        public bool cdStatic;

        //Mob:
        public zString focusName;

        //MobFire:
        public zString fireVobTreeName;

        //MobContainer:
        public ushort capacity;

        protected override void Read(BinaryReader br)
        {
            ID = br.ReadUInt16();

            type = (MobType)br.ReadByte();
            visual = zString.Create(Program.Process, br.ReadString());
            cdDyn = br.ReadByte() > 0;
            cdStatic = br.ReadByte() > 0;
            focusName = zString.Create(Program.Process, br.ReadString());
            fireVobTreeName = zString.Create(Program.Process, br.ReadString());
            capacity = br.ReadUInt16();
        }

        protected override void Write(BinaryWriter bw)
        {
            bw.Write(ID);

            bw.Write((byte)type);
            bw.Write(visual.Value);
            bw.Write(cdDyn);
            bw.Write(cdStatic);
            bw.Write(focusName.Value);
            bw.Write(fireVobTreeName.Value);
            bw.Write(capacity);
        }

        public new static MobInstance Get(ushort id)
        {
            AbstractInstance result = null;
            instanceList.TryGetValue(id, out result);
            return (MobInstance)result;
        }

        protected override void DisposeInstance()
        {
            visual.Dispose();
            focusName.Dispose();
            fireVobTreeName.Dispose();
        }

        public override zCVob CreateVob()
        {
            zCVob vob = zCVob.Create(Program.Process);
            vob.SetVisual(visual);

            if (cdDyn)
                vob.BitField1 |= (int)zCVob.BitFlag0.collDetectionDynamic;
            else
                vob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionDynamic;

            if (cdStatic)
                vob.BitField1 |= (int)zCVob.BitFlag0.collDetectionStatic;
            else
                vob.BitField1 &= ~(int)zCVob.BitFlag0.collDetectionStatic;

            vob.BitField1 |= (int)zCVob.BitFlag0.staticVob;


            switch (type)
            {
                case MobType.Mob:
                    ((oCMob)vob).SetName(focusName);
                    break;
            }

            return vob;
        }
    }
}
