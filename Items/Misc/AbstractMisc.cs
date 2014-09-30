using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Misc
{
    /// <summary>
    /// Diese Klasse ist abstrakt, das heißt, sie wird nur für
    /// andere Food-Instanzen zum ableiten genutzt.
    /// </summary>
    public abstract class AbstractMisc : ItemInstance
    {
        public AbstractMisc(String instanceName)
            : base(instanceName)
        {
            MainFlags = Enumeration.MainFlags.ITEM_KAT_NONE;
            Flags = Enumeration.Flags.ITEM_MULTI;

            Materials = Enumeration.MaterialTypes.MAT_WOOD;
        }
    }
}
