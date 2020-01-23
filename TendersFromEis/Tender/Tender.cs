using System.Security.Cryptography.X509Certificates;

namespace TendersFromEis.Tender
{
    public class Tender
    {
        public string PurchaseNumber { get; set; }
        public string DocPublishDate { get; set; }
        public string Href { get; set; }
        public Tender()
        {
            
        }
        
    }
}