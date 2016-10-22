using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.VobGuiding
{
    public abstract partial class GuidedVob : BaseVob
    {
        #region Constructors

        public GuidedVob(IScriptBaseVob scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        GuideCmd currentCmd;
        public GuideCmd CurrentCommand { get { return this.currentCmd; } }

        internal GameClient guide;

        #endregion

        #region Spawn & Despawn

        partial void pSpawn(World world, Vec3f position, Vec3f direction);
        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            base.Spawn(world, position, direction);
            pSpawn(world, position, direction);
        }

        partial void pDespawn();
        public override void Despawn()
        {
            pDespawn();
            base.Despawn();
        }

        #endregion
    }
}
