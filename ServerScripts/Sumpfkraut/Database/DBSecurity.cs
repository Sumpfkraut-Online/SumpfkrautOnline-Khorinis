using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GUC.Server.Scripts.Sumpfkraut.Database
{
    class DBSecurity
    {

       new public static readonly String _staticName = "DBSecurity (static)";
        new protected String _objName = "DBSecurity (default)";

        // according to http://www.symantec.com/connect/articles/detection-sql-injection-and-cross-site-scripting-attacks 
        private static string pattern_sqlMeta_1 = @"/(\%27)|(\')|(\-\-)|(\%23)|(#)/ix";
        private static Regex rgx_sqlMeta_1 = new Regex(pattern_sqlMeta_1);
        private static string pattern_sqlMeta_2 = @"/((\%3D)|(=))[^\n]*((\%27)|(\')|(\-\-)|(\%3B)|(;))/i";
        private static Regex rgx_sqlMeta_2 = new Regex(pattern_sqlMeta_2);
        private static string pattern_sqlMeta_3 = @"/\w*((\%27)|(\'))((\%6F)|o|(\%4F))((\%72)|r|(\%52))/ix";
        private static Regex rgx_sqlMeta_3 = new Regex(pattern_sqlMeta_3);

        public static bool IsSecureSQLCommand (string cmd)
        {
            if (rgx_sqlMeta_1.Match(cmd).Length > 0)
            {
                //Log.Logger.logWarning("Detected insecure sql-code with pattern: " + pattern_sqlMeta_1);
                Console.WriteLine("1)" + rgx_sqlMeta_1.Match(cmd).Value);
                return false;
            }
            else if (rgx_sqlMeta_2.Match(cmd).Length > 0)
            {
                //Log.Logger.logWarning("Detected insecure sql-code with pattern: " + pattern_sqlMeta_2);
                Console.WriteLine("2)" + rgx_sqlMeta_1.Match(cmd).Value);
                return false;
            }
            else if (rgx_sqlMeta_3.Match(cmd).Length > 0)
            {
                //Log.Logger.logWarning("Detected insecure sql-code with pattern: " + pattern_sqlMeta_3);
                Console.WriteLine("3)" + rgx_sqlMeta_1.Match(cmd).Value);
                return false;
            }
            return true;
        }

    }
}
