using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Prediction_API.Services
{
    public class AWSConfigurationProvider : ConfigurationProvider
    {
        private readonly IAWSSecretsManagerService awsSecretsManagerService;
        private readonly IConnectionService connectionService;

        public AWSConfigurationProvider(IAWSSecretsManagerService awsSecretsManagerService, IConnectionService connectionService)
        {
            this.awsSecretsManagerService = awsSecretsManagerService;
            this.connectionService = connectionService;
        }

        public async override void Load()
        {
            // Load the configuration using the AWSSecretsManager Service -- how to get that service?? -- pass in reference
            Dictionary<string, string> dbSecretSet = await this.awsSecretsManagerService.GetSecretSetAsync(Environment.GetEnvironmentVariable("CONNECTION_SECRETS"));
            Data.Add("ConnectionStrings:PredictionStore-Production", this.connectionService.CreateDBConnectionString(
                dbSecretSet["DB_USERNAME"],
                dbSecretSet["DB_PASSWORD"],
                dbSecretSet["DB_SERVER"],
                Environment.GetEnvironmentVariable("DB_NAME"),
                5542
            ));
        }
    }
}
