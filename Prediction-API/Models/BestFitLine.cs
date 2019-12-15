using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Models
{
    public class BestFitLine
    {
        public decimal YIntercept { get; set; }
        public decimal Slope { get; set; }

        public BestFitLine(List<decimal> xValues, List<decimal> yValues)
        {
            if (xValues.Count != yValues.Count)
            {
                throw new ApplicationException("Number of x and y values must be the same");
            }

            int count = xValues.Count;
            decimal sumX = 0;
            decimal sumY = 0;
            decimal sumXSquared = 0;
            decimal sumXY = 0;


            // Calculate best fit from the input parameters
            for (int i = 0; i < count; i++)
            {
                sumX += xValues[i];
                sumY += yValues[i];
                sumXSquared += xValues[i] * xValues[i];
                sumXY += xValues[i] * yValues[i];
            }

            // Call the helper methods to set the slope calcs
            this.Slope = this.SlopeCalc(sumX, sumY, sumXSquared, sumXY, count);
            this.YIntercept = this.InterceptCalc(sumX, sumY, sumXSquared, sumXY, count);
        }

        public decimal ApproximateValueAtX(decimal xValue)
        {
            return this.Slope * xValue + this.YIntercept;
        }

        private decimal SlopeCalc(decimal sumX, decimal sumY, decimal sumXSquared, decimal sumXY, int count)
        {
            return (count * sumXY - sumX * sumY) / (count * sumXSquared - sumX * sumX);
        }

        private decimal InterceptCalc(decimal sumX, decimal sumY, decimal sumXSquared, decimal sumXY, int count)
        {
            return (sumY * sumXSquared - sumX * sumXY) / (count * sumXSquared - sumX * sumX);
        }
    }
}
