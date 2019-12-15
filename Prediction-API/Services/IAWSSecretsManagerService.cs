using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Prediction_API.Services
{
    public interface IAWSSecretsManagerService
    {
        public Task<string> GetSecretStringAsync(string secretName, string innerSelector);
        public Task<Dictionary<string, string>> GetSecretSetAsync(string secretSetName);
    }
}
