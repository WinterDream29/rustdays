using System.Collections.Generic;

namespace Assets.Scripts
{
    public class IapItemDefinition
    {
        public string Description { get; private set; }
        public string DetailDescription { get; private set; }
        public string IconName { get; private set; }
        public Dictionary<string, int> Items { get; private set; }
        public int Currency { get; private set; }
        public int OfferPercent { get; private set; }
        public bool HotDeal { get; private set; }
        public bool MostPopular { get; private set; }

        public IapItemDefinition(int currency, string descr, string icon, Dictionary<string, int> items = null, int offerPercent = 0, bool hotDeal = false, bool mostPopular = false, string detailDescr = null)
        {
            Currency = currency;
            IconName = icon;
            Description = descr;
            Items = items;
            OfferPercent = offerPercent;
            HotDeal = hotDeal;
            MostPopular = mostPopular;
            DetailDescription = detailDescr;
            if (string.IsNullOrEmpty(detailDescr))
                DetailDescription = Description;
        }
    }
}
