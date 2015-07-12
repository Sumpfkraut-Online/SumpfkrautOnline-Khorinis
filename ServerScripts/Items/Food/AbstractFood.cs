using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Items.Food
{
    /// <summary>
    /// Diese Klasse ist abstrakt, das heißt, sie wird nur für
    /// andere Food-Instanzen zum ableiten genutzt.
    /// </summary>
    public abstract class AbstractFood : ItemInstance
    {
        protected AbstractFood() : base()
        {
            MainFlags = Enumeration.MainFlags.ITEM_KAT_FOOD;
            Flags = Enumeration.Flags.ITEM_MULTI;

            Materials = Enumeration.MaterialType.MAT_LEATHER;
            ScemeName = "FOODHUGE";

            Weight = 1;
        }
    }
}
