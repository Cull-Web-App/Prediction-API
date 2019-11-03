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
        private readonly IConnectionService connectionService;

        public AWSConfigurationSource(IAWSSecretsManagerService awsSecretsManagerService, IConnectionService connectionService)
        {
            this.awsSecretsManagerService = awsSecretsManagerService;
            this.connectionService = connectionService;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AWSConfigurationProvider(this.awsSecretsManagerService, this.connectionService);
        }
    }
}
