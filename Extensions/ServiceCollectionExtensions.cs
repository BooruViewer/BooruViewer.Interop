using System;
using BooruViewer.Interop.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;

namespace BooruViewer.Interop.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static RefitSettings RefitSettings;

        static ServiceCollectionExtensions()
        {
            RefitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy(),
                    },
                    Converters =
                    {
                        new StringEnumConverter(),
                    },
                })
            };
        }

        public static IServiceCollection WithDanbooru(this IServiceCollection services, RefitSettings refitSettings = null)
        {
            services.AddRefitClient<IDanbooruApi>(refitSettings ?? RefitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.DanbooruBaseUrl));
            return services;
        }
    }
}
