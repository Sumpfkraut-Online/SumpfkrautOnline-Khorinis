namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_Replace : IManagerInteraction
    {

        public TimedFunction OldTF;
        public TimedFunction NewTF;
        public bool ReplaceAll;

        public bool HasAmount { get { return Amount >= 0; } }
        private int amount;
        public int Amount
        {
            get { return amount; }
            set { if (value >= 0) { amount = value; } }
        }



        public MI_Replace (TimedFunction oldTF, TimedFunction newTF, bool replaceAll)
            : this(oldTF, newTF, replaceAll, -1)
        { }

        public MI_Replace (TimedFunction oldTF, TimedFunction newTF, bool replaceAll, int amount)
        {
            OldTF = oldTF;
            NewTF = newTF;
            ReplaceAll = replaceAll;
            this.amount = (amount >= 0) ? amount : -1;
        }

    }

}
