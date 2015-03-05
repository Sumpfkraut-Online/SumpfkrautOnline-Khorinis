using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Plants
{
    /// <summary>
    /// Diese Klasse ist abstrakt, das heißt, sie wird nur für
    /// andere Food-Instanzen zum ableiten genutzt.
    /// </summary>
    public abstract class AbstractPlants : ItemInstance
    {
        public AbstractPlants(String instanceName)
            : base(instanceName)
        {
            MainFlags = Enumeration.MainFlags.ITEM_KAT_FOOD;
            Flags = Enumeration.Flags.ITEM_MULTI;

            Materials = Enumeration.MaterialType.MAT_LEATHER;
            ScemeName = "FOOD";

        }
    }
}
