namespace GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction
{

    public struct MI_AddRange : IManagerInteraction
    {

        public TimedFunction[] TF;
        public bool AllowDuplicate;



        public MI_AddRange (TimedFunction[] tf, bool allowDuplicate)
        {
            TF = tf;
            AllowDuplicate = allowDuplicate;
        }

    }

}
