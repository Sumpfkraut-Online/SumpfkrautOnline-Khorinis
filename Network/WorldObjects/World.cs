using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;
using GUC.Log;

namespace GUC.WorldObjects
{
    public partial class World : WorldObject
    {
        public partial interface IScriptWorld : IScriptWorldObject
        {
        }

        static Dictionary<string, World> sWorldDict = new Dictionary<string, World>();
        public static World GetWorld(string worldName) { World world; sWorldDict.TryGetValue(worldName.ToUpper(), out world); return world; }
        public static IEnumerable<World> GetWorlds() { return sWorldDict.Values; }
        public static int GetWorldCount() { return sWorldDict.Count; }

        public readonly string WorldName;
        public readonly string FileName;
        public readonly VobCollection Vobs = new VobCollection();

        public readonly VobDictionary Npcs;
        public readonly VobDictionary Items;
        public readonly VobDictionary MobInters;

        #region Creation

        public World(string worldName, string fileName, IScriptWorld scriptObj) : base(scriptObj)
        {
            Npcs = this.Vobs.GetDict(NPC.sVobType);
            Items = this.Vobs.GetDict(Item.sVobType);
            MobInters = this.Vobs.GetDict(MobInter.sVobType);

            this.WorldName = worldName.Trim().ToUpper();
            this.FileName = fileName.Trim().ToUpper();
        }

        public override void Create()
        {
            if (String.IsNullOrWhiteSpace(this.WorldName))
            {
                throw new Exception("World creation failed: WorldName is null or empty.");
            }

            if (String.IsNullOrWhiteSpace(this.FileName))
            {
                throw new Exception("World creation failed: FileName is null or empty.");
            }

            base.Create();

            World.sWorldDict.Add(this.WorldName, this);
        }

        public override void Delete()
        {
            base.Delete();

            foreach (Vob vob in Vobs.GetAll())
            {
                vob.Delete();
            }

            World.sWorldDict.Remove(this.WorldName);
        }

        #endregion

        #region Spawn

        partial void pSpawnVob(Vob vob);
        public void SpawnVob(Vob vob)
        {
            if (!vob.IsCreated)
            {
                Logger.LogWarning("Vobs need to be created before they can spawn!");
                return;
            }

            if (vob.IsSpawned)
            {
                if (vob.World != this)
                {
                    vob.Despawn();
                }
            }
            else
            {
                vob.World = this;
                Vobs.Add(vob);
            }

            pSpawnVob(vob);
        }

        partial void pDespawnVob(Vob vob);
        public void DespawnVob(Vob vob)
        {
            if (vob.World != this)
                return;

            vob.World = null;
            Vobs.Remove(vob);

            pDespawnVob(vob);
        }

        #endregion
    }
}
