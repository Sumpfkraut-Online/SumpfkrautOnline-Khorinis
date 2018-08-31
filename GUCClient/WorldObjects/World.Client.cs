using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic;
using Gothic.Objects;
using GUC.Network;
using GUC.Scripting;
using GUC.Types;
using GUC.GUI;

namespace GUC.WorldObjects
{
    public partial class World
    {
        #region Network Messages

        internal static class Messages
        {
            #region World Loading & Joining

            public static void ReadLoadWorld(PacketReader stream)
            {
                if (current != null)
                {
                    current.ForEachVob(v => v.Despawn());
                    current.Delete();
                }

                current = ScriptManager.Interface.CreateWorld();

                current.ID = 0;
                current.Create();

                current.ReadStream(stream);
                if (stream.ReadBit())
                {
                    current.Clock.ScriptObject.Start();
                }
                current.WeatherCtrl.ScriptObject.SetWeatherType(current.WeatherCtrl.WeatherType);
                current.WeatherCtrl.ScriptObject.SetNextWeight(current.WeatherCtrl.EndTime, current.WeatherCtrl.EndWeight);
                current.BarrierCtrl.ScriptObject.SetNextWeight(current.BarrierCtrl.EndTime, current.BarrierCtrl.EndWeight);

                Hooks.hGame.FirstRenderDone = false;
                current.ScriptObject.Load();

                var hero = oCNpc.GetPlayer();
                if (hero.Address != 0)
                {
                    hero.Disable();
                    GothicGlobals.Game.GetWorld().RemoveVob(hero);
                }

                PacketWriter confirmation = GameClient.SetupStream(ClientMessages.WorldLoadedMessage);
                GameClient.Send(confirmation, NetPriority.Immediate, NetReliability.Reliable);

                CGameManager.ApplySomeSettings();
            }

            public static void ReadJoinWorld(PacketReader stream)
            {
                for (int i = stream.ReadUShort(); i > 0; i--)
                {
                    ReadVobSpawn(stream);
                }
            }

            public static void ReadLeaveWorldMessage(PacketReader stream)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region Spawns

            public static void ReadCellMessage(PacketReader stream)
            {
                // remove vobs
                for (int i = stream.ReadUShort(); i > 0; i--)
                {
                    ReadVobDespawnMessage(stream);
                }

                // add vobs
                for (int i = stream.ReadUShort(); i > 0; i--)
                {
                    ReadVobSpawn(stream);
                }
            }

            public static void ReadVobSpawn(PacketReader stream)
            {
                BaseVob vob = ScriptManager.Interface.CreateVob((VobTypes)stream.ReadByte());
                vob.ReadStream(stream);
                vob.ScriptObject.Spawn(current);
            }

            public static void ReadVobDespawnMessage(PacketReader stream)
            {
                int id = stream.ReadUShort();

                if (current.TryGetVob(id, out BaseVob vob))
                {
                    vob.ScriptObject.Despawn();
                }

                GameClient.Client.guidedIDs.Remove(id);
            }

            #endregion
        }

        #endregion

        static World current;
        public static World Current { get { return current; } }

        #region ScriptObject

        public partial interface IScriptWorld : IScriptGameObject
        {
            void Load();
        }

        #endregion

        #region Properties

        /// <summary> The correlating gothic-object of this world. </summary>
        public zCWorld gWorld { get { return GothicGlobals.Game.GetWorld(); } }

        #endregion

        #region Gothic-Object Address Dictionary

        // Dictionary with all addresses of gothic-objects in this world.
        Dictionary<int, BaseVob> vobAddr = new Dictionary<int, BaseVob>();

        public bool TryGetVobByAddress(int address, out BaseVob vob)
        {
            return vobAddr.TryGetValue(address, out vob);
        }

        public bool TryGetVobByAddress<T>(int address, out T vob) where T : BaseVob
        {
            BaseVob v;
            if (vobAddr.TryGetValue(address, out v))
            {
                if (v is T)
                {
                    vob = (T)v;
                    return true;
                }
            }
            vob = null;
            return false;
        }

        #endregion

        #region Add & Remove

        partial void pAfterAddVob(BaseVob vob)
        {
            // add the vob to the gothic world
            gWorld.AddVob(vob.gVob);

            // add the gothic-object's address to the dictionary
            vobAddr.Add(vob.gVob.Address, vob);
        }

        partial void pBeforeRemoveVob(BaseVob vob)
        {
            var gVob = vob.gVob;

            // update position & direction one last time
            //vob.UpdateOrientation();
            //vob.UpdateEnvironment();

            // remove gothic-object from the gothic-world
            gWorld.RemoveVob(gVob);

            // remove the gothic-object's address from the dictionary
            vobAddr.Remove(gVob.Address);
        }

        #endregion
    }
}
