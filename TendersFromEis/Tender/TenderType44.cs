using System;
using Newtonsoft.Json.Linq;

namespace TendersFromEis.Tender
{
    public class TenderType44: TenderAbstract, ITender

    {
        public TenderType44(JToken j, string url) : base(j, url)
        {
        }

        public void ParsingTender()
        {
            var tender = new Tender();
            tender.PurchaseNumber = ((string) J.SelectToken("purchaseNumber") ?? "").Trim();
            tender.DocPublishDate = ((string) J.SelectToken("docPublishDate") ?? "").Trim();
            tender.Href = ((string) J.SelectToken("href") ?? "").Trim();
            Console.WriteLine(tender.DocPublishDate);
        }
    }
}