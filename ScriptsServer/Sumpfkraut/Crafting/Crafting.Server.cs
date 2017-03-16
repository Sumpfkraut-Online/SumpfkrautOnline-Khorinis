using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.Crafting
{
    class Crafting
    {

        /// <summary>
        /// The key is a representation of a crafting property(CP), the value a dictionary with recipes related to this CP.
        /// </summary>
        Dictionary<int, Dictionary<int, Recipe>> RecipeLists = new Dictionary<int, Dictionary<int, Recipe>>();

        /// <summary>
        /// Prop
        /// </summary>
        Dictionary<string, int> Properties = new Dictionary<string, int>();

        /// <summary>
        /// Each found crafting property will be mapped to an index. This value represents the nr of cp's found so far.
        /// </summary>
        private int nrOfProperties;

        public Crafting()
        {
            nrOfProperties = 0;

            /*NPCInst npc = new NPCInst(NPCDef.Get("maleplayer"));
            Log.Logger.Log("NPC Health was " + npc.GetHealth());
            npc.SetHealth(2000);

            typeof(NPCInst).GetMethod("SetHealth").Invoke(npc, new object[] { 200, 200 } );
            Log.Logger.Log("NPC Health is " + npc.GetHealth()); */
        }


        public void Craft(NPCInst craftsmen, int recipeID, int craftingProperty)
        {
            Dictionary<int, Recipe> RecipeList;
            RecipeLists.TryGetValue(craftingProperty, out RecipeList);
            if(RecipeList == null)
            {
                throw new Exception("Crafting: Couldn't find a RecipeList! s(CP=" + craftingProperty + ")(rID=" + recipeID + ")");
                return;
            }

            Recipe r;
            RecipeList.TryGetValue(recipeID, out r);
            if (r == null)
            {
                throw new Exception("Crafting: Couldn't find a Recipe! (CP=" + craftingProperty + ")(rID=" + recipeID + ")");
                return;
            }

            // control whether player fullfills all conditions needed for the recipe
            if(!r.CheckRequiredConditions(craftsmen))
            {
                // this is a securtiy check in matters of cheaters or error's
                // no feedback needs to be given, but maybe
                // TODO log this?
                return;
            }

            // control whether player got's all of the necessary materials
            if (!r.CheckRequiredMaterials(craftsmen))
            {
                // this is a securtiy check in matters of cheaters or error's
                // no feedback needs to be given, but maybe
                // TODO log this?
                return;
            }

            r.CreateProducts(craftsmen);
            r.ApplyEffects(craftsmen);
        }
        

        /// <summary>
        /// Loads recipes from data base, creates recipe objects and prepares a dictionary for access
        /// </summary>
        public void CreateRecipeLists()
        {
            // create delegate with actions parsed from db strings
        }

    }
}
