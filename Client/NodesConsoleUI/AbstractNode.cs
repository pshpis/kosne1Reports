using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.NodesConsoleUI
{
    public abstract class AbstractNode
    {
        private readonly int _menuPointsCount;
        protected static string ServerPath { get; } = "http://localhost:5194";
        public static HttpClient Client { get; } = new();

        public AbstractNode(AbstractNode parentNode, int menuPointsCount)
        {
            _parentNode = parentNode;
            _menuPointsCount = menuPointsCount; 
        }

        private AbstractNode _parentNode { get; set; }

        public abstract Task Launch();

        protected int ReadMenuPoint(int maxPoint = -1)
        {
            if (maxPoint < 0) maxPoint = _menuPointsCount;

            int point = -1;
            while (true)
            {
                try
                {
                    string value = Console.ReadLine();
                    point = Convert.ToInt32(value.Trim());
                    if (!(point >= 1 && point <= maxPoint))
                        throw new FormatException("Wrong format of menu point");
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please write number from 1 to " + _menuPointsCount);
                }
            }

            return point;
        }
        
        public async Task Exit()
        {
            await _parentNode.Launch();
        }

    }
}