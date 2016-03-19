using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Launcher2
{
    class Download
    {
        private HttpWebRequest request;
        private string URL;

        //public uint length;
        
        public Download(string inputURL)
        {
            URL = inputURL;
            //length = 0xFFFFFFFF;
            request = (HttpWebRequest)WebRequest.Create(URL);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 5000;
            request.Proxy = null;
        }

        public async Task<Stream> StartStream(uint start)
        {
            request.AddRange(start);
            return await StartStream();
        }

        public async Task<Stream> StartStream(uint start, uint end)
        {
            request.AddRange(start, end-1);
            return await StartStream();
        }

        public async Task<Stream> StartStream()
        {
            try
            {
                WebResponse response = await request.GetResponseAsync();
               /* if (!uint.TryParse(response.Headers.Get("Content-Length"), out length))
                {
                    length = 0xFFFFFFFF;
                }*/

                return response.GetResponseStream();
            }
            catch
            {
                Global.Progress.Error("Fehler:  Updateserver konnte nicht erreicht werden!");
                return null;
            }
        }
    }
}
