using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public abstract class BaseChangeInit : ExtendedObject
    {

        new public static readonly string _staticName = "BaseChangeInit (static)";
        // static representative of the class (do not change it in any way after instantiation!)
        public static BaseChangeInit representative;
        // will be filled up automatically by EffectHandlers due to information from initialized Destinations
        public static List<List<ChangeDestination>> influencedDestinations = new List<List<ChangeDestination>>();

        protected List<ChangeType> includedChangeTypes;
        protected List<List<Type>> parameterTypeLists;
        public List<List<Type>> GetParameterTypeLists () { return parameterTypeLists; }
        public List<Type> GetParameterTypes (int index)
        {
            if ((index < 0) || (index >= parameterTypeLists.Count)) { return null; }
            return parameterTypeLists[index];
        }

        public BaseChangeInit ()
        {
            if (includedChangeTypes == null)
            {
                includedChangeTypes = new List<ChangeType>();
                MakeLogWarning("Missing includedChangeTypes in subsclass-constructor of BaseChangeInit!");
            }
            if (parameterTypeLists == null)
            {
                parameterTypeLists = new List<List<Type>>();
                MakeLogWarning("Missing parameterTypeLists in subsclass-constructor of BaseChangeInit!");
            }
        }

    }

}
