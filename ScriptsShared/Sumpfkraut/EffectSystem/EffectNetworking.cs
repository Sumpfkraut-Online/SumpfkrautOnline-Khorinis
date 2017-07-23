using GUC.Network;
using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Types;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public class EffectNetworking : ExtendedObject
    {

        public delegate void WriteObject (PacketWriter pw, object obj);
        public delegate object ReadObject (PacketReader pr);
        public class ReadWriteObject
        {
            public WriteObject Write;
            public ReadObject Read;

            public ReadWriteObject (WriteObject write, ReadObject read)
            {
                Write = write;
                Read = read;
            }
        }

        public static readonly Dictionary<Type, ReadWriteObject> TypeToRW = 
            new Dictionary<Type, ReadWriteObject>()
        {
            { typeof(byte), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((byte) obj); },
                (PacketReader pr) => { return pr.ReadByte(); }
            ) },
            { typeof(sbyte), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((sbyte) obj); },
                (PacketReader pr) => { return pr.ReadSByte(); }
            ) },

            { typeof(bool), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((bool) obj); },
                (PacketReader pr) => { return Convert.ToBoolean(pr.ReadByte()); }
            ) },

            { typeof(short), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((short) obj); },
                (PacketReader pr) => { return (short) pr.ReadByte(); }
            ) },
            { typeof(ushort), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((ushort) obj); },
                (PacketReader pr) => { return (ushort) pr.ReadByte(); }
            ) },

            { typeof(int), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((int) obj); },
                (PacketReader pr) => { return pr.ReadInt(); }
            ) },
            { typeof(uint), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((uint) obj); },
                (PacketReader pr) => { return (uint) pr.ReadInt(); }
            ) },

            { typeof(long), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((long) obj); },
                (PacketReader pr) => { return pr.ReadLong(); }
            ) },
            { typeof(ulong), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((ulong) obj); },
                (PacketReader pr) => { return (ulong) pr.ReadLong(); }
            ) },

            { typeof(float), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((float) obj); },
                (PacketReader pr) => { return pr.ReadFloat(); }
            ) },

            { typeof(double), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((double) obj); },
                (PacketReader pr) => { return pr.ReadDouble(); }
            ) },

            { typeof(ColorRGBA), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((ColorRGBA) obj); },
                (PacketReader pr) => { return pr.ReadColorRGBA(); }
            ) },

            { typeof(Vec3f), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((Vec3f) obj); },
                (PacketReader pr) => { return pr.ReadVec3f(); }
            ) },

            { typeof(ChangeDestination), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((int) obj); },
                (PacketReader pr) => { return (ChangeDestination) pr.ReadInt(); }
            ) },

            { typeof(ChangeType), new ReadWriteObject(
                (PacketWriter pw, object obj) => { pw.Write((int) obj); },
                (PacketReader pr) => { return (ChangeType) pr.ReadInt(); }
            ) },
        };



        public static void WriteTotalChange (PacketWriter pw, ChangeDestination cd, TotalChange tc)
        {
            var total = tc.GetTotal();
            var param = total.GetParameters();
            // return preemtively when there is nothing to send
            if (total == null) { return; }

            // write head
            TypeToRW[typeof(ChangeDestination)].Write(pw, cd);
            TypeToRW[typeof(ChangeType)].Write(pw, total.GetChangeType());
            TypeToRW[typeof(int)].Write(pw, param.Count);

            // write parameters
            Type t;
            ReadWriteObject rw;
            for (int i = 0; i < param.Count; i++)
            {
                t = param[i].GetType();
                if (TypeToRW.TryGetValue(t, out rw))
                {
                    rw.Write(pw, param[i]);
                }
            }
        }

        public static TotalChange ReadTotalChange (PacketReader pr)
        {
            var tc = new TotalChange();

            // head
            var changeDest = (ChangeDestination) TypeToRW[typeof(ChangeDestination)].Read(pr);
            var changeType = (ChangeType) TypeToRW[typeof(ChangeType)].Read(pr);
            var paramLength = (int) TypeToRW[typeof(int)].Read(pr);

            // parameters
            List<object> param = new List<object>(paramLength);

            // generate the simplified TotalChange with only a total-value, no components
            var total = Change.Create(changeType, param);
            tc.SetTotal(total);

            return tc;
        }

    }

}
