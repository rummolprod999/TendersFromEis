using System;
using TendersFromEis.Logger;
using TendersFromEis.Parser;

namespace TendersFromEis.Executor
{
    public class Executor
    {
        private readonly IParser _parser;

        public Executor(IParser parser)
        {
            _parser = parser;
        }

        public void ExecuteParser()
        {
            Log.Logger("Start");
            try
            {
                _parser.Parsing();
            }
            catch (Exception e)
            {
                Logger.Log.Logger("Exception in parsing()", e);
            }
            Log.Logger($"Add tenders to excel {ParserAbstract.ListTenders.Count}");
            Log.Logger("End");
        }
    }
}