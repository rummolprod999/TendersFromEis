using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TendersFromEis.Extensions;
using TendersFromEis.NetworkLibrary;
using TendersFromEis.Tender;
using Log = TendersFromEis.Logger.Log;

namespace TendersFromEis.Parser
{
    public class ParserWeb44 : ParserAbstract, IParser

    {
        public ParserWeb44() : base()
        {
        }

        public void Parsing()
        {
            Initialize();
            CreateListUrls();
        }

        private void CreateTenderFromDocList()
        {
            foreach (var url in setUrls)
            {
                CreateTenderFromDoc(url);
            }
        }

        private void CreateTenderFromDoc(string url)
        {
            var s = DownloadString.DownLUserAgent(url);
            if (String.IsNullOrEmpty(s))
            {
                Log.Logger("Empty string in CreateTenderFromDoc()", url);
                return;
            }
            s = s.CleanStringXml();
            var doc = new XmlDocument();
            doc.LoadXml(s);
            var jsons = JsonConvert.SerializeXmlNode(doc);
            var json = JObject.Parse(jsons);
            var firstOrDefault = json.Properties().FirstOrDefault(p => p.Name.Contains("fcs"));
            if (firstOrDefault != null)
            {
                var tender = firstOrDefault.Value;
                var t = new TenderType44(tender, url);
                RunTenderParsing(t);
            }
            else
            {
                firstOrDefault = json.Properties().FirstOrDefault(p => p.Name.Contains("epN"));
                if (firstOrDefault != null)
                {
                    
                }
                else
                {
                    Log.Logger("Can not to define the tender type", url);
                }
                
            }

        }

        private void RunTenderParsing(ITender t)
        {
            try
            {
                t.ParsingTender();
            }
            catch (Exception e)
            {
                Log.Logger(e);
            }
        }

        private void CreateListUrls()
        {
            for (var i = 1; i <= _pageCount; i++)
            {
                try
                {
                    ParsingPage(i);
                }
                catch (Exception e)
                {
                    Log.Logger(e);
                }
            }
        }

        private void ParsingPage(int i)
        {
            var currUrl = CreateCurrentUrl(_currentUrl, i);
            if (DownloadString.MaxDownload >= 1000) return;
            var s = DownloadString.DownLUserAgent(currUrl);
            if (string.IsNullOrEmpty(s))
            {
                Log.Logger("Empty string in ParserPage()", currUrl);
                return;
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(s);
            var tens = htmlDoc.DocumentNode.SelectNodes(
                           "//div[contains(@class, 'search-registry-entry-block')]/div[contains(@class, 'row')][1]//a[contains(@href, 'printForm/view')]") ??
                       new HtmlNodeCollection(null);
            foreach (var a in tens)
            {
                try
                {
                    ParserLink(a);
                }
                catch (Exception e)
                {
                    Log.Logger(e);
                }
            }
        }

        private void ParserLink(HtmlNode n)
        {
            var url = (n.Attributes["href"]?.Value ?? "").Trim();
            if (string.IsNullOrEmpty(url) || !url.Contains("epz/order")) return;
            url = url.Replace("view.html", "viewXml.html");
            url = $"http://zakupki.gov.ru{url}";
            setUrls.Add(url);
        }
    }
}