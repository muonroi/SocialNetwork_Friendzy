namespace Infrastructure.Commons;

public static class LocalizationExtensions
{
    public static IServiceCollection AddCultureProviders(this IServiceCollection services)
    {
        _ = services.AddLocalization(options => options.ResourcesPath = "Resources");
        const string viVN = "vi-VN";
        const string enUS = "en-US";

        _ = services.Configure<RequestLocalizationOptions>(options =>
        {
            CultureInfo[] supportedCultures =
            [
                new CultureInfo(viVN),
                new CultureInfo(enUS)
            ];

            options.DefaultRequestCulture = new RequestCulture(viVN);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
            {
                string language = context.Request.Headers.AcceptLanguage.ToString();
                string defaultLanguage = string.IsNullOrEmpty(language) ? viVN : language;
                if (!supportedCultures.Any(s => s.Name.Equals(defaultLanguage)))
                {
                    defaultLanguage = viVN;
                }

                ProviderCultureResult providerCultureResult = new(defaultLanguage, defaultLanguage);
                return await Task.FromResult<ProviderCultureResult?>(providerCultureResult);
            }));
        });

        return services;
    }

    public static IApplicationBuilder UseCultureProviders(this IApplicationBuilder app)
    {
        IOptions<RequestLocalizationOptions>? localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        if (localizationOptions is not null)
        {
            localizationOptions.Value.ApplyCurrentCultureToResponseHeaders = true;
            _ = app.UseRequestLocalization(localizationOptions.Value);
        }

        return app;
    }
}