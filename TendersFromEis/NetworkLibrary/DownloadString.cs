using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TendersFromEis.Logger;

namespace TendersFromEis.NetworkLibrary
{
    public static class DownloadString
    {
        public static int MaxDownload;
        static DownloadString()
        {
            MaxDownload = 0;
        }
        public static string DownLUserAgent(string url)
        {
            var tmp = "";
            var count = 0;
            while (true)
            {
                try
                {
                    var task = Task.Run(() => (new TimedWebClient()).DownloadString(url));
                    if (!task.Wait(TimeSpan.FromSeconds(60))) throw new TimeoutException();
                    tmp = task.Result;
                    break;
                }
                catch (WebException ex)
                {
                    if (ex.Response is HttpWebResponse r) Log.Logger("Response code: ", r.StatusCode);
                    if (ex.Response is HttpWebResponse errorResponse &&
                        errorResponse.StatusCode == HttpStatusCode.Forbidden)
                    {
                        Log.Logger("Error 403 or 434");
                        return tmp;
                    }

                    if (count >= 2)
                    {
                        Log.Logger($"Cannot download for {count} attemps", url);
                        break;
                    }

                    Log.Logger("The string has not been downloaded", ex.Message, url);
                    count++;
                    Thread.Sleep(5000);
                }
                catch (Exception e)
                {
                    if (count >= 2)
                    {
                        Log.Logger($"Cannot download for {count} attemps", url);
                        break;
                    }

                    switch (e)
                    {
                        case AggregateException a
                            when a.InnerException != null && a.InnerException.Message.Contains("(404) Not Found"):
                            Log.Logger("404 Exception", a.InnerException.Message, url);
                            goto Finish;
                        case AggregateException a
                            when a.InnerException != null && a.InnerException.Message.Contains("(403) Forbidden"):
                            Log.Logger("403 Exception", a.InnerException.Message, url);
                            goto Finish;
                        case AggregateException a when a.InnerException != null &&
                                                       a.InnerException.Message.Contains(
                                                           "The remote server returned an error: (434)"):
                            Log.Logger("434 Exception", a.InnerException.Message, url);
                            goto Finish;
                    }

                    Log.Logger("The string has not been downloaded", e, url);
                    count++;
                    Thread.Sleep(5000);
                }
            }

            Finish:
            return tmp;
        }
    }
}