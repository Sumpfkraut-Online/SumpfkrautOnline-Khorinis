namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_ReplaceRangel : IManagerInteraction
    {

        public TimedFunction[] OldTF;
        public TimedFunction[] NewTF;
        public bool ReplaceAll;



        public MI_ReplaceRangel (TimedFunction[] oldTF, TimedFunction[] newTF, bool replaceAll)
        {
            OldTF = oldTF;
            NewTF = newTF;
            ReplaceAll = replaceAll;
        }

    }

}
