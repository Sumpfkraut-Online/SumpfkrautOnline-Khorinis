using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract class CItemDef : CBaseVobDef
    {

        public void PositionInVobVisual (GUCVobVisual vis)
        {
            Vec3f offset;
            Angles angles;
            switch (this.ItemType)
            {
                case ItemTypes.Wep1H:
                case ItemTypes.Wep2H:
                    offset = new Vec3f(40, -15, 25);
                    angles = new Angles(-Angles.PI / 2.0f, 0, 0.5235988f);
                    break;
                case ItemTypes.WepXBow:
                    offset = new Vec3f(15, -15, -5);
                    angles = new Angles(-Angles.PI / 2.0f, 0, 0.5235988f);
                    break;
                case ItemTypes.WepBow:
                    offset = new Vec3f(-15, -15, 50);
                    angles = new Angles(-Angles.PI / 2.0f, 0, 0.5235988f);
                    break;
                case ItemTypes.Drinkable:
                    offset = new Vec3f(0, -10, -60);
                    angles = new Angles(0, 0, 0);
                    break;
                case ItemTypes.AmmoBow:
                    offset = new Vec3f(20, -10, -25);
                    angles = new Angles(-Angles.PI / 2.0f, 0, 0.4363323f);
                    break;
                case ItemTypes.AmmoXBow:
                    offset = new Vec3f(5, -2.5f, -70);
                    angles = new Angles(0, 0, 0.4363323f);
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
