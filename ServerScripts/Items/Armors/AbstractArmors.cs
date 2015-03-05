using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Armors
{
    /// <summary>
    /// Diese Klasse ist abstrakt, das heißt, sie wird nur für
    /// andere Food-Instanzen zum ableiten genutzt.
    /// </summary>
    public abstract class AbstractArmors : ItemInstance
    {
        public AbstractArmors(String instanceName)
            : base(instanceName)
        {
            MainFlags = Enumeration.MainFlags.ITEM_KAT_ARMOR;
            Flags = 0;

            Materials = Enumeration.MaterialTypes.MAT_LEATHER;
            Wear = Enumeration.ArmorFlags.WEAR_TORSO;

            Visual_skin = 0;
        }
    }
}
