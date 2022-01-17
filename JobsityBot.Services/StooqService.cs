using CsvHelper;
using JobsityBot.Core;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Globalization;

namespace JobsityBot.Services;

public interface IStooqService
{
    Task<string> GetData(string code);
}

public class StooqService : IStooqService
{
    private readonly StooqOptions options;

    public StooqService(IOptions<StooqOptions> options)
    {
        this.options = options.Value;
    }

    public async Task<string> GetData(string code)
    {
        var url = string.Format(this.options.Url!, code);
        var client = new RestClient(url);
        var request = new RestRequest(string.Empty, Method.Get);
        var response = await client.ExecuteAsync(request);
        var data = response.IsSuccessful ?
           ParseCsvFile(response.Content!) :
            "I could not retrieve data, sorry";

        return data;
    }

    private static string ParseCsvFile(string response)
    {
        try
        {
            var record = new StockData();
            using (var stringReader = new StringReader(response))
            using (var csvReader = new CsvReader(stringReader, CultureInfo.CurrentCulture))
            {
                record = csvReader.GetRecords<StockData>().FirstOrDefault();
            }
            return record == null ?
                "I could not find data" :
                $"{record.Symbol} quote is {record.Open} per share";
        }
        catch
        {
            return "Api returned an invalid CSV. Could not parse data";
        }

    }
}
