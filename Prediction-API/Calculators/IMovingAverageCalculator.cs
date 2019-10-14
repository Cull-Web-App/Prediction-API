using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Calculators
{
    public interface IMovingAverageCalculator<T>
    {
        public List<T> Calculate(List<T> nums, int period);
    }
}
