using System;
using BooruViewer.Interop.Boorus;
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
                        new UnixDateTimeConverter(),
                    },
                })
            };
        }

        public static IServiceCollection WithAllBoorus(this IServiceCollection services,
            RefitSettings refitSettings = null)
        {
            return services.WithDanbooru()
                .WithYandere()
                .WithDanbooru();
        }

        public static IServiceCollection WithDanbooru(this IServiceCollection services, RefitSettings refitSettings = null)
        {
            services.AddRefitClient<IDanbooruApi>(refitSettings ?? RefitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.DanbooruBaseUrl));

            services.AddSingleton<IBooru, Danbooru>();
            return services;
        }

        public static IServiceCollection WithYandere(this IServiceCollection services, RefitSettings refitSettings = null)
        {
            services.AddRefitClient<IYandereApi>(refitSettings ?? RefitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.YandereBaseUrl));

            services.AddSingleton<IBooru, Yandere>();
            return services;
        }

        public static IServiceCollection WithKonachan(this IServiceCollection services, RefitSettings refitSettings = null)
        {
            services.AddRefitClient<IKonachanApi>(refitSettings ?? RefitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.KonaChanBaseUrl));

            services.AddSingleton<IBooru, KonaChan>();
            return services;
        }
    }
}
