using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

namespace MinhaAcademiaTEM.API.Extensions;

public static class CompressionExtensions
{
    public static void ConfigureCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

        services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });
    }
}