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
        /// The key is a a crafting property(CP), the value a list with recipes craftable with this CP.
        /// </summary>
        Dictionary<int, List<Recipe>> RecipeDict = new Dictionary<int, List<Recipe>>();

        /// <summary>
        /// The strings are crafting property names from the DB and the corresponding integer is the numeric representation for the RecipeDict.
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

        /// <summary>
        /// Prints an information log into server console
        /// </summary>
        public void Info()
        {
            Log.Logger.Log("Crafting currently stores xy Recipes with " + nrOfProperties + " different Crafting Properties");
        }

        public bool Craft(NPCInst craftsmen, int recipeID, int craftingProperty)
        {
            List<Recipe> RecipeList;
            RecipeDict.TryGetValue(craftingProperty, out RecipeList);
            if(RecipeList == null)
            {
                throw new Exception("ERROR: Crafting: Couldn't find a RecipeList for crafting property! (CP=" + craftingProperty + ")(rID=" + recipeID + ")");
                return false;
            }

            Recipe r;
            r = RecipeList.ElementAt(recipeID);
            if (r == null)
            {
                throw new Exception("ERROR: Crafting: Couldn't find a Recipe! (CP=" + craftingProperty + ")(rID=" + recipeID + ")");
                return false;
            }

            /* control whether player fullfills all conditions needed for the recipe
            if(!r.CheckRequiredConditions(craftsmen))
            {
                // also checked client side
                // this is a securtiy check in matters of cheaters or error's
                // no feedback needs to be given, but maybe
                // TODO log this?
                return false;
            }*/

            // control whether player got's all of the necessary materials
            if (!r.CheckRequiredMaterials(craftsmen))
            {
                // also checked client side
                // this is a securtiy check in matters of cheaters or error's
                // no feedback needs to be given, but maybe
                // TODO log this?
                return false;
            }

            // try create products for NPC
            if(!r.CreateProducts(craftsmen))
            {
                Log.Logger.Log("ERROR: Crafting: Couldn't create products!");
                return false;
            }

            // try apply effects on NPC
            if(!r.ApplyEffects(craftsmen))
            {
                Log.Logger.Log("ERROR: Crafting: Couldn't apply Effects!");
                return false;
            }

            return true;
        }
        

        /// <summary>
        /// Loads recipes from data base, creates recipe objects and prepares a dictionary for access
        /// </summary>
        public void CreateRecipeDict()
        {
            // create delegate with actions parsed from db strings
            //while new entry =>
            string craftingProperty = "";
            int craftingProbertyNumber = 0;
            if(!Properties.ContainsKey(craftingProperty))
            {
                // get ID
                craftingProbertyNumber = nrOfProperties;
                // relate craftingproperty string to number
                Properties.Add(craftingProperty, craftingProbertyNumber);
                // create new empty recipe list
                RecipeDict.Add(craftingProbertyNumber, new List<Recipe>());
                // increase for next entry
                nrOfProperties++;

            }
            else
            {
                craftingProbertyNumber = Properties[craftingProperty];
            }
            // our new recipe
            Recipe r = new Recipe(1, "trank_test", 1, "", "", "", "", "", true, 0, 0);
            // add recipe to correct list
            RecipeDict[craftingProbertyNumber].Add(r);
        }

    }
}
