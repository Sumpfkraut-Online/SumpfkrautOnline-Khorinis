using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zStruct
{
    /// <summary>
    /// Größe der Klasse: 0x98 Classdef: 0x00AB2980
    /// </summary>
    public class oCMsgConversation : zCObject
    {
        public enum Offsets
        {
            subType = 36, //18=Overlay 14=keinOverlay 19=keinOverlay????
            targetVobName = 44,//zString
            text = 68,
            name = 88,
            vob = 108
        }

        /// <summary>
        /// Subtypes...
        /// </summary>
        public enum TConversationSubType
        {
            Error = 0,//nicht bekannt?
            EV_PLAYANI,
            EV_PLAYSOUND,
            EV_LOOKAT,
            EV_OUTPUT = 4,
            EV_OUTPUTSVM = 5,
            EV_CUTSCENE,
            EV_WAITTILLEND,
            EV_ASK,
            EV_WAITFORQUESTION,
            EV_STOPLOOKAT,
            EV_STOPPOINTAT,
            EV_POINTAT,
            EV_QUICKLOOK,
            EV_PLAYANI_NOOVERLAY,
            EV_PLAYANI_FACE,
            EV_PROCESSINFOS,
            EV_STOPPROCESSINFOS,
            EV_OUTPUTSVM_OVERLAY = 18,
            EV_SNDPLAY
        }

        public oCMsgConversation()
            : base()
        {

        }

        public oCMsgConversation(Process process, int address)
            : base(process, address)
        {

        }



        #region statics

        public static oCMsgConversation Create(Process process, TConversationSubType conversationType, zCVob vob)
        {
            oCMsgConversation rVal = null;

            IntPtr address = process.Alloc(0x98);
            zCClassDef.ObjectCreated(process, address.ToInt32(), 0x00AB2980);
            
            //Konstruktor...
            process.THISCALL<NullReturnCall>((uint)address.ToInt32(), 0x0076A1E0, new CallValue[] { (IntArg)((int)conversationType), vob });
            rVal = new oCMsgConversation(process, address.ToInt32());


            
            return rVal;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Wird durch den Konstruktor angegeben.
        /// <return>Subtype als ushort, entspricht der Enumeration TConversationSubType</return>
        /// </summary>
        public ushort subType
        {
            get
            {
                return Process.ReadUShort(Address + (int)Offsets.subType);
            }
        }

        /// <summary>
        /// Der Text der z.B. bei Sprachausgabe ausgegeben wird 
        /// Bsp. "Krieg ich dich doch noch!"
        /// </summary>
        public zString Text
        {
            get
            {
                return new zString(Process, Address + (int)Offsets.text);
            }
        }

        /// <summary>
        /// Gibt den Namen wieder z.B. bei Sprachausgabe (SVM) im zusammenhang mit Text: SVM_1_IGETYOUSTILL.WAV oder auch $IGETYOUSTILL
        /// Bei OutputSVM_Overlay wird erst ohne einen Text geschickt, und $IGETYOUSTILL, danach mehrere male? mit der angegebenen WAV und dem dazugehörigen Text.
        /// </summary>
        public zString Name
        {
            get
            {
                return new zString(Process, Address + (int)Offsets.name);
            }
        }

        public zString TargetVobName
        {
            get
            {
                return new zString(Process, Address + (int)Offsets.targetVobName);
            }
        }

        public zCVob Vob
        {
            get
            {
                return new zCVob(Process, Process.ReadInt(Address + (int)Offsets.vob));
            }
        }

        #endregion
    }
}
