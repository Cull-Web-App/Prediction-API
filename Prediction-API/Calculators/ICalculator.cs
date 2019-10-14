using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Calculators
{
    public interface ICalculator
    {
        public T Calculate<T>();
    }
}
