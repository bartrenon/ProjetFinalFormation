namespace Infrastructure.Dtos.Card;

public class TcgDexCardDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public TcgDexPricingDto? Pricing { get; set; }
}

public class TcgDexPricingDto
{
    public TcgDexCardmarketDto? Cardmarket { get; set; }
    public TcgDexTcgplayerDto? Tcgplayer { get; set; }
}

public class TcgDexCardmarketDto
{
    public decimal? Avg30 { get; set; }
    public decimal? Avg { get; set; }
}

public class TcgDexTcgplayerDto
{
    public TcgDexTcgVariantDto? Normal { get; set; }
}

public class TcgDexTcgVariantDto
{
    public decimal? MarketPrice { get; set; }
}
