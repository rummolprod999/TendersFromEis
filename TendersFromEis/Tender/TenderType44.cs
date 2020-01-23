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
            throw new System.NotImplementedException();
        }
    }
}