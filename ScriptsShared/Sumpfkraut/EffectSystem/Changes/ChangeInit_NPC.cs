using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Enumeration;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class ChangeInit_NPC : BaseChangeInit
    {

        new public static ChangeInit_NPC representative;



        static ChangeInit_NPC ()
        {
            representative = new ChangeInit_NPC();
        }



        public ChangeInit_NPC ()
            : base()
        {
            // add all types of changes and their corresponding parameter types

            //AddOrChange(new ChangeInitInfo(ChangeType.Vob_CDDyn_Set, new List<Type>()
            //{
            //    typeof(bool),                   // active or not
            //}, null));

        }

    }

}
