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

}
