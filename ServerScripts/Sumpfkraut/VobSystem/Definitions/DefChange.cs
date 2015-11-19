using GUC.Server.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    class DefChange : ScriptObject
    {

        new public static readonly String _staticName = "DefChange (static)";
        new protected String _objName = "DefChange (default)";

        public static readonly Dictionary<String, SQLiteGetTypeEnum> defTab_GetTypeByColumn =
            new Dictionary<String, SQLiteGetTypeEnum>
            {
                {"DefChangeId",             SQLiteGetTypeEnum.GetInt32},
                {"ChangeDate",              SQLiteGetTypeEnum.GetString},
                {"CreationDate",            SQLiteGetTypeEnum.GetString},
            };

    }
}
