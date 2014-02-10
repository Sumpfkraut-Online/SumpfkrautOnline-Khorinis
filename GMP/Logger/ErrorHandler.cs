using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GMP.Logger
{
    public class ErrorHandler
    {

        public static void ErrorMessage(GMP.Logger.ErrorLog.LogLevel ll, Type t, String message, int id)
        {
            MessageBox.Show(t.FullName + " | " + message + " | " + id);
        }
    }
}
