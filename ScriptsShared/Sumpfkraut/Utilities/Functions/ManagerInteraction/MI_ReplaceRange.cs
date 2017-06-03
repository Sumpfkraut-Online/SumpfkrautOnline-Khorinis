namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_ReplaceRange : IManagerInteraction
    {

        public TimedFunction[] OldTF;
        public TimedFunction[] NewTF;
        public bool ReplaceAll;



        public MI_ReplaceRange (TimedFunction[] oldTF, TimedFunction[] newTF, bool replaceAll)
        {
            OldTF = oldTF;
            NewTF = newTF;
            ReplaceAll = replaceAll;
        }

    }

}
