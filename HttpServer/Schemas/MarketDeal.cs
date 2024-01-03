using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer.Schemas
{
    internal class MarketDeal
    {
        public string Id { get; set; }
        public string CardToSell { get; set; }
        public double Price { get; set; }

        public bool IsValid()
        {
            if (Id == null || CardToSell == null || Price == 0)
            {
                return false;
            }
            return true;
        }
        public void Print()
        {
            if (Id == null)
            {
                Console.WriteLine("Market Deal Id: NULL");
            }
            else
            {
                Console.WriteLine("Market Deal Id {0}", Id);
            }
            if (CardToSell == null)
            {
                Console.WriteLine("Card To Sell Id: NULL");
            }
            else
            {
                Console.WriteLine("Card To Sell Id:: {0}", CardToSell);
            }
            if (Price == 0)
            {
                Console.WriteLine("price: NULL");
            }
            else
            {
                Console.WriteLine("price: {0}", Price);
            }
        }
    }
}
