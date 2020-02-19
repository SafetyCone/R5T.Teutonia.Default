using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Lombardy;


namespace R5T.Teutonia.Default.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Also requies a:
        /// * <see cref="IStringlyTypedPathOperator"/>
        /// </summary>
        public static IServiceCollection UseDefaultFileSystemCloningOperator(this IServiceCollection services)
        {
            services
                .AddSingleton<IFileSystemCloningOperator, DefaultFileSystemCloningOperator>()
                .AddSingleton<IFileSystemCloningDifferencer, DefaultFileSystemCloningDifferencer>()
                ;

            return services;
        }
    }
}
