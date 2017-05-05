using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.VobGuiding;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{

    public class GuideCommandInfo : GUC.Utilities.ExtendedObject
    {

        new public static readonly string _staticName = "SimpleAIPersonality (s)";



        private GuideCmd guideCommand;
        public GuideCmd GuideCommand { get { return this.guideCommand; } }

        private VobInst guidedVobInst;
        public VobInst GuidedVobInst { get { return this.guidedVobInst; } }

        private DateTime creationDate;
        public DateTime CreationDate { get { return this.creationDate; } }

        public DateTime ChangeDate;
        public DateTime ExpirationDate;



        public GuideCommandInfo (GuideCmd guideCommand, VobInst guidedVobInst)
            : this(guideCommand, guidedVobInst, DateTime.MaxValue)
        { }

        public GuideCommandInfo (GuideCmd guideCommand, VobInst guidedVobInst, DateTime expirationDate)
        {
            SetObjName("GuideCommandInfo");
            this.guideCommand = guideCommand;
            this.guidedVobInst = guidedVobInst;
            this.creationDate = DateTime.Now;
            this.ChangeDate = DateTime.Now;
            this.ExpirationDate = expirationDate;
        }



        public void UpdateInfo (GuideCmd guideCommand, VobInst guidedVobInst, DateTime expirationDate)
        {
            this.guideCommand = guideCommand;
            this.guidedVobInst = guidedVobInst;
            this.ExpirationDate = expirationDate;
            this.ChangeDate = DateTime.Now;
        }

    }

}
