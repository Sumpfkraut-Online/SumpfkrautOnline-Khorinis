using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class BaseChangeInit : ExtendedObject
    {

        new public static readonly string _staticName = "BaseChangeInit (static)";
        // static representative of the class (do not change it in any way after instantiation!)
        public static BaseChangeInit representative;

        protected List<BaseChangeInit> parents;
        protected List<BaseChangeInit> children;
        protected List<ChangeType> includedChangeTypes;
        public List<ChangeType> GetIncludedChangeTypes () { return includedChangeTypes; }
        protected List<List<Type>> parameterTypeLists;
        public List<List<Type>> GetParameterTypeLists () { return parameterTypeLists; }
        public List<Type> GetParameterTypes (int index)
        {
            if ((index < 0) || (index >= parameterTypeLists.Count)) { return null; }
            return parameterTypeLists[index];
        }

        // will be filled up automatically by EffectHandlers due to information from initialized Destinations
        protected List<List<ChangeDestination>> influencedDestinationLists;
        public List<List<ChangeDestination>> GetInfluencedDestinationLists () { return influencedDestinationLists; }



        static BaseChangeInit ()
        {
            // in child classes use Inherit(parent) to inherit the parameterTypeLists and their respective types
            // for all supported / listes types of changes

            // always create own representative in inheriting classes
            //representative = new BaseChangeInit();
        }



        protected BaseChangeInit ()
        {
            // ensure initialization for basic, necessary fields
            influencedDestinationLists = new List<List<ChangeDestination>>();

            // if not defined at all, at least provide dummies for these important listing
            if (includedChangeTypes == null)
            {
                MakeLogWarning("Missing includedChangeTypes in subsclass-constructor of BaseChangeInit!");
                includedChangeTypes = new List<ChangeType>();
            }
            if (parameterTypeLists == null)
            {
                MakeLogWarning("Missing parameterTypeLists in subsclass-constructor of BaseChangeInit!");
                parameterTypeLists = new List<List<Type>>();
            }

            // both listings must have same length to work properly
            if (includedChangeTypes.Count > parameterTypeLists.Count)
            {
                MakeLogWarning("includedChangeTypes has more entries than parameterTypeLists!"
                    + " Filling in missing entries in paramterTypeLists with new List<Type>.");
                int fill = includedChangeTypes.Count - parameterTypeLists.Count;
                int i;
                for (i = 0; i < fill; i++)
                {
                    parameterTypeLists.Add(new List<Type>());
                }
            }
            else if (parameterTypeLists.Count > includedChangeTypes.Count)
            {
                MakeLogWarning("parameterTypeLists has more entries than includedChangeTypes!"
                    + " Removing unnecessary entries which still might lead to runtime errors.");
                int rem = parameterTypeLists.Count - includedChangeTypes.Count;
                parameterTypeLists.RemoveRange(includedChangeTypes.Count - 1, rem);
            }
        }



        // add or change existing included type of change and its respective parametertypes
        protected void AddOrChange (ChangeType changeType, List<Type> parameterTypes)
        {
            int index = includedChangeTypes.IndexOf(changeType);
            if (index < 0)
            {
                includedChangeTypes.Add(changeType);
                parameterTypeLists.Add(parameterTypes);
            }
            else
            {
                includedChangeTypes[index] = changeType;
                parameterTypeLists[index] = parameterTypes;
            }
        }
        
        //// let calling object inherit change and respective parameter types from a parent
        //protected void Inherit (BaseChangeInit parent)
        //{
        //    List<ChangeType> ict = parent.GetIncludedChangeTypes();
        //    List<List<Type>> ptl = parent.GetParameterTypeLists();

        //    // inherit all change types and their paramter types if they are no duplicate
        //    int i;
        //    for (i = 0; i < ict.Count; i++)
        //    {
        //        if (!includedChangeTypes.Contains(ict[i]))
        //        {
        //            includedChangeTypes.Add(ict[i]);
        //            parameterTypeLists.Add(ptl[i]);
        //        }
        //    }
        //}

    }

}
