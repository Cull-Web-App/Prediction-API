using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Conventions;

namespace Prediction_API.Models
{
    public class PropertyTransformConvention : Convention
    {
        public PropertyTransformConvention()
        {
            // This is a property transformation that will map PriceInput => Price_Input for the col names. Just need it to lowercase
            Properties()
                .Configure(c => c.Transform(s => Regex.Replace(input: s, pattern: "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", replacement: "$1$3_$2$4".ToLower())));
        }
    }
}
