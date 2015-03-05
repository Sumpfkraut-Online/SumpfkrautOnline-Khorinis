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
    public class oCMsgConversation : oCNpcMessage
    {
        public enum Offsets
        {
            text = 68,
            name = 88,
            target = 108,
            targetPos = 112,
            aniID = 124,
            ModelAni = 128,
            EventMessage = 132,
            SoundHandle = 136,
            timer = 140,
            number = 144,
            
        }

        /// <summary>
        /// Subtypes...
        /// </summary>
        public enum TConversationSubType
        {
            EV_PLAYANISOUND = 0,//Default
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
            EV_SNDPLAY,
            EV_SNDPLAY3D,

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

        public zCVob Target
        {
            get
            {
                return new zCVob(Process, Process.ReadInt(Address + (int)Offsets.target));
            }
        }

        #endregion
    }
}
