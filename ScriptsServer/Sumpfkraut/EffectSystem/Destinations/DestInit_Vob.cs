using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public class DestInit_Vob : BaseDestInit
    {

        new public static readonly string _staticName = "DestInit_Vob (static)";
        new public static DestInit_Vob representative;



        // make sure, the destination makes itself known to its related changes
        static DestInit_Vob ()
        {
            representative = new DestInit_Vob();
        }

        protected DestInit_Vob ()
        {
            SetObjName("DestInit_Vob");

            AddOrChange(ChangeDestination.Effect_Name, new List<ChangeType>() { ChangeType.Vob_CodeName_Set },
                CTC_CodeName, ATC_CodeName);
        }

        public void CTC_CodeName (BaseEffectHandler effectHandler)
        {

        }

        public void ATC_CodeName (BaseEffectHandler effectHandler)
        {

        }

    }

}
