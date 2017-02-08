using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class ChangeInit_Effect : BaseChangeInit
    {

        new public static readonly string _staticName = "ChangeInit_Effect (static)";
        new public static ChangeInit_Effect representative;



        static ChangeInit_Effect ()
        {
            representative = new ChangeInit_Effect();
        }



        public ChangeInit_Effect ()
            : base()
        {
            SetObjName("ChangeInit_Effect");

            includedChangeTypes = new List<ChangeType>();
            parameterTypeLists = new List<List<Type>>();

            // add all types of changes and their corresponding parameter types

            AddOrChange(new ChangeInitInfo(ChangeType.Effect_Name_Set, new List<Type>()
            {
                typeof(string),
            }, null));
        }

    }

}
