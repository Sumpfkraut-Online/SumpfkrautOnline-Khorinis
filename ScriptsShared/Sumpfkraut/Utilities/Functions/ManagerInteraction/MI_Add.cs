namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_Add : IManagerInteraction
    {

        public TimedFunction TF;
        public bool AllowDuplicate;

        

        public MI_Add (TimedFunction tf, bool allowDuplicate)
        {
            TF = tf;
            AllowDuplicate = allowDuplicate;
        }

    }

}
