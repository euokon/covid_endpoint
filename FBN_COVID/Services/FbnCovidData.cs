using FBN_COVID.DataObject;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBN_COVID.Services
{
    public interface IFbnCovidData
    {
        public CovidSummary GetCovidData();
    }

    public class FbnCovidData: IFbnCovidData
    {
        private readonly IConfiguration configuration;

        public FbnCovidData(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public CovidSummary GetCovidData()
        {
            string baseUrl = configuration.GetValue<string>("AppSettings:CovidBaseUrl");

            CovidResponse covidResponse = new CovidResponse();

            CovidSummary covidSummary = new CovidSummary();

            var client = new RestClient(baseUrl);
            var request = new RestRequest(Method.GET);

            var response = client.Execute(request);

            covidResponse = JsonConvert.DeserializeObject<CovidResponse>(response.Content);
            covidSummary = JsonConvert.DeserializeObject<CovidSummary>(covidResponse.Data.ToString());

            return covidSummary;
        }

    }
}
