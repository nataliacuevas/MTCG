using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace MTCG.HttpServer.Schemas
{

    internal class TradingDeal
    {
        public string Id {  get; set; }
        public string CardToTrade {  get; set; }
        private Type? _cType;
        //implemented custom setter to convert from string to Enum using JSON.NET
        public String Type
        {
            get
            {
                if (_cType == null)
                {
                    return null;
                }
                else
                {
                    return _cType.ToString();
                }
            }
            set
            {
                Type TypeIntermediate;
                bool result = Enum.TryParse<Type>(value, out TypeIntermediate);
                if (!result)
                {
                    _cType = null;
                }
                else
                {
                    _cType = TypeIntermediate;
                }
            }
        }
        public Double? MinimumDamage { get; set; }
        public bool IsValid()
        {
            if (_cType == null || Id == null || MinimumDamage == null)
            {
                return false;
            }
            return true;
        }
        public void Print()
        {
            if (Id == null)
            {
                Console.WriteLine("Trading Deal Id: NULL");
            }
            else
            {
                Console.WriteLine("Trading Deal Id {0}", Id);
            }
            if (CardToTrade == null)
            {
                Console.WriteLine("Card To Trade Id: NULL");
            }
            else
            {
                Console.WriteLine("Card To Trade Id:: {0}", CardToTrade);
            }
            if (_cType == null)
            {
                Console.WriteLine("cType: NULL");
            }
            else
            {
                Console.WriteLine("cType: {0}", _cType);
            }
            
            if (MinimumDamage == null)
            {
                Console.WriteLine("Minimum Damage: NULL");
            }
            else
            {
                Console.WriteLine("Minimum Damage: {0}", MinimumDamage);
            }
        }
    }
}
