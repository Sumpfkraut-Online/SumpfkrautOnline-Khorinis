using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace GUCLauncher
{
    static class Download
    {
        public const double TimeOut = 3000;

        public static bool TryGetStream(string url, out Stream stream, Action<double> SetProgress = null)
        {
            try
            {
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    HttpWebRequest request = WebRequest.CreateHttp(uri);
                    request.UseDefaultCredentials = true;
                    request.Proxy = null;

                    var result = request.BeginGetResponse(null, null);

                    Stopwatch watch = Stopwatch.StartNew();
                    double diff;
                    while ((diff = TimeOut - watch.Elapsed.TotalMilliseconds) >= 0 && !result.IsCompleted)
                    {
                        if (SetProgress != null)
                            SetProgress((1d - diff / TimeOut) * 100);
                    }

                    var response = request.EndGetResponse(result);
                    stream = response.GetResponseStream();
                    return stream != null;
                }
            }
            catch
            {
            }

            stream = null;
            return false;
        }
    }
}
