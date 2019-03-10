using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public partial class ChangeInit_Vob : BaseChangeInit
    {

        new public static ChangeInit_Vob representative;



        static ChangeInit_Vob ()
        {
            representative = new ChangeInit_Vob();
        }



        public ChangeInit_Vob ()
            : base()
        {
            // add all types of changes and their corresponding parameter types

            AddOrChange(new ChangeInitInfo(ChangeType.Vob_CDDyn_Set, new List<Type>()
            {
                typeof(bool),                   // active or not
            }, null));

            AddOrChange(new ChangeInitInfo(ChangeType.Vob_CDStatic_Set, new List<Type>()
            {
                typeof(bool),                   // active or not
            }, null));

            AddOrChange(new ChangeInitInfo(ChangeType.Vob_CodeName_Set, new List<Type>()
            {
                typeof(string),                 // codeName
            }, null));

            AddOrChange(new ChangeInitInfo(ChangeType.Vob_VobType_Set, new List<Type>()
            {
                typeof(VobType),                // type of vob
            }, null));
        }

    }

}
