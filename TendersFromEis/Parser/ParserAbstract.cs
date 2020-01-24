using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MailKit.Net.Smtp;
using MimeKit;
using OfficeOpenXml;
using TendersFromEis.BuilderApp;
using TendersFromEis.Logger;
using TendersFromEis.NetworkLibrary;

namespace TendersFromEis.Parser
{
    public abstract class ParserAbstract
    {
        protected int PageCount = 1; //TODO change it
        protected string CurrentUrl;
        protected int MaxDown = 1000;
        protected readonly HashSet<string> SetUrls = new HashSet<string>();
        public static List<Tender.Tender> ListTenders = new List<Tender.Tender>();

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
            var maxPageS =
                htmlDoc.DocumentNode.SelectSingleNode("//ul[@class = 'pages']/li[last()]/a/span")?.InnerText ?? "1";
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

        protected void ExcelWriter(string pathFile)
        {
            using var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
            worksheet.Cells[1, 1].Value = "Номер закупки";
            worksheet.Cells[1, 2].Value = "Дата публикации";
            worksheet.Cells[1, 3].Value = "Дата окончания подачи заявок";
            worksheet.Cells[1, 4].Value = "Ссылка";
            worksheet.Cells[1, 5].Value = "НМЦК";
            worksheet.Cells[1, 6].Value = "Валюта";
            worksheet.Cells[1, 7].Value = "Название лота";
            worksheet.Cells[1, 8].Value = "КОД ПОЗИЦИИ";
            worksheet.Cells[1, 9].Value = "НАИМЕНОВАНИЕ ТОВАРА, РАБОТЫ, УСЛУГИ";
            worksheet.Cells[1, 10].Value = "ЕД. ИЗМЕРЕНИЯ";
            worksheet.Cells[1, 11].Value = "КОЛИЧЕСТВО";
            worksheet.Cells[1, 12].Value = "ЦЕНА ЗА ЕД.";
            worksheet.Cells[1, 13].Value = "СТОИМОСТЬ";
            var row = 2;
            ListTenders.ForEach(t =>
            {
                t.Lots.ForEach(l =>
                {
                    l.PurchaseObjects.ForEach(po =>
                    {
                        worksheet.Cells[row, 1].Value = t.PurchaseNumber;
                        worksheet.Cells[row, 2].Value = t.DocPublishDate;
                        worksheet.Cells[row, 3].Value = t.EndDate;
                        worksheet.Cells[row, 4].Value = t.Href;
                        worksheet.Cells[row, 5].Value = l.LotMaxPrice;
                        worksheet.Cells[row, 6].Value = l.LotCurrency;
                        worksheet.Cells[row, 7].Value = l.LotName;
                        worksheet.Cells[row, 8].Value = po.Code;
                        var fullPoName = $"{po.Name}\n";
                        po.KtruCharacteristics.ForEach(ch =>
                        {
                            fullPoName += $"{ch.Name}: ";
                            ch.CharacteristicValues.ForEach(v =>
                            {
                                fullPoName += $"{v.QualityDescription}";
                                if (v.ValueRangeMinMathNotation != "" && v.ValueRangeMaxMathNotation != "")
                                {
                                    fullPoName +=
                                        $"{v.ValueRangeMinMathNotation} {v.ValueRangeMin} и {v.ValueRangeMaxMathNotation} {v.ValueRangeMax}";
                                }
                            });
                            fullPoName += "\n";
                        });
                        worksheet.Cells[row, 9].Value = fullPoName;
                        worksheet.Cells[row, 10].Value = po.OkeiName;
                        worksheet.Cells[row, 11].Value = po.Quantity;
                        worksheet.Cells[row, 12].Value = po.Price;
                        worksheet.Cells[row, 13].Value = po.Sum;
                        row++;
                    });
                });
            });
            excelPackage.SaveAs(new FileInfo(pathFile));
        }

        protected void SendEmail(string pathFile)
        {
            if (!new FileInfo(pathFile).Exists)
            {
                Log.Logger("the excel file not found, return without send email");
                return;
            }

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Тестовый аккаунт", Builder.EmailFrom));
            emailMessage.To.Add(new MailboxAddress("", Builder.EmailTo));
            emailMessage.Subject = "Новые тенедеры из поисковой выдачи по запросу";
            var body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "Файл во вложении"
            };
            var attachment = new MimePart("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx")
            {
                Content = new MimeContent(File.OpenRead(pathFile)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(pathFile)
            };
            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);
            emailMessage.Body = multipart;
            var client = new SmtpClient();
            client.Connect(Builder.SmtpServer, Builder.SmtpPort, true);
            client.Authenticate(Builder.EmailFrom, Builder.SmtpPass);
            client.Send(emailMessage);
            client.Disconnect(true);
        }

        protected void DeleteOldExcel(string pathFile)
        {
            var fInfo = new FileInfo(pathFile);
            if (fInfo.Exists)
            {
                fInfo.Delete();
            }
        }

        protected void Initialize()
        {
            CurrentUrl = ChangeRecPerPage(CurrentUrl);
            // PageCount = MaxPage(); //TODO change it
        }
    }
}