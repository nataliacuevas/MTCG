namespace MTCG.HttpServer.Schemas
{
    public class MarketDeal
    {
        public string Id { get; set; }
        public string CardToSell { get; set; }
        public int? Price { get; set; }

        public bool IsValid()
        {
            if (Id == null || CardToSell == null || Price == null || Price <= 0)
            {
                return false;
            }
            return true;
        }

    }
}
