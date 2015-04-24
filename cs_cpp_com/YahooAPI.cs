using System.Net; // WebClient
using System.Globalization; // CultureInfo
using System.Runtime.InteropServices;
 
public class YahooAPI
{
    public delegate void PrintCallback(string str);

    private static int instanceCounter = 0;

    private static readonly WebClient webClient = new WebClient();
 
    private const string UrlTemplate = "http://download.finance.yahoo.com/d/quotes.csv?s={0}&f={1}";
 
    public YahooAPI() 
    {
        instanceCounter++;
    }
 
    private double ParseDouble(string value)
    {
         return double.Parse(value.Trim(), CultureInfo.InvariantCulture);
    }
 
    private string[] GetDataFromYahoo(string symbol, string fields)
    {
        string request = string.Format(UrlTemplate, symbol, fields);
 
        string rawData = webClient.DownloadString(request).Trim();
 
        return rawData.Split(',');
    }
 
    public double GetBid(string symbol)
    {
        return ParseDouble(GetDataFromYahoo(symbol, "b")[0]);
    }
 
    public double GetAsk(string symbol)
    {
        return ParseDouble(GetDataFromYahoo(symbol, "a")[0]);
    }
 
    public string GetCapitalization(string symbol)
    {
        return GetDataFromYahoo(symbol, "j1")[0];
    }
 
    public string[] GetValues(string symbol, string fields)
    {
        return GetDataFromYahoo(symbol, fields);
    } 
    
    public int GetStringLength(string str)
    {
        return str.Length;
    }
    
    public string test()
    {
        return "YahooAPI library";
    }
    
    public void test(string mode, [MarshalAs(UnmanagedType.FunctionPtr)] PrintCallback cb)
    {
        cb(test() + " - C# library with " + mode + " - Instances: " + instanceCounter);
    }
}