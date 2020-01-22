using System;
using System.IO;
using System.Net;
using System.Text;

namespace TendersFromEis.NetworkLibrary
{
    public class TimedWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var wr = (HttpWebRequest) base.GetWebRequest(address);
            if (wr != null)
            {
                wr.Timeout = 20000;
                wr.UserAgent = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:55.0) Gecko/20100101 Firefox/55.0";
                return wr;
            }

            return null;
        }
    }
}