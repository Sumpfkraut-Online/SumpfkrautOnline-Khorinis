using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCDocumentManager : zClass
    {
        public oCDocumentManager() { }
        public oCDocumentManager(Process process, int address)
            : base(process, address)
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }

        #region OffsetLists

        public enum Offsets
        {

        }

        public enum FuncOffsets
        {
            GetDocumentManager = 0x0065EA40,
            CreateDocument = 0x0065ED20
        }
        #endregion


        #region staticMethods

        public static oCDocumentManager GetDocumentManager( Process process )
        {
            return process.CDECLCALL<oCDocumentManager>((uint)FuncOffsets.GetDocumentManager, new CallValue[]{});
        }

        #endregion


        public int CreateDocument(oCDocumentManager docManager)
        {
            return Process.FASTCALL<IntArg>((uint)docManager.Address, 0, (uint)FuncOffsets.CreateDocument, new CallValue[] { });
        }
    }
}
