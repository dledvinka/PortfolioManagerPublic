namespace PortfolioManager.Core;

public static class Assets
{
    public static Asset ADA = new("ADA", false, "cardano");
    public static Asset BAT = new("BAT", false, "basic-attention-token");
    public static Asset BNB = new("BNB", false, "binancecoin");
    public static Asset BTC = new("BTC", false, "bitcoin");
    public static Asset CZK = new("CZK", true, string.Empty);
    public static Asset EOS = new("EOS", false, "eos");
    public static Asset ETH = new("ETH", false, "ethereum");
    public static Asset ETH2S = new("ETH2S", false, "seth2");
    public static Asset EUR = new("EUR", true, string.Empty);
    public static Asset FIRO = new("FIRO", false, "zcoin");
    public static Asset ICX = new("ICX", false, "icon");
    public static Asset IOTA = new("IOTA", false, "iota");
    public static Asset LINK = new("LINK", false, "chainlink");
    public static Asset LTC = new("LTC", false, "litecoin");
    public static Asset MKR = new("MKR", false, "maker");
    public static Asset NEO = new("NEO", false, "neo");
    public static Asset OMG = new("OMG", false, "omisego");
    public static Asset PAY = new("PAY", false, "tenx");
    public static Asset SNX = new("SNX", false, "havven");
    public static Asset SUB = new("SUB", false, "substratum");
    public static Asset Undefined = new("Undefined", false, string.Empty);
    public static Asset UNI = new("UNI", false, "uniswap");
    public static Asset USD = new("USD", true, string.Empty);
    public static Asset USDT = new("USDT", false, "tether");
    public static Asset VTC = new("VTC", false, "vertcoin");
    public static Asset WABI = new("WABI", false, "wabi");
    public static Asset XLM = new("XLM", false, "stellar");
    public static Asset XMR = new("XMR", false, "monero");
    public static Asset XRP = new("XRP", false, "ripple");
    public static Asset XVG = new("XVG", false, "verge");
    public static Asset YFI = new("YFI", false, "yearn-finance");

    public static List<Asset> All =>
        new()
        {
            CZK,
            EUR,
            BTC,
            XVG,
            ADA,
            XRP,
            NEO,
            LTC,
            ETH,
            ETH2S,
            BAT,
            FIRO,
            VTC,
            OMG,
            PAY,
            SUB,
            WABI,
            ICX,
            XLM,
            USDT,
            XMR,
            EOS,
            IOTA,
            BNB,
            MKR,
            UNI,
            YFI,
            LINK,
            SNX,
            USD
        };
}