using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Utilities
{
    public static class ExceptionCheck
    {
        public static void ArgumentNull<T>(T obj, string name = null) where T : class
        {
            if (obj == null)
            {
                string message;
                if (name == null)
                    message = string.Format("Argument of type {0} is null!", typeof(T));
                else
                    message = string.Format("Argument '{0}' of type {1} is null!", name, typeof(T));

                throw new ArgumentNullException(name, message);
            }
        }
    }
}
