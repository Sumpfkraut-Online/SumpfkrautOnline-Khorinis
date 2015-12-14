namespace GUC.Server.Scripts.Sumpfkraut.Web.WS
{

    public enum WSServerState
    {
        undefined       = 0,
        initialized     = undefined + 1,
        running         = initialized + 1,
        stopped         = running + 1,
        aborted         = stopped + 1,
    }

}