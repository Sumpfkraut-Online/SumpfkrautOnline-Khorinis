using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Items.Weapons.Melee
{
    /// <summary>
    /// Diese Klasse ist abstrakt, das heißt, sie wird nur für
    /// andere Food-Instanzen zum ableiten genutzt.
    /// </summary>
    public abstract class AbstractMelee : ItemInstance
    {
        protected AbstractMelee() : base()
        {
            MainFlags = Enumeration.MainFlags.ITEM_KAT_NF;
            Materials = Enumeration.MaterialType.MAT_METAL;
            Weight = 5;
        }
    }
}
