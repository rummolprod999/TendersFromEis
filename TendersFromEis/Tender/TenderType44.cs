using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TendersFromEis.Parser;

namespace TendersFromEis.Tender
{
    public class TenderType44 : TenderAbstract, ITender

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
            var printForm = ((string) J.SelectToken("printForm.url") ?? "").Trim();
            if (!String.IsNullOrEmpty(printForm) && printForm.IndexOf("CDATA", StringComparison.Ordinal) != -1)
                printForm = printForm.Substring(9, printForm.Length - 12);
            tender.PrintForm = printForm;
            tender.PurchaseObjectInfo = ((string) J.SelectToken("purchaseObjectInfo") ?? "").Trim();
            tender.OrganizerRegNum = ((string) J.SelectToken("purchaseResponsible.responsibleOrg.regNum") ?? "").Trim();
            tender.OrganizerFullName =
                ((string) J.SelectToken("purchaseResponsible.responsibleOrg.fullName") ?? "").Trim();
            tender.OrganizerPostAddress =
                ((string) J.SelectToken("purchaseResponsible.responsibleOrg.postAddress") ?? "").Trim();
            tender.OrganizerFactAddress =
                ((string) J.SelectToken("purchaseResponsible.responsibleOrg.factAddress") ?? "").Trim();
            tender.OrganizerInn = ((string) J.SelectToken("purchaseResponsible.responsibleOrg.INN") ?? "").Trim();
            tender.OrganizerKpp = ((string) J.SelectToken("purchaseResponsible.responsibleOrg.KPP") ?? "").Trim();
            tender.OrganizerResponsibleRole =
                ((string) J.SelectToken("purchaseResponsible.responsibleRole") ?? "").Trim();
            tender.OrganizerEmail =
                ((string) J.SelectToken("purchaseResponsible.responsibleInfo.contactEMail") ?? "").Trim();
            tender.OrganizerFax =
                ((string) J.SelectToken("purchaseResponsible.responsibleInfo.contactFax") ?? "").Trim();
            tender.OrganizerPhone =
                ((string) J.SelectToken("purchaseResponsible.responsibleInfo.contactPhone") ?? "").Trim();
            var organizerLastName =
                ((string) J.SelectToken("purchaseResponsible.responsibleInfo.contactPerson.lastName") ??
                 "").Trim();
            var organizerFirstName =
                ((string) J.SelectToken("purchaseResponsible.responsibleInfo.contactPerson.firstName") ??
                 "").Trim();
            var organizerMiddleName =
                ((string) J.SelectToken("purchaseResponsible.responsibleInfo.contactPerson.middleName") ??
                 "").Trim();
            tender.OrganizerContact = $"{organizerLastName} {organizerFirstName} {organizerMiddleName}"
                .Trim();
            tender.PlacingWayCode = ((string) J.SelectToken("placingWay.code") ?? "").Trim();
            tender.PlacingWayName = ((string) J.SelectToken("placingWay.name") ?? "").Trim();
            tender.EtpCode = ((string) J.SelectToken("ETP.code") ?? "").Trim();
            tender.EtpName = ((string) J.SelectToken("ETP.name") ?? "").Trim();
            tender.EtpUrl = ((string) J.SelectToken("ETP.url") ?? "").Trim();
            tender.EndDate =
                (((string) J.SelectToken("procedureInfo.collecting.endDate") ??
                  (string) J.SelectToken("procedureInfo.collectingEndDate")) ?? "").Trim();
            tender.ScoringDate = ((string) J.SelectToken("procedureInfo.scoring.date") ?? "").Trim();
            tender.BiddingDate = ((string) J.SelectToken("procedureInfo.bidding.date") ?? "").Trim();
            tender.Lots = CreateLots(J);
            ParserAbstract.ListTenders.Add(tender);
        }

        private List<Tender.Lot> CreateLots(JToken j)
        {
            var lotsList = new List<Tender.Lot>();
            var lots = GetElements(j, "lot");
            if (lots.Count == 0)
                lots = GetElements(j, "lots.lot");
            lots.ForEach(l =>
            {
                var lot = new Tender.Lot();
                lot.LotMaxPrice = ((string) l.SelectToken("maxPrice") ?? "").Trim();
                lot.LotCurrency = ((string) l.SelectToken("currency.name") ?? "").Trim();
                lot.LotFinanceSource = ((string) l.SelectToken("financeSource") ?? "").Trim();
                lot.LotName = ((string) l.SelectToken("lotObjectInfo") ?? "").Trim();
                lot.PurchaseObjects = CreatePurchaseObjects(l);
                lotsList.Add(lot);
            });
            return lotsList;
        }

        private List<Tender.Lot.PurchaseObject> CreatePurchaseObjects(JToken l)
        {
            var poList = new List<Tender.Lot.PurchaseObject>();
            var purchaseObjects = GetElements(l, "purchaseObjects.purchaseObject");
            purchaseObjects.ForEach(po =>
            {
                var purObj = new Tender.Lot.PurchaseObject();
                purObj.Code = ((string) po.SelectToken("KTRU.code") ?? "").Trim();
                purObj.Name = ((string) po.SelectToken("KTRU.name") ?? "").Trim();
                purObj.Price = ((string) po.SelectToken("price") ?? "").Trim();
                purObj.Quantity = ((string) po.SelectToken("quantity.value") ?? "").Trim();
                purObj.Sum = ((string) po.SelectToken("sum") ?? "").Trim();
                purObj.OkeiName = ((string) po.SelectToken("OKEI.fullName") ?? "").Trim();
                purObj.Okpd2Code = ((string) po.SelectToken("OKPD2.code") ?? "").Trim();
                purObj.Okpd2Name = ((string) po.SelectToken("OKPD2.name") ?? "").Trim();
                purObj.Okpd2AddCharacteristic = ((string) po.SelectToken("OKPD2.addCharacteristics") ?? "").Trim();
                purObj.KtruCharacteristics = CreateKtruCharacteristics(po);
                poList.Add(purObj);
            });
            return poList;
        }

        private List<Tender.Lot.PurchaseObject.KtruCharacteristic> CreateKtruCharacteristics(JToken p)
        {
            var ctruCharacteristics = new List<Tender.Lot.PurchaseObject.KtruCharacteristic>();
            var characteristics = GetElements(p, "KTRU.characteristics.characteristicsUsingReferenceInfo");
            characteristics.ForEach(ch =>
            {
                var charact = new Tender.Lot.PurchaseObject.KtruCharacteristic();
                charact.Name = ((string) ch.SelectToken("name") ?? "").Trim();
                charact.CharacteristicValues = CreateCharacteristicValues(ch);
                ctruCharacteristics.Add(charact);
            });
            return ctruCharacteristics;
        }

        private List<Tender.Lot.PurchaseObject.KtruCharacteristic.CharacteristicValue>
            CreateCharacteristicValues(JToken ch)
        {
            var charValues = new List<Tender.Lot.PurchaseObject.KtruCharacteristic.CharacteristicValue>();
            var values = GetElements(ch, "values.value");
            values.ForEach(v =>
            {
                var value = new Tender.Lot.PurchaseObject.KtruCharacteristic.CharacteristicValue();
                value.QualityDescription = ((string) v.SelectToken("qualityDescription") ?? "").Trim();
                value.ValueRangeMinMathNotation =
                    ((string) v.SelectToken("rangeSet.valueRange.minMathNotation") ?? "").Trim();
                value.ValueRangeMin = ((string) v.SelectToken("rangeSet.valueRange.min") ?? "").Trim();
                value.ValueRangeMaxMathNotation =
                    ((string) v.SelectToken("rangeSet.valueRange.maxMathNotation") ?? "").Trim();
                value.ValueRangeMax = ((string) v.SelectToken("rangeSet.valueRange.max") ?? "").Trim();
                charValues.Add(value);
            });
            return charValues;
        }
    }
}