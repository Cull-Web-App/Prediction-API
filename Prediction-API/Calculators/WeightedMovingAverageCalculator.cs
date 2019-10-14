using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Calculators
{
    public class WeightedMovingAverageCalculator : IMovingAverageCalculator<decimal>
    {
        // Again, these weighted averages arent going to scale well. Need to Dynamically program this most likely
        public List<decimal> Calculate(List<decimal> nums, int period)
        {
            List<decimal> weightedMovingAverage = new List<decimal>();
            for (int i = 0; i < nums.Count - period; i++)
            {
                // Get the range values
                List<decimal> periodValues = nums.GetRange(i, period);
                decimal sum = 0;
                for (int j = 0; j < periodValues.Count; j++)
                {
                    sum += periodValues[j] * (period - j);
                }
                weightedMovingAverage.Add(sum / (period * (period + 1) / 2));
            }
            return weightedMovingAverage;
        }
    }
}
