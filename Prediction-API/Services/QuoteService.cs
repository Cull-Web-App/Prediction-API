﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Prediction_API.Models;

namespace Prediction_API.Services
{
    public class QuoteService : IQuoteService
    {
        // Keep the injected HTTP client in this service -- should be the typed client for this service!
        private readonly HttpClient httpClient;
        
        public QuoteService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Quote>> GetQuotesAsync(string symbol, string interval = "1y")
        {
            // Construct the request path for this get request -- add query parameters for the get -- should go off the base URI from client
            string requestPath = string.Format("/data?symbol={0}&interval={1}", symbol, interval);

            // Use the typed client to request from FinancialDataAPI the stock data for this time frame!
            HttpResponseMessage response = await this.httpClient.GetAsync(requestPath);

            if (response.IsSuccessStatusCode)
            {
                // Extract the list of the data from the response -- json raw string
                string jsonString = await response.Content.ReadAsStringAsync();

                // Deserialize the json string into the .NET objects
                return JsonConvert.DeserializeObject<List<Quote>>(jsonString);
            }
            else
            {
                // Throw an error if this was not a successful request
                throw new ApplicationException(string.Format("Call to {0} failed with status {1}.", requestPath, response.StatusCode));
            }
        }
    }
}
