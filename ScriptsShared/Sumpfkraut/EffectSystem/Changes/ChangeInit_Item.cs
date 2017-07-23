using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Enumeration;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public partial class ChangeInit_Item : BaseChangeInit
    {

        new public static ChangeInit_Item representative;



        static ChangeInit_Item ()
        {
            representative = new ChangeInit_Item();
        }



        public ChangeInit_Item ()
            : base()
        {
            // add all types of changes and their corresponding parameter types

            AddOrChange(new ChangeInitInfo(ChangeType.Item_Material_Set, new List<Type>()
            {
                typeof(string),                 // material-name
            }, null));
        }

    }

}
