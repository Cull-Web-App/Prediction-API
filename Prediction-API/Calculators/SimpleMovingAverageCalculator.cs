using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Calculators
{
    public class SimpleMovingAverageCalculator : IMovingAverageCalculator<decimal>
    {
        // This method could end up being slow -- watch out!! O(n^2)!! -- Might need to implement caching to speed this up
        public List<decimal> Calculate(List<decimal> nums, int period)
        {
            List<decimal> movingAverage = new List<decimal>();
            for (int i = 0; i < nums.Count - period; i++)
            {
                // Create the list of the correct period
                movingAverage.Add(nums.GetRange(i, period).Average());
            }

            return movingAverage;
        }
    }
}
