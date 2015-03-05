using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Keys
{
    /// <summary>
    /// Diese Klasse ist abstrakt, das heißt, sie wird nur für
    /// andere Food-Instanzen zum ableiten genutzt.
    /// </summary>
    public abstract class AbstractKeys : ItemInstance
    {
        public AbstractKeys(String instanceName)
            : base(instanceName)
        {
            Name = "Schlüssel";
            Description = Name;
            MainFlags = Enumeration.MainFlags.ITEM_KAT_NONE;
            Flags = Enumeration.Flags.ITEM_MISSION;

            Materials = Enumeration.MaterialTypes.MAT_METAL;

        }
    }
}
