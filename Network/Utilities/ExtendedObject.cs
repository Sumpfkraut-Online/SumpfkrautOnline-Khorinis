using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Utilities
{

    public abstract partial class ExtendedObject
    { 

        #region attributes

        public static readonly String _staticName = "ExtendedObject (static)";

        protected String _objName;
        public virtual String GetObjName ()
        {
            return this._objName;
        }
        public virtual void SetObjName (String objName)
        {
            this._objName = objName;
        }

        #endregion



        #region constructors

        public ExtendedObject ()
        {
            SetObjName("ExtendedObject (default)");
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
                    //String output = staticName + ": " + obj.ToString();
                    //Log.Logger.log(output);
                    ToOutputController(1, BuildMessage(new object[] { true }, staticName, obj));
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while creating log: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                //Log.Logger.logError(errMsg);
                ToOutputController(3, errMsg);
            }
        }

        public static void MakeLogErrorStatic (Type type, Object obj)
        {
            try
            {
                String staticName = type.GetField("_staticName").GetValue("").ToString();
                if (obj != null)
                {
                    //String output = staticName + ": " + obj.ToString();
                    //Log.Logger.log(output);
                    ToOutputController(3, BuildMessage(new object[] { true }, staticName, obj));
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while creating error-log: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                //Log.Logger.logError(errMsg);
                ToOutputController(3, errMsg);
            }
        }

        public static void MakeLogWarningStatic (Type type, Object obj)
        {
            try
            {
                String staticName = type.GetField("_staticName").GetValue("").ToString();
                if (obj != null)
                {
                    //String output = staticName + ": " + obj.ToString();
                    //Log.Logger.log(output);
                    ToOutputController(2, BuildMessage(new object[] { true }, staticName, obj));
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while creating warning-log: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                //Log.Logger.logError(errMsg);
                ToOutputController(3, errMsg);
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
                    //String output = staticName + ": " + obj.ToString();
                    if (newLine)
                    {
                        ToOutputController(0, BuildMessage(new object[] { true }, staticName, obj));
                    }
                    else
                    {
                        ToOutputController(0, BuildMessage(null, staticName, obj));
                    }
                }
            }
            catch (Exception e)
            {
                String errMsg = String.Format(
                    "Couldn't find field _staticName while printing to console: {0} || ERROR: {1}",
                    obj.ToString(), e.ToString());
                ToOutputController(3, errMsg);
            }
        }

        public void MakeLog ()
        { }
        
        public void MakeLog (Object obj)
        {
            if (obj != null)
            {
                //String output = _objName + ": " + obj.ToString();
                //Log.Logger.log(output);
                ToOutputController(1, BuildMessage(new object[] { true }, GetObjName(), obj));
            }
        }

        public void MakeLogError (Object obj)
        {
            if (obj != null)
            {
                ToOutputController(3, BuildMessage(new object[] { true }, GetObjName(), obj));
            }
        }

        public void MakeLogWarning (Object obj)
        {
            if (obj != null)
            {
                ToOutputController(2, BuildMessage(new object[] { true }, GetObjName(), obj));
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
                    ToOutputController(0, BuildMessage(new object[] { true }, GetObjName(), obj));
                }
                else
                {
                    ToOutputController(0, BuildMessage(null, GetObjName(), obj));
                }
            }
        }

        #endregion



        // used to build the final output string
        public static String BuildMessage (object[] options, params object[] args)
        {
            String msg = "";
            BuildMessage(ref msg, options, args);
            return msg;
        }

        static partial void BuildMessage (ref String msg, object[] options, params object[] args);

        // defines the way to reach the desired output controller where messages are printed to
        // must be completed where this class is needed
        static partial void ToOutputController (int msgType, String msg, params object[] args);

    }

}
