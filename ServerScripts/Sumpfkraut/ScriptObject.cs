using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut
{
    public abstract class ScriptObject : IScriptObject
    {

        #region attributes

        protected String _objName = "ScriptObject (default)";
        public virtual String getObjName ()
        {
            return this._objName;
        }
        public virtual void SetObjName (String objName)
        {
            this._objName = objName;
        }

        protected static string _staticName = "ScriptObject (static)";

        #endregion



        #region constructors

        public ScriptObject ()
        { }

        public ScriptObject (String objName)
        {
            this._objName = objName;
        }

        #endregion



        #region print- and log-methods

        public static void MakeLogStatic ()
        { }

        public void MakeLogStatic (Object obj)
        {
            if (obj != null)
            {
                String output = _staticName + ": " + obj.ToString();
                Log.Logger.log(output);
            }
        }

        public void MakeLogErrorStatic (Object obj)
        {
            if (obj != null)
            {
                String output = _staticName + ": " + obj.ToString();
                Log.Logger.logError(output);
            }
        }

        public void MakeLogWarningStatic (Object obj)
        {
            if (obj != null)
            {
                String output = _staticName + ": " + obj.ToString();
                Log.Logger.logWarning(output);
            }
        }
        
        public void PrintStatic ()
        { }

        public void PrintStatic (Object obj)
        {
            Print(obj, true);
        }

        public void PrintStatic (Object obj, bool newLine)
        {
            if (obj == null)
            {
                return;
            }
            else
            {
                String output = _staticName + ": " + obj.ToString();
                if (newLine)
                {
                    Console.WriteLine(output);
                }
                else
                {
                    Console.Write(output);
                }
            }
        }

        public void MakeLog ()
        { }
        
        public void MakeLog (Object obj)
        {
            if (obj != null)
            {
                String output = _objName + ": " + obj.ToString();
                Log.Logger.log(output);
            }
        }

        public void MakeLogError (Object obj)
        {
            if (obj != null)
            {
                String output = _objName + ": " + obj.ToString();
                Log.Logger.logError(output);
            }
        }

        public void MakeLogWarning (Object obj)
        {
            if (obj != null)
            {
                String output = _objName + ": " + obj.ToString();
                Log.Logger.logWarning(output);
            }
        }
        
        public void Print ()
        { }

        public void Print (Object obj)
        {
            Print(obj, true);
        }

        public void Print (Object obj, bool newLine)
        {
            if (obj == null)
            {
                return;
            }
            else
            {
                String output = _objName + ": " + obj.ToString();
                if (newLine)
                {
                    Console.WriteLine(output);
                }
                else
                {
                    Console.Write(output);
                }
            }
        }

        #endregion

    }
}
