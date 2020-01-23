using System.Security.Cryptography.X509Certificates;

namespace TendersFromEis.Tender
{
    public class Tender
    {
        public string PurchaseNumber { get; set; }
        public string DocPublishDate { get; set; }
        public string Href { get; set; }
        public string PrintForm { get; set; }
        public string PurchaseObjectInfo { get; set; }
        public string OrganizerRegNum { get; set; }
        public string OrganizerFullName { get; set; }
        public string OrganizerPostAddress { get; set; }
        public string OrganizerFactAddress { get; set; }
        public string OrganizerInn { get; set; }
        public string OrganizerKpp { get; set; }
        public string OrganizerResponsibleRole { get; set; }
        public string OrganizerEmail { get; set; }
        public string OrganizerFax { get; set; }
        public string OrganizerPhone { get; set; }
        public string OrganizerContact { get; set; }
        public string PlacingWayCode { get; set; }
        public string PlacingWayName { get; set; }
        public Tender()
        {
            
        }
        
    }
}