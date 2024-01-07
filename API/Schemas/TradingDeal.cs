using System;


namespace MTCG.HttpServer.Schemas
{
    // enum to enforce the OpenAPI specification for the trading deals
    internal enum Type
    {
        monster, spell
    }

    public class TradingDeal
    {
        public string Id { get; set; }
        public string CardToTrade { get; set; }
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
    }
}
