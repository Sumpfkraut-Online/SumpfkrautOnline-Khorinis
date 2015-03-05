using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Weapons.Melee
{
    /// <summary>
    /// Diese Klasse ist abstrakt, das heißt, sie wird nur für
    /// andere Food-Instanzen zum ableiten genutzt.
    /// </summary>
    public abstract class AbstractMelee : ItemInstance
    {
        public AbstractMelee(String instanceName)
            : base(instanceName)
        {
            MainFlags = Enumeration.MainFlags.ITEM_KAT_NF;
            Flags = Enumeration.Flags.ITEM_AXE;

            Materials = Enumeration.MaterialTypes.MAT_METAL;
            

        }
    }
}
