using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Database
{
    
    public enum DBReaderMode
    {
        undefined = 0,
        loadData = undefined + 1,
        saveData = loadData + 1,
    }

    /**
    *   Enum of the supported SQLite-Get-Types, used internally 
    */
    public enum SQLiteGetTypeEnum
    {
        GetBoolean          = 0,
        GetByte             = 1,
        GetChar             = 2,
        GetDateTime         = 3,
        GetDecimal          = 4,
        GetDouble           = 5,
        GetFloat            = 6,
        GetGuid             = 7,
        GetInt16            = 8,
        GetInt32            = 9,
        GetInt64            = 10,
        GetString           = 11,
    }

}
