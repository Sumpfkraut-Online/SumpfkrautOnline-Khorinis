using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;

namespace GUC.Scripts.Sumpfkraut
{
    public abstract class ScriptObject
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
                    Logger.Log(output);
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while creating log: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                Logger.LogError(errMsg);
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
                    Logger.Log(output);
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while creating error-log: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                Logger.LogError(errMsg);
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
                    Logger.Log(output);
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while creating warning-log: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                Logger.LogError(errMsg);
            }
        }
        
        public static void PrintStatic (Type type, Object obj)
        {
            PrintStatic(type, obj, false);
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
                        Logger.Print(output + "<br>");
                        //Console.WriteLine(output);
                    }
                    else
                    {
                        Logger.Print(output);
                        //Console.Write(output);
                    }
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while printing to console: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                Logger.LogError(errMsg);
            }
        }

        public void MakeLog ()
        { }
        
        public void MakeLog (Object obj)
        {
            if (obj != null)
            {
                String output = _objName + ": " + obj.ToString();
                Logger.Log(output);
            }
        }

        public void MakeLogError (Object obj)
        {
            if (obj != null)
            {
                String output = _objName + ": " + obj.ToString();
                Logger.LogError(output);
            }
        }

        public void MakeLogWarning (Object obj)
        {
            if (obj != null)
            {
                String output = _objName + ": " + obj.ToString();
                Logger.LogWarning(output);
            }
        }
        
        public void Print ()
        { }

        public void Print (Object obj)
        {
            Print(obj, false);
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
                    Logger.Print(output + "<br>");
                    //Console.WriteLine(output);
                }
                else
                {
                    Logger.Print(output);
                    //Console.Write(output);
                }
            }
        }

        #endregion

    }
}
