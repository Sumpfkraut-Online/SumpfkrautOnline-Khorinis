using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Belts
{
    /// <summary>
    /// Diese Klasse ist abstrakt, das heißt, sie wird nur für
    /// andere Food-Instanzen zum ableiten genutzt.
    /// </summary>
    public abstract class AbstractBelts : ItemInstance
    {
        public AbstractBelts(String instanceName)
            : base(instanceName)
        {
            Name = "Gürtel";
            MainFlags = Enumeration.MainFlags.ITEM_KAT_MAGIC;
            Flags = Enumeration.Flags.ITEM_BELT;

            Materials = Enumeration.MaterialType.MAT_METAL;
            Visual_skin = 0;
        }
    }
}
