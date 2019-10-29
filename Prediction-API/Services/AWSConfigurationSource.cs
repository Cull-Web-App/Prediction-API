using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Prediction_API.Services
{
    public class AWSConfigurationSource : IConfigurationSource
    {
        private readonly IAWSSecretsManagerService awsSecretsManagerService;

        public AWSConfigurationSource(IAWSSecretsManagerService awsSecretsManagerService)
        {
            this.awsSecretsManagerService = awsSecretsManagerService;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AWSConfigurationProvider(this.awsSecretsManagerService);
        }
    }
}
