#if SSM_WEB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Types;

namespace GUC.Server.Scripts.Web.Actions
{
    public class GetPlayerListAction: Action
    {
        public class NPCCOPYED
        {
            public String name;
            public bool IsPlayer = false;
            public Vec3f position;

        }
        public NPCCOPYED[] list;

        public NPCCOPYED[] List
        {
            get
            { 
            if(!this.IsFinished)
                throw new FieldAccessException("Task has to be finished!");
            return list;
        }}

        public override void update(ActionTimer timer)
        {
            NPC[] _list = World.getWorld("NEWWORLD\\NEWWORLD.ZEN").getNPCList();
            list = new NPCCOPYED[_list.Length];

            for ( int i = 0; i < list.Length; i++)
            {
                list[i] = new NPCCOPYED() { name = _list[i].Name, position = new Vec3f(_list[i].Position), IsPlayer = (_list[i] is Player) ? true : false };
            }


            isFinished = true;
            timer.removeAction(this);
        }
    }
}

#endif