﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GUC.Scripts.Sumpfkraut.Database
{
    public partial class DBSecurity : GUC.Utilities.ExtendedObject
    {

        // search patterns to filter out malicious code in sql-commands to prevent sql-injection, etc.
        // according to http://www.symantec.com/connect/articles/detection-sql-injection-and-cross-site-scripting-attacks 
        private static string pattern_sqlMeta_1 = @"/(\%27)|(\')|(\-\-)|(\%23)|(#)/ix";
        private static Regex rgx_sqlMeta_1 = new Regex(pattern_sqlMeta_1);
        private static string pattern_sqlMeta_2 = @"/((\%3D)|(=))[^\n]*((\%27)|(\')|(\-\-)|(\%3B)|(;))/i";
        private static Regex rgx_sqlMeta_2 = new Regex(pattern_sqlMeta_2);
        private static string pattern_sqlMeta_3 = @"/\w*((\%27)|(\'))((\%6F)|o|(\%4F))((\%72)|r|(\%52))/ix";
        private static Regex rgx_sqlMeta_3 = new Regex(pattern_sqlMeta_3);



        public static bool IsSecureSQLCommand (string cmd)
        {
            // detect malicious sql-code
            if (rgx_sqlMeta_1.Match(cmd).Length > 0)
            {
                MakeLogWarningStatic(typeof(DBSecurity), string.Format(
                    "IsSecureSQLCommand: Detected malicious sql-command (pattern {0}): {1}{2}{1}In:{1}{3}", 
                    1, Environment.NewLine, rgx_sqlMeta_1.Match(cmd).Value, cmd));
                return false;
            }
            else if (rgx_sqlMeta_2.Match(cmd).Length > 0)
            {
                MakeLogWarningStatic(typeof(DBSecurity), string.Format(
                    "IsSecureSQLCommand: Detected malicious sql-command (pattern {0}): {1}{2}{1}In:{1}{3}", 
                    2, Environment.NewLine, rgx_sqlMeta_2.Match(cmd).Value, cmd));
                return false;
            }
            else if (rgx_sqlMeta_3.Match(cmd).Length > 0)
            {
                MakeLogWarningStatic(typeof(DBSecurity), string.Format(
                    "IsSecureSQLCommand: Detected malicious sql-command (pattern {0}): {1}{2}{1}In:{1}{3}", 
                    3, Environment.NewLine, rgx_sqlMeta_3.Match(cmd).Value, cmd));
                return false;
            }

            return true;
        }

    }
}
