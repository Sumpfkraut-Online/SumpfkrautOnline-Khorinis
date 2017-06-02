using System;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_RemoveExceptTimeRange : IManagerInteraction
    {

        public DateTime Start;
        public DateTime End;



        public MI_RemoveExceptTimeRange (DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

    }

}
