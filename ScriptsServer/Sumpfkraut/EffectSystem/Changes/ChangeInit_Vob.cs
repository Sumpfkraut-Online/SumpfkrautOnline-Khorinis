using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class ChangeInit_Vob : BaseChangeInit
    {

        new public static readonly string _staticName = "ChangeInit_Vob (static)";

        new public static ChangeInit_Vob representative;



        static ChangeInit_Vob ()
        {
            representative = new ChangeInit_Vob();
            representative.SetObjName("ChangeInit_Vob");
        }



        public ChangeInit_Vob ()
            : base()
        {
            // add all types of changes and their corresponding parameter types

            AddOrChange(new ChangeInitInfo(ChangeType.Vob_CodeName_Set, new List<Type>()
            {
                typeof(string),
            }, null));
        }

    }

}
