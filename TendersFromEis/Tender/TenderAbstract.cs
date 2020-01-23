using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TendersFromEis.Tender
{
    public class TenderAbstract
    {
        protected readonly JToken J;
        protected readonly string Url;
        

        protected TenderAbstract(JToken j, string url)
        {
            J = j;
            Url = url;
        }
    }
}