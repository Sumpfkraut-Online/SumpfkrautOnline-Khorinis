using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class ItemDef
    {
        public void PositionInVobVisual(GUCVobVisual vis)
        {
            Vec3f offset;
            Angles angles;
            switch (this.ItemType)
            {
                case ItemTypes.Wep1H:
                case ItemTypes.Wep2H:
                case ItemTypes.WepBow:
                case ItemTypes.WepXBow:
                    offset = new Vec3f(40, -15, 25);
                    angles = new Angles(-Angles.PI/2.0f, 0, 0.5235988f);
                    break;
                case ItemTypes.Drinkable:
                    offset = new Vec3f(0, -10, -60);
                    angles = new Angles(0, 0, 0);
                    break;
                default:
                    offset = new Vec3f(0, 0, 0);
                    angles = new Angles(0, 0, 0);
                    break;
            }

            vis.OffsetX = offset.X + this.InvOffset.X;
            vis.OffsetY = offset.Y + this.InvOffset.Y;
            vis.OffsetZ = offset.Z + this.InvOffset.Z;

            vis.RotationPitch = angles.Pitch + this.InvRotation.Pitch;
            vis.RotationYaw = angles.Yaw + this.InvRotation.Yaw;
            vis.RotationRoll = angles.Roll + this.InvRotation.Roll;
        }
    }
}
