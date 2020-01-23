using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using TendersFromEis.NetworkLibrary;
using Log = TendersFromEis.Logger.Log;

namespace TendersFromEis.Parser
{
    public class ParserWeb44: ParserAbstract, IParser

    {
        

        public ParserWeb44(): base()
        {
        }

        public void Parsing()
        {
            Initialize();
            for (var i = 1; i <= _pageCount; i++)
            {
                try
                {
                    ParsingPage(i);
                }
                catch (Exception e)
                {
                    Log.Logger(e);
                }
            }
        }

        private void ParsingPage(int i)
        {
            var currUrl = CreateCurrentUrl(_currentUrl, i);
            Console.WriteLine(currUrl);
        }
        
    }
}