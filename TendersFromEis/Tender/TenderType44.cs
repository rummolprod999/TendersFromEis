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
            var printForm = ((string) J.SelectToken("printForm.url") ?? "").Trim();
            if (!String.IsNullOrEmpty(printForm) && printForm.IndexOf("CDATA", StringComparison.Ordinal) != -1)
                printForm = printForm.Substring(9, printForm.Length - 12);
            tender.PrintForm = printForm;
            tender.PurchaseObjectInfo = ((string) J.SelectToken("purchaseObjectInfo") ?? "").Trim();
            tender.OrganizerRegNum = ((string) J.SelectToken("purchaseResponsible.responsibleOrg.regNum") ?? "").Trim();
            tender.OrganizerFullName = ((string) J.SelectToken("purchaseResponsible.responsibleOrg.fullName") ?? "").Trim();
            tender.OrganizerPostAddress = ((string) J.SelectToken("purchaseResponsible.responsibleOrg.postAddress") ?? "").Trim();
            tender.OrganizerFactAddress = ((string) J.SelectToken("purchaseResponsible.responsibleOrg.factAddress") ?? "").Trim();
            tender.OrganizerInn = ((string) J.SelectToken("purchaseResponsible.responsibleOrg.INN") ?? "").Trim();
            tender.OrganizerKpp = ((string) J.SelectToken("purchaseResponsible.responsibleOrg.KPP") ?? "").Trim();
            tender.OrganizerResponsibleRole = ((string) J.SelectToken("purchaseResponsible.responsibleRole") ?? "").Trim();
            tender.OrganizerEmail = ((string) J.SelectToken("purchaseResponsible.responsibleInfo.contactEMail") ?? "").Trim();
            tender.OrganizerFax = ((string) J.SelectToken("purchaseResponsible.responsibleInfo.contactFax") ?? "").Trim();
            tender.OrganizerPhone = ((string) J.SelectToken("purchaseResponsible.responsibleInfo.contactPhone") ?? "").Trim();
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
            Console.WriteLine(tender.DocPublishDate);
        }
    }
}