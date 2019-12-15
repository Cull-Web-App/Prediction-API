using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;

namespace Prediction_API.Services
{
    public class AWSSecretsManagerService : IAWSSecretsManagerService
    {
        private readonly AmazonSecretsManagerClient client;

        public AWSSecretsManagerService()
        {
            this.client = new AmazonSecretsManagerClient(RegionEndpoint.USEast2);
        }

        public async Task<string> GetSecretStringAsync(string secretName, string innerSelector)
        {
            return (await this.GetSecretSetAsync(secretName))[innerSelector];
        }

        public async Task<Dictionary<string, string>> GetSecretSetAsync(string secretSetName)
        {
            GetSecretValueRequest request = new GetSecretValueRequest
            {
                // Name for the secret in AWS
                SecretId = secretSetName
            };

            GetSecretValueResponse response = await client.GetSecretValueAsync(request);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.SecretString);
        }
    }
}
