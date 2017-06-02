using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Enumeration;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public partial class ChangeInit_World : BaseChangeInit
    {

        new public static readonly string _staticName = "ChangeInit_World (s)";

        new public static ChangeInit_World representative;



        static ChangeInit_World ()
        {
            representative = new ChangeInit_World();
            representative.SetObjName("ChangeInit_World");
        }



        public ChangeInit_World ()
            : base()
        {
            // add all types of changes and their corresponding parameter types

            AddOrChange(new ChangeInitInfo(ChangeType.World_Clock_IsRunning_Set, new List<Type>()
            {
                typeof(bool),                 // world clock ticking or not (standing still)
            }, null));

            AddOrChange(new ChangeInitInfo(ChangeType.World_Clock_Rate_Set, new List<Type>()
            {
                //typeof(bool),                 // world clock ticking or not (standing still)
            }, null));

            AddOrChange(new ChangeInitInfo(ChangeType.World_Clock_Time_Set, new List<Type>()
            {
                //typeof(bool),                 // world clock ticking or not (standing still)
            }, null));
        }

    }

}
