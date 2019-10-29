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

        public AWSConfigurationProvider(IAWSSecretsManagerService awsSecretsManagerService)
        {
            this.awsSecretsManagerService = awsSecretsManagerService;
        }

        public async override void Load()
        {
            // Load the configuration using the AWSSecretsManager Service -- how to get that service?? -- pass in reference
            Data.Add("ConnectionStrings:PredictionStore-Production", await awsSecretsManagerService.GetSecretStringAsync(
                Environment.GetEnvironmentVariable("DB_SECRET_NAME"),
                Environment.GetEnvironmentVariable("DB_NAME")
            ));
        }
    }
}
