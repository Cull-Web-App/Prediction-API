using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prediction_API.Models;

namespace Prediction_API.Calculators
{
    public class SimpleLinearRegressionCalculator : ICalculator
    {
        // Calculate the simple linear regression of a set of numbers -- what to return for this calc??
        public BestFitLine Calculate(List<decimal> nums)
        {
            // Get the best fit line -- send the list of x and y values -- this can be used with the regression to calculate predictions at different intervals!!
            return new BestFitLine(Enumerable.Range(0, nums.Count).ToList().ConvertAll(x => (decimal) x), nums);
        }
    }
}
