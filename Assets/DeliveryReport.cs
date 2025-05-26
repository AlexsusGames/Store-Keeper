using System.Collections.Generic;

public struct DeliveryReport 
{
    public Dictionary<string, float> Products;

    public bool HasSpoiled;
    public bool HasPriceChanged;

    public bool IsDeliverySent;

    public DeliveryReport(bool isSuccess = false)
    {
        IsDeliverySent = isSuccess;

        Products = null;
        HasSpoiled = false;
        HasPriceChanged = false;
    }

    public DeliveryReport(Dictionary<string, float> products, bool hasSpoiled, bool hasPriceChanged)
    {
        Products = products;
        HasSpoiled = hasSpoiled;
        HasPriceChanged = hasPriceChanged;

        IsDeliverySent = true;
    }
}
