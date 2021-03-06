﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prediction_API.Models;

namespace Prediction_API.Services
{
    public interface IQuoteService
    {
        public Task<List<Quote>> GetQuotesAsync(string symbol, string interval);
    }
}
