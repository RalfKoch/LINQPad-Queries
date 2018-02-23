<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	Console.WriteLine(GetBitstampTickerData());
	var query = new List<string>();
	while (true)
	{
		query.Add(GetBitstampTickerData().ToString());
		System.Threading.Thread.Sleep(2000);
	}
	
}

// Define other methods and classes here
public BitStampTickerData GetBitstampTickerData()
{
	var url = @"https://www.bitstamp.net/api/v2/ticker/xrpeur/";
	var client = new HttpClient();
	var response = client.GetStringAsync(url);
	var query = JsonConvert.DeserializeObject<BitStampTickerDataJson>(response.Result);

	return ConvertFromJson(query);
}

public class BitStampTickerDataJson
{
	[JsonProperty("ask")]
	public string Ask { get; set; }

	[JsonProperty("bid")]
	public string Bid { get; set; }

	[JsonProperty("high")]
	public string High { get; set; }

	[JsonProperty("last")]
	public string Last { get; set; }

	[JsonProperty("low")]
	public string Low { get; set; }

	[JsonProperty("open")]
	public string Open { get; set; }

	[JsonProperty("timestamp")]
	public string Timestamp { get; set; }

	[JsonProperty("volume")]
	public string Volume { get; set; }

	[JsonProperty("vwap")]
	public string Vwap { get; set; }
}

public BitStampTickerData ConvertFromJson(BitStampTickerDataJson data)
{
	if (data == null) return null;

	var culture = new CultureInfo("en-US");
	var result = new BitStampTickerData
	{
		Oid = Guid.NewGuid(),
		Bid = Convert.ToDecimal(data.Bid, culture),
		Ask = Convert.ToDecimal(data.Ask, culture),
		High = Convert.ToDecimal(data.High, culture),
		Last = Convert.ToDecimal(data.Last, culture),
		Low = Convert.ToDecimal(data.Low, culture),
		Open = Convert.ToDecimal(data.Open, culture),
		Volume = Convert.ToDecimal(data.Volume, culture),
		Vwap = Convert.ToDecimal(data.Vwap, culture),
		Timestamp = data.Timestamp
	};

	return result;
}
public class BitStampTickerData
{
	public Guid Oid { get; set; }

	public decimal Ask { get; set; }

	public decimal Bid { get; set; }

	public decimal High { get; set; }

	public decimal Last { get; set; }

	public decimal Low { get; set; }

	public decimal Open { get; set; }

	public string Timestamp { get; set; }

	public decimal Volume { get; set; }

	public decimal Vwap { get; set; }

	public override string ToString()
	{
		return $"{Oid};{Ask};{Bid};{High};{Last};{Low};{Open};{Timestamp};{Volume};{Vwap}";
	}
}