using System;

namespace Owin.Security.Providers.Deezer
{
    public static class DeezerAuthenticationExtensions
    {
        public static IAppBuilder UseDeezerAuthentication(this IAppBuilder app,
            DeezerAuthenticationOptions options)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            app.Use(typeof(DeezerAuthenticationMiddleware), app, options);

            return app;
        }

        public static IAppBuilder UseDeezerAuthentication(this IAppBuilder app, string appId, string secretKey)
        {
            return app.UseDeezerAuthentication(new DeezerAuthenticationOptions
            {
                AppId = appId,
                SecretKey = secretKey
            });
        }
    }
}