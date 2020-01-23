using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using TendersFromEis.Logger;
using TendersFromEis.NetworkLibrary;

namespace TendersFromEis.Parser
{
    public abstract class ParserAbstract
    {
        protected int PageCount;
        protected string CurrentUrl;
        protected int MaxDown = 1000;
        protected readonly HashSet<string> SetUrls = new HashSet<string>();

        protected ParserAbstract()
        {
            CurrentUrl = Uri.EscapeUriString(BuilderApp.Builder.SearchString);
        }

        protected string ChangeRecPerPage(string s)
        {
            if (s.Contains("recordsPerPage="))
            {
                var regex = new Regex(@"recordsPerPage=(_)?\d{1,3}");
                return regex.Replace(s, "recordsPerPage=50");
            }
            return $"{s}&recordsPerPage=50";
        }

        protected int MaxPage()
        {
            if (DownloadString.MaxDownload >= 1000) return 1;
            var s = DownloadString.DownLUserAgent(CurrentUrl);
            if (string.IsNullOrEmpty(s))
            {
                Log.Logger("cannot get first page from EIS", CurrentUrl);
                throw new Exception("cannot get first page from EIS");
            }
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(s);
            var maxPageS = htmlDoc.DocumentNode.SelectSingleNode("//ul[@class = 'pages']/li[last()]/a/span")?.InnerText ?? "1";
            if (int.TryParse(maxPageS, out var maxP))
            {
                return maxP;
            }
            return 1;
        }

        protected string CreateCurrentUrl(string url, int page)
        {
            if (!url.Contains("pageNumber=")) return $"{url}pageNumber={page}";
            var regex = new Regex(@"pageNumber=\d{1,3}");
            return regex.Replace(url, $"pageNumber={page}");
        }

        protected void Initialize()
        {
            CurrentUrl = ChangeRecPerPage(CurrentUrl);
            PageCount = MaxPage();
        }
    }
}