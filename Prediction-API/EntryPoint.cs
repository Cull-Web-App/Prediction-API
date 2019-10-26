using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.Json;

namespace Prediction_API
{
    /// <summary>
    /// The Main function can be used to run the ASP.NET Core application locally using the Kestrel webserver.
    /// </summary>
    public class EntryPoint
    {
        public static async void Main(string[] args)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME")))
            {
                // Run the application in local mode -- this will run ASP.NET Core 3.0 in kestrel
                BuildWebHost(args).Run();
            }
            else
            {
                // Bootstrap .NET Core 3.0 onto AWS Lambda and run the handler loop
                LambdaEntryPoint lambdaEntry = new LambdaEntryPoint();
                Func<APIGatewayProxyRequest, ILambdaContext, Task<APIGatewayProxyResponse>> functionHandler = lambdaEntry.FunctionHandlerAsync;
                using HandlerWrapper handlerWrapper = HandlerWrapper.GetHandlerWrapper(functionHandler, new JsonSerializer());
                using LambdaBootstrap bootstrap = new LambdaBootstrap(handlerWrapper);
                await bootstrap.RunAsync();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
