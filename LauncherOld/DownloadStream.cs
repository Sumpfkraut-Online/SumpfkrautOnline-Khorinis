using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Launcher2
{
    class DownloadStream : Stream
    {
        Stream strm;
        int len;
        int pos;

        public DownloadStream(Stream netStream, int length)
        {
            strm = netStream;
            len = length;
            pos = 0;
        }
        
        /*
        protected override void Dispose(bool disposing)
        {
          if (disposing)
          {
            if (strm != null)
            {
              strm.Dispose();
            }
          }

          base.Dispose(disposing);
        }*/

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int rest = len - pos;
            int nRead;

            if (count > rest)
            {
                nRead = strm.Read(buffer, 0, rest);
            }
            else
            {
                nRead = strm.Read(buffer, 0, count);
            }
            pos += nRead;
            Global.Progress.Add((uint)nRead);

            return nRead;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }
    }
}
