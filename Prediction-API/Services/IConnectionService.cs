using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Services
{
    public interface IConnectionService
    {
        public string CreateDBConnectionString(string dbUsername, string dbPassword, string dbServer, string dbName, int portNum);
    }
}
