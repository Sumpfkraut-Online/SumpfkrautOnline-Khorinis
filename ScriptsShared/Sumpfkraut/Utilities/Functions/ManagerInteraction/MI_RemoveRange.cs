namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_RemoveRange : IManagerInteraction
    {

        public TimedFunction[] TF;
        public bool RemoveAll;



        public MI_RemoveRange (TimedFunction[] tf, bool removeAll)
        {
            TF = tf;
            RemoveAll = removeAll;
        }

    }

}
