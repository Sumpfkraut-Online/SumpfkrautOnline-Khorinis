namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_ReplaceRange : IManagerInteraction
    {

        public TimedFunction[] OldTF;
        public TimedFunction[] NewTF;
        public bool ReplaceAll;

        public bool HasAmount { get { return Amount >= 0; } }
        private int amount;
        public int Amount
        {
            get { return amount; }
            set { if (value >= 0) { amount = value; } }
        }



        public MI_ReplaceRange (TimedFunction[] oldTF, TimedFunction[] newTF, bool replaceAll)
            : this(oldTF, newTF, replaceAll, -1)
        { }

        public MI_ReplaceRange (TimedFunction[] oldTF, TimedFunction[] newTF, bool replaceAll, int amount)
        {
            OldTF = oldTF;
            NewTF = newTF;
            ReplaceAll = replaceAll;
            this.amount = (amount >= 0) ? amount : -1;
        }

    }

}
