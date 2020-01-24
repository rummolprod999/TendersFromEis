using System.Collections.Generic;

namespace TendersFromEis.Tender
{
    public class Tender
    {
        public Tender()
        {
            
        }

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
        public string EtpCode { get; set; }
        public string EtpName { get; set; }
        public string EtpUrl { get; set; }
        public string EndDate { get; set; }
        public string ScoringDate { get; set; }
        public string BiddingDate { get; set; }
        public List<Lot> Lots { get; set; }
        

        public class Lot
        {
            public string LotMaxPrice { get; set; }
            public string LotCurrency { get; set; }
            public string LotFinanceSource { get; set; }
            public string LotName { get; set; }
            public List<PurchaseObject> PurchaseObjects { get; set; }
            public class PurchaseObject
            {
                public string Code { get; set; }
                public string Okpd2Code { get; set; }
                public string Okpd2Name { get; set; }
                public string Okpd2AddCharacteristic { get; set; }
                public string Name { get; set; }
                public string Price { get; set; }
                public string Quantity { get; set; }
                public string Sum { get; set; }
                public string OkeiName { get; set; }
                public List<KtruCharacteristic> KtruCharacteristics { get; set; }
                public class KtruCharacteristic
                {
                    public string Name { get; set; }
                    public List<CharacteristicValue> CharacteristicValues { get; set; }
                    public class CharacteristicValue
                    {
                        public string QualityDescription { get; set; }
                        public string ValueRangeMinMathNotation { get; set; }
                        public string ValueRangeMin { get; set; }
                        public string ValueRangeMaxMathNotation { get; set; }
                        public string ValueRangeMax { get; set; }
                    }
                }
            }
        }

        
    }
}