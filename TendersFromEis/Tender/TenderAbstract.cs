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
        public List<JToken> GetElements(JToken j, string s)
        {
            List<JToken> els = new List<JToken>();
            var elsObj = j.SelectToken(s);
            if (elsObj != null && elsObj.Type != JTokenType.Null)
            {
                switch (elsObj.Type)
                {
                    case JTokenType.Object:
                        els.Add(elsObj);
                        break;
                    case JTokenType.Array:
                        els.AddRange(elsObj);
                        break;
                }
            }

            return els;
        }
    }
}