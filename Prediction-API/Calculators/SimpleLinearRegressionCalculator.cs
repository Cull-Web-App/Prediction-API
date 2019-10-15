using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prediction_API.Models;

namespace Prediction_API.Calculators
{
    public class SimpleLinearRegressionCalculator : ICalculator
    {
        private BestFitLine BestFitLine { get; set; }

        public SimpleLinearRegressionCalculator(List<decimal> nums)
        {
            this.BestFitLine = new BestFitLine(Enumerable.Range(0, nums.Count).ToList().ConvertAll(x => (decimal)x), nums);
        }

        // Calculate the best approximation using tthe best fit line
        public decimal Calculate(int index)
        {
            return this.BestFitLine.ApproximateValueAtX(index);
        }
    }
}
