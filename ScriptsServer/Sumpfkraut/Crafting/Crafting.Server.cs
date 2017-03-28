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
        Dictionary<int, List<Recipe>> RecipeLists = new Dictionary<int, List<Recipe>>();

        /// <summary>
        /// The strings are crafting property names from the DB and the corresponding integer is the numeric representation for the RecipeList Dict.
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

        public void Craft(NPCInst craftsmen, int recipeID, int craftingProperty)
        {
            List<Recipe> RecipeList;
            RecipeLists.TryGetValue(craftingProperty, out RecipeList);
            if(RecipeList == null)
            {
                throw new Exception("Crafting: Couldn't find a RecipeList! s(CP=" + craftingProperty + ")(rID=" + recipeID + ")");
                return;
            }

            Recipe r;
            r = RecipeList.ElementAt(recipeID);
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
                RecipeLists.Add(craftingProbertyNumber, new List<Recipe>());
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
            RecipeLists[craftingProbertyNumber].Add(r);
        }

    }
}
