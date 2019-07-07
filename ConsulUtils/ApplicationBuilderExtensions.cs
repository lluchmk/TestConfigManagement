using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ConsulUtils
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            var consulService = app.ApplicationServices
                .GetRequiredService<IConsulService>();

            lifetime.ApplicationStarted.Register(() =>
            {
                var address = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
                var url = new Uri(address);

                consulService.Register(url.Host, url.Port).GetAwaiter().GetResult();
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                consulService.Deregister().GetAwaiter().GetResult();
            });

            return app;
        }
    }
}