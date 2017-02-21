using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public struct EffectLoadInfo<C>
    {

        public int id;
        public List<int> componentIDs;
        public List<int> componentIndices;
        public List<C> components;



        public EffectLoadInfo (int id)
        {
            this.id = id;
            this.componentIDs = new List<int>();
            this.componentIndices = new List<int>();
            this.components = new List<C>();
        }



        public int AddComponent (int componentID, C component)
        {
            componentIDs.Add(componentID);
            components.Add(component);
            return components.Count;
        }

        public int RemoveComponent (int componentID)
        {
            int index = componentIDs.IndexOf(componentID);
            if (index > -1)
            {
                componentIDs.RemoveAt(index);
                components.RemoveAt(index);
            }
            return index;
        }

        public int RemoveComponent (C component)
        {
            int index = components.IndexOf(component);
            if (index > -1)
            {
                componentIDs.RemoveAt(index);
                components.RemoveAt(index);
            }
            return index;
        }


    }

}
