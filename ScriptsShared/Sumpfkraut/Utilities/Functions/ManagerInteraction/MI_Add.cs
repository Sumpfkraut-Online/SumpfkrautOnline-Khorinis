namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_Add : IManagerInteraction
    {

        public TimedFunction TF;
        public int Amount;
        public bool AllowDuplicate;



        public MI_Add (TimedFunction tf, int amount, bool allowDuplicate)
        {
            TF = tf;
            Amount = amount >= 1 ? amount : 1;
            AllowDuplicate = allowDuplicate;
        }

    }

}
