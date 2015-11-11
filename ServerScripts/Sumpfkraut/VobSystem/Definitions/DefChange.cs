using GUC.Server.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    class DefChange : ScriptObject
    {

        public readonly Dictionary<String, SQLiteGetTypeEnum> defTab_GetTypeByColumn =
            new Dictionary<String, SQLiteGetTypeEnum>
            {
                {"DefChangeId",             SQLiteGetTypeEnum.GetInt32},
                {"ChangeDate",              SQLiteGetTypeEnum.GetString},
                {"CreationDate",            SQLiteGetTypeEnum.GetString},
            };

    }
}
