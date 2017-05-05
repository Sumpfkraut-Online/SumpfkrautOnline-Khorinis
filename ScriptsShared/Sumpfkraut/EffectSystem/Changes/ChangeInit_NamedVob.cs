using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Enumeration;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public partial class ChangeInit_NamedVob : BaseChangeInit
    {

        new public static readonly string _staticName = "ChangeInit_NamedVob (s)";

        new public static ChangeInit_NamedVob representative;



        static ChangeInit_NamedVob ()
        {
            representative = new ChangeInit_NamedVob();
            representative.SetObjName("ChangeInit_Vob");
        }



        public ChangeInit_NamedVob ()
            : base()
        {
            // add all types of changes and their corresponding parameter types

            AddOrChange(new ChangeInitInfo(ChangeType.Vob_Name_Set, new List<Type>()
            {
                typeof(string),                 // vob-name
            }, null));
        }

    }

}
