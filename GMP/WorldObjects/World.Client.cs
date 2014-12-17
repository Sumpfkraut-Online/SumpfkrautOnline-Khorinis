using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Types;
using GUC.Network;

namespace GUC.WorldObjects
{
    internal partial class World
    {
        public void Read(BitStream stream)
        {
            stream.Read(out this.mapName);

            int ilc = 0;
            stream.Read(out ilc);
            for (int i = 0; i < ilc; i++)
            {
                int id;
                Vec3f pos, dir;
                stream.Read(out id);
                stream.Read(out pos);
                stream.Read(out dir);

                Vob v = sWorld.VobDict[id];
                v.Position = pos;
                v.Direction = dir;
                addVob(v);

                v.Spawn(this.Map, v.Position, v.Direction);
            }

            int vlc = 0;
            stream.Read(out vlc);
            for (int i = 0; i < vlc; i++)
            {
                int id;
                Vec3f pos, dir;
                stream.Read(out id);
                stream.Read(out pos);
                stream.Read(out dir);

                Vob v = sWorld.VobDict[id];
                v.Position = pos;
                v.Direction = dir;
                addVob(v);

                v.Spawn(this.Map, v.Position, v.Direction);
            }

            int nlc = 0;
            stream.Read(out nlc);
            for (int i = 0; i < nlc; i++)
            {
                int id;
                Vec3f pos, dir;
                stream.Read(out id);
                stream.Read(out pos);
                stream.Read(out dir);

                if (id == Player.Hero.ID)
                    continue;

                Vob v = sWorld.VobDict[id];
                v.Position = pos;
                v.Direction = dir;
                addVob(v);

                v.Spawn(this.Map, v.Position, v.Direction);
                //((NPCProto)v).Disable();
            }
        }



        public void SpawnWorld()
        {
            foreach (Item item in this.itemList)
            {
                if (!item.IsSpawned)
                    continue;
                item.Spawn(item.Map, item.Position, item.Direction);
            }

            foreach (Vob vob in this.VobList)
            {
                if (!vob.IsSpawned)
                    continue;
                vob.Spawn(vob.Map, vob.Position, vob.Direction);
            }


            foreach (NPCProto proto in this.NPCList)
            {
                if (proto.ID == Player.Hero.ID)
                    continue;
                if (!proto.IsSpawned)
                    continue;

                
                proto.Spawn(proto.Map, proto.Position, proto.Direction);
                //proto.Disable();
            }
        }

        public void DespawnWorld()
        {
            foreach (Item item in this.itemList)
            {
                if (!item.IsSpawned || item.Address == 0)
                    continue;
                sWorld.SpawnedVobDict.Remove(item.Address);
                item.Address = 0;
            }

            foreach (Vob vob in this.VobList)
            {
                if (!vob.IsSpawned || vob.Address == 0)
                    continue;
                sWorld.SpawnedVobDict.Remove(vob.Address);
                vob.Address = 0;
            }


            foreach (NPCProto proto in this.NPCList)
            {
                if (proto.ID == Player.Hero.ID)
                    continue;
                if (!proto.IsSpawned || proto.Address == 0)
                    continue;

                sWorld.SpawnedVobDict.Remove(proto.Address);
                proto.Address = 0;
                foreach (Item it in itemList)
                {
                    sWorld.SpawnedVobDict.Remove(it.Address);
                    it.Address = 0;
                }
            }
        }



        public void RemoveAllObjects()
        {
            Process process = Process.ThisProcess();


            zCWorld zWorld = oCGame.Game(process).World;


            Dictionary<zCVob.VobTypes, List<zCVob>> vobDict = zWorld.getVobLists(zCVob.VobTypes.Item, zCVob.VobTypes.MobInter, zCVob.VobTypes.MobBed, zCVob.VobTypes.MobDoor, zCVob.VobTypes.MobContainer, zCVob.VobTypes.MobSwitch);
            foreach (KeyValuePair<zCVob.VobTypes, List<zCVob>> vobTypePair in vobDict)
            {
                foreach (zCVob vob in vobTypePair.Value)
                {
                    zWorld.RemoveVob(vob);
                }
            }

            List<zCVob> vobList = zWorld.getVobList(zCVob.VobTypes.Npc);

            foreach (zCVob vob in vobList)
            {
                if (vob.Address == oCNpc.Player(process).Address)
                    continue;

                if (sWorld.SpawnedVobDict.ContainsKey(vob.Address))
                {
                    sWorld.SpawnedVobDict[vob.Address].Address = 0;
                    sWorld.SpawnedVobDict.Remove(vob.Address);
                }

                oCGame.Game(process).GetSpawnManager().DeleteNPC(new oCNpc(process, vob.Address));
            }
            
        }
    }
}
