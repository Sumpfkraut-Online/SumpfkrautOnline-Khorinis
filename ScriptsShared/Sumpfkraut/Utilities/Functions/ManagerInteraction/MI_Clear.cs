using System;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_Clear : IManagerInteraction
    {

        public DateTime AtTime;



        public MI_Clear (DateTime atTime)
        {
            AtTime = atTime;
        }

    }

}
