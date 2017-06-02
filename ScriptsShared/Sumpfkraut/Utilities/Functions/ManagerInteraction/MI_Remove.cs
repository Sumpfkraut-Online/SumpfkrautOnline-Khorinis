namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_Remove : IManagerInteraction
    {

        public TimedFunction TF;
        public bool RemoveAll;



        public MI_Remove (TimedFunction tf, bool removeAll)
        {
            TF = tf;
            RemoveAll = removeAll;
        }

    }

}
