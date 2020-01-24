using TendersFromEis.BuilderApp;
using TendersFromEis.Parser;

namespace TendersFromEis
{
    static class Program
    {

        static void Main(string[] args)
        {
            Init();
            Parser();
        }

        private static void Init()
        {
            Builder.GetBuilder();
        }

        private static void Parser()
        {
            var executor = new Executor.Executor(new ParserWeb44());
            executor.ExecuteParser();
        }
    }
}