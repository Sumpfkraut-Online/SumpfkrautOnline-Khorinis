using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut
{
    public abstract class ScriptObject : IScriptObject
    {

        protected String objName = "ScriptObject (default)";
        public virtual String getObjName ()
        {
            return this.objName;
        }
        public virtual void SetObjName (String objName)
        {
            this.objName = objName;
        }



        public ScriptObject ()
        { }

        public ScriptObject (String objName)
        {
            this.objName = objName;
        }



        public void MakeLog ()
        { }
        
        public void MakeLog (Object obj)
        {
            if (obj != null)
            {
                String output = objName + ": " + obj.ToString();
                Log.Logger.log(output);
            }
        }

        public void MakeLogError (Object obj)
        {
            if (obj != null)
            {
                String output = objName + ": " + obj.ToString();
                Log.Logger.logError(output);
            }
        }

        public void MakeLogWarning (Object obj)
        {
            if (obj != null)
            {
                String output = objName + ": " + obj.ToString();
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
                String output = this.objName + ": " + obj.ToString();
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

    }
}
