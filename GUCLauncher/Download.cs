using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace GUCLauncher
{
    static class Download
    {
        public const double TimeOut = 3000;

        public static WebResponse GetResponse(string url, Action<float> SetProgress = null)
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
                    double elapsed;
                    while ((elapsed = watch.Elapsed.TotalMilliseconds) <= TimeOut && !result.IsCompleted)
                    {
                        if (SetProgress != null) 
                            SetProgress((float)Math.Sqrt(elapsed / TimeOut)); // do some fake progress while connecting
                        Thread.Sleep(1);
                    }

                    return request.EndGetResponse(result);
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("exceptions.txt", e.ToString());
            }

            return null;
        }

        public static WebResponse GetResponse(string url, int from, int to)
        {
            try
            {
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    HttpWebRequest request = WebRequest.CreateHttp(uri);
                    request.UseDefaultCredentials = true;
                    request.Proxy = null;
                    request.AddRange(from, to);

                    var result = request.BeginGetResponse(null, null);

                    Stopwatch watch = Stopwatch.StartNew();
                    double elapsed;
                    while ((elapsed = watch.Elapsed.TotalMilliseconds) <= TimeOut && !result.IsCompleted)
                    {
                        Thread.Sleep(1);
                    }

                    return request.EndGetResponse(result);
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("exceptions.txt", e.ToString());
            }

            return null;
        }
    }
}
