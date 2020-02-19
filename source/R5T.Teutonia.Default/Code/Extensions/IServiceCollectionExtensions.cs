using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Lombardy;


namespace R5T.Teutonia.Default.Extensions
{
    public static class IServiceCollectionExtensions
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
        /// Adds the <see cref="DefaultFileSystemCloningDifferencer"/> implementation of <see cref="IFileSystemCloningDifferencer"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static ServiceAction<IFileSystemCloningDifferencer> AddDefaultFileSystemCloningDifferencerAction(this IServiceCollection services)
        {
            var serviceAction = new ServiceAction<IFileSystemCloningDifferencer>(() => services.AddDefaultFileSystemCloningDifferencer());
            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="DefaultFileSystemCloningOperator"/> implementation of <see cref="IFileSystemCloningOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddDefaultFileSystemCloningOperator(this IServiceCollection services,
            ServiceAction<IFileSystemCloningDifferencer> addFileSystemCloningDifferencer,
            ServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            services
                .AddSingleton<IFileSystemCloningOperator, DefaultFileSystemCloningOperator>()
                .RunServiceAction(addFileSystemCloningDifferencer)
                .RunServiceAction(addStringlyTypedPathOperator)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="DefaultFileSystemCloningOperator"/> implementation of <see cref="IFileSystemCloningOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static ServiceAction<IFileSystemCloningOperator> AddDefaultFileSystemCloningOperatorAction(this IServiceCollection services,
            ServiceAction<IFileSystemCloningDifferencer> addFileSystemCloningDifferencer,
            ServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            var serviceAction = new ServiceAction<IFileSystemCloningOperator>(() => services.AddDefaultFileSystemCloningOperator(
                addFileSystemCloningDifferencer,
                addStringlyTypedPathOperator));
            return serviceAction;
        }
    }
}
