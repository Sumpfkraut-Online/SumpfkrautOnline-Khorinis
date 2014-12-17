using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Amulet
{
    /// <summary>
    /// Diese Klasse ist abstrakt, das heißt, sie wird nur für
    /// andere Food-Instanzen zum ableiten genutzt.
    /// </summary>
    public abstract class AbstractAmulets : ItemInstance
    {
        public AbstractAmulets(String instanceName)
            : base(instanceName)
        {
            Name = "Amulett";
            MainFlags = Enumeration.MainFlags.ITEM_KAT_MAGIC;
            Flags = Enumeration.Flags.ITEM_AMULET;

            Materials = Enumeration.MaterialTypes.MAT_METAL;
            Effect = "SPELLFX_ITEMGLIMMER";
            Wear = Enumeration.ArmorFlags.WEAR_EFFECT;

            Visual_skin = 0;

        }
    }
}
