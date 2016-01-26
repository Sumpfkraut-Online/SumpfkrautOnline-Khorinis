using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class MobDoor : MobLockable
    {
        public partial interface IScriptMobDoor : IScriptMobLockable
        {
        }

        new public const VobTypes sVobType = MobDoorInstance.sVobType;
        public static readonly VobDictionary MobDoors = Vob.AllVobs.GetDict(sVobType);

        new public IScriptMobDoor ScriptObj { get; protected set; }
        new public MobDoorInstance Instance { get; protected set; }

        public MobDoor(MobDoorInstance instance, IScriptMobDoor scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
