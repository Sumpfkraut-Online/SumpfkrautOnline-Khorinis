using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using Gothic.Types;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class Vob
    {
        #region Network Messages

        new internal static class Messages
        {
            public static void ReadThrow(PacketReader stream)
            {
                int id = stream.ReadUShort();

                Vob vob;
                if (World.Current.TryGetVob(id, out vob))
                {
                    vob.ScriptObject.Throw(stream.ReadVec3f());
                }
            }
        }

        #endregion


        public void SetPhysics(bool enabled)
        {
            this.gVob.SetPhysicsEnabled(enabled);
        }

        partial void pThrow(Vec3f velocity)
        {
            SetPhysics(true);
            var rb = WinApi.Process.ReadInt(gVob.Address + 224);
            using (zVec3 vec = velocity.CreateGVec())
                WinApi.Process.THISCALL<WinApi.NullReturnCall>(rb, 0x5B66D0, vec);
        }
    }
}
