using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Lombardy;

using R5T.T0063;


namespace R5T.Teutonia.Default
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="DefaultFileSystemCloningDifferencer"/> implementation of <see cref="IFileSystemCloningDifferencer"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddDefaultFileSystemCloningDifferencer(this IServiceCollection services)
        {
            services.AddSingleton<IFileSystemCloningDifferencer, DefaultFileSystemCloningDifferencer>();

            return services;
        }

        /// <summary>
        /// Adds the <see cref="DefaultFileSystemCloningOperator"/> implementation of <see cref="IFileSystemCloningOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddDefaultFileSystemCloningOperator(this IServiceCollection services,
            IServiceAction<IFileSystemCloningDifferencer> fileSystemCloningDifferencerAction,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            services
                .Run(fileSystemCloningDifferencerAction)
                .Run(stringlyTypedPathOperatorAction)
                .AddSingleton<IFileSystemCloningOperator, DefaultFileSystemCloningOperator>()
                ;

            return services;
        }
    }
}
