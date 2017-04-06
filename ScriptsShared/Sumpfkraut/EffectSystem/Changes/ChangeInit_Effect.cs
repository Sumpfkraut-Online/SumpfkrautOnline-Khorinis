using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public partial class ChangeInit_Effect : BaseChangeInit
    {

        new public static readonly string _staticName = "ChangeInit_Effect (static)";
        new public static ChangeInit_Effect representative;



        static ChangeInit_Effect ()
        {
            representative = new ChangeInit_Effect();
            representative.SetObjName("ChangeInit_Effect");
        }



        public ChangeInit_Effect ()
            : base()
        {
            // add all types of changes and their corresponding parameter types

            AddOrChange(new ChangeInitInfo(ChangeType.Effect_GlobalID_Set, new List<Type>()
            {
                typeof(string),         // globalID
            }, null));

            AddOrChange(new ChangeInitInfo(ChangeType.Effect_Name_Set, new List<Type>()
            {
                typeof(string),         // name for display in game
            }, null));

            AddOrChange(new ChangeInitInfo(ChangeType.Effect_Parent_Add, new List<Type>()
            {
                typeof(string),         // parent's globalID
            }, null));
        }

    }

}
