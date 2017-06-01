using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

/* DB Structure
    Tabelle: Recipe

    ID -int
        eindeutige identifikation

    Codename - string
        Zur Identifikation für uns

    Description - string
        Information shown in crafting menu
        
    RecipeStep - int
        Jedes Rezept kann aus mehreren aufeinandernfolgenden Schritten bestehen

    NecessaryProperties - string
        Eigenschaften die ein Vob besitzen muss, damit an diesem Vob das Rezept ausgeführt werden kann

    Condition - string - method name
        Bedingungen die zu erfüllen sind, damit das Rezept verfügbar ist
        Condition wird als methoden name angeben der aufgerufen wird

    Euductlist - string
        Liste an nötigen Gegenständen für das Crafting
        Wie wird festgehalten wv verbraucht wird, bzw. bei einer Zange garnicht oder nur Haltbarkeit und ähnliches
        => lösung: haltbarkeitsitem wird als educt angegeben, removed und mit neuer haltbarkeit in der productliste angegeben
        #wird geparst#

    Productlist - string
        Liste an Gegenständen die durch das Crafting erschaffen werden
        Notation für change values überlegen
        #wird geparst#
    
    Effects - string
        Liste an effekten die nach dem crafting auf den spieler einwirken
        bsp, vergiftung, leben mehr/weniger, ausdauer etc, learning by doing effect etc.
        #wird geparst#

    CanCancel
        Regelung was passiert, wenn der der Vorgang abgebrochen wird bevor das Crafting vollendet ist
            - Edukte zurückerhalten - Fortschritt an item behalten => TRUE
            - Edukte zurückerhalten - Fortschritt an item verloren => FALSE
            - Edukte verlieren => NULL 

    TimeToCraft - int
        Zeit in ms wie lange die Crafting animation abgespielt werden muss
        (Bei Minigame eventuell irrelevant)

    MinigameId - short
        Falls es ein Minispiel gibt, hier die ID dazu

    NEU Recipe control -
 
*/


namespace GUC.Scripts.Sumpfkraut.Crafting
{
    class Recipe
    {

        public int uniqueID;
        public string description;

        string codename;
        public string Codename { get { return codename; } }

        string[] conditionParameters;

        string effect;
        public string Effect { get { return effect; } }

        public Conditions.Condition CheckRequiredCondition;
        public Effects.Effect ApplyEffects;

        public Recipe(int ID, string codename, int step, string craftingProperties, string conditionMethod, string eductList, string productList, string effectMethod,
            bool canCancel, int timeToCraft, int minigameID )
        {
            // create item use actions, create item create actions, create effects
            uniqueID = ID;

            // retrieve parameters for functioncall by DB string
            string[] condition = conditionMethod.Split(',');
            for(int i = 1; i < condition.Length; i++)
            {
                conditionParameters[i - 1] = condition[i];
            }

            // condition[0] contains the method name
            Conditions.conditionDict.TryGetValue(condition[0], out CheckRequiredCondition);
            if (CheckRequiredCondition == null)
            {
                RecipeError("Couldn't find a crafting condition method named " + condition[0]);
            }

            // retrieve parameters for functioncall by DB string
            string[] effect = effectMethod.Split(',');
            for (int i = 1; i < effect.Length; i++)
            {
                conditionParameters[i - 1] = condition[i];
            }

            // effect[0] contains the method name
            Effects.effectDict.TryGetValue(effect[0], out ApplyEffects);
            if (CheckRequiredCondition == null)
            {
                RecipeError("Couldn't find a effect method named " + effect[0]);
            }

        }

        public bool CheckRequiredMaterials(NPCInst craftsmen)
        {
            // parse string to materials?
            return true;
        }

        public bool CreateProducts(NPCInst craftsmen)
        {
            return true;   
        }

        void RecipeError(string err)
        {
            throw new Exception("ERROR: Crafting: " + err + " to create recipe " + codename);
        }
    }
}
