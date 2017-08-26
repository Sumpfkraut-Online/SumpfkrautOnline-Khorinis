namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_RemoveRange : IManagerInteraction
    {

        public TimedFunction[] TF;
        public bool RemoveAll;
        
        public bool HasAmount { get { return Amount >= 0; } }
        private int amount;
        public int Amount
        {
            get { return amount; }
            set { if (value >= 0) { amount = value; } }
        }



        public MI_RemoveRange (TimedFunction[] tf, bool removeAll)
            : this(tf, removeAll, -1)
        { }

        public MI_RemoveRange (TimedFunction[] tf, bool removeAll, int amount)
        {
            TF = tf;
            RemoveAll = removeAll;
            this.amount = (amount >= 0) ? amount : -1;
        }

    }

}
