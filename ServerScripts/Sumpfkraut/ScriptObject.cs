using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut
{
    public abstract class ScriptObject : IScriptObject
    {

        #region attributes

        public static readonly String _staticName = "ScriptObject (static)";

        protected String _objName;
        public virtual String getObjName ()
        {
            return this._objName;
        }
        public virtual void SetObjName (String objName)
        {
            this._objName = objName;
        }

        #endregion



        #region constructors

        public ScriptObject ()
        {
            SetObjName("ScriptObject (default)");
        }

        #endregion



        #region print- and log-methods

        public static void MakeLogStatic ()
        { }

        public static void MakeLogStatic (Type type, Object obj)
        {
            try
            {
                String staticName = type.GetField("_staticName").GetValue("").ToString();
                if (obj != null)
                {
                    String output = staticName + ": " + obj.ToString();
                    Log.Logger.log(output);
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while creating log: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                Log.Logger.logError(errMsg);
            }
        }

        public static void MakeLogErrorStatic (Type type, Object obj)
        {
            try
            {
                String staticName = type.GetField("_staticName").GetValue("").ToString();
                if (obj != null)
                {
                    String output = staticName + ": " + obj.ToString();
                    Log.Logger.log(output);
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while creating error-log: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                Log.Logger.logError(errMsg);
            }
        }

        public static void MakeLogWarningStatic (Type type, Object obj)
        {
            try
            {
                String staticName = type.GetField("_staticName").GetValue("").ToString();
                if (obj != null)
                {
                    String output = staticName + ": " + obj.ToString();
                    Log.Logger.log(output);
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while creating warning-log: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                Log.Logger.logError(errMsg);
            }
        }
        
        public static void PrintStatic (Type type, Object obj)
        {
            PrintStatic(type, obj, true);
        }

        public static void PrintStatic (Type type, Object obj, bool newLine)
        {
            try
            {
                String staticName = type.GetField("_staticName").GetValue("").ToString();
                if (obj == null)
                {
                    return;
                }
                else
                {
                    String output = staticName + ": " + obj.ToString();
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
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while printing to console: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                Log.Logger.logError(errMsg);
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
