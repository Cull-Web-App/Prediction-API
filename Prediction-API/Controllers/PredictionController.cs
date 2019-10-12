using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prediction_API.Models;
using Prediction_API.Services;

namespace Prediction_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IStockTickerService stockTickerService;

        public PredictionController(IStockTickerService stockTickerService)
        {
            this.stockTickerService = stockTickerService;
        }

        // POST: Prediction -- Create a new prediction and update it in the DB
        [HttpPost]
        public void Post([FromBody] string body)
        {
        }
    }
}
