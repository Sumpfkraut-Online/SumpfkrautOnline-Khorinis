using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Utilities
{

    /// <summary>
    /// Basic printing and logging functionality for inheriting classes.
    /// </summary>
    public abstract partial class ExtendedObject
    { 
        #region Properties

        protected string _objName = null;
        public virtual string GetObjName ()
        {
            return this._objName ?? this.GetType().Name;
        }
        public virtual void SetObjName (string objName)
        {
            this._objName = objName;
        }

        #endregion
        
        #region Constructors

        public ExtendedObject ()
        {
        }

        #endregion

        #region print- and log-methods

        public static void MakeLogStatic ()
        { }

        public static void MakeLogStatic (Type type, object obj)
        {
            try
            {
                string staticName = type.Name + " (s)";
                if (obj != null)
                {
                    ToOutputController(1, BuildMessage(new object[] { true }, staticName, obj));
                }
            }
            catch (Exception e)
            {
                ToOutputController(3, e.ToString());
            }
        }

        public static void MakeLogErrorStatic (Type type, object obj)
        {
            try
            {
                string staticName = type.Name + " (s)";
                if (obj != null)
                {
                    ToOutputController(3, BuildMessage(new object[] { true }, staticName, obj));
                }
            }
            catch (Exception e)
            {
                ToOutputController(3, e.ToString());
            }
        }

        public static void MakeLogWarningStatic (Type type, object obj)
        {
            try
            {
                string staticName = type.Name + " (s)";
                if (obj != null)
                {
                    ToOutputController(2, BuildMessage(new object[] { true }, staticName, obj));
                }
            }
            catch (Exception e)
            {
                ToOutputController(3, e.ToString());
            }
        }
        
        public static void PrintStatic (Type type, object obj)
        {
            PrintStatic(type, obj, false);
        }

        public static void PrintStatic (Type type, object obj, bool newLine)
        {
            try
            {
                string staticName = type.Name + " (s)";
                if (obj == null)
                {
                    return;
                }
                else
                {
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
                ToOutputController(3, e.ToString());
            }
        }

        public void MakeLog ()
        { }
        
        public void MakeLog (object obj)
        {
            if (obj != null)
            {
                ToOutputController(1, BuildMessage(new object[] { true }, GetObjName(), obj));
            }
        }

        public void MakeLogError (object obj)
        {
            if (obj != null)
            {
                ToOutputController(3, BuildMessage(new object[] { true }, GetObjName(), obj));
            }
        }

        public void MakeLogWarning (object obj)
        {
            if (obj != null)
            {
                ToOutputController(2, BuildMessage(new object[] { true }, GetObjName(), obj));
            }
        }
        
        public void Print ()
        { }

        public void Print (object obj)
        {
            Print(obj, false);
        }

        public void Print (object obj, bool newLine)
        {
            if (obj == null)
            {
                return;
            }
            else
            {
                string output = _objName + ": " + obj.ToString();
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
        public static string BuildMessage (object[] options, params object[] args)
        {
            string msg = "";
            BuildMessage(ref msg, options, args);
            return msg;
        }

        static partial void BuildMessage (ref string msg, object[] options, params object[] args);

        // defines the way to reach the desired output controller where messages are printed to
        // must be completed where this class is needed
        static partial void ToOutputController (int msgType, string msg, params object[] args);
    }

}
