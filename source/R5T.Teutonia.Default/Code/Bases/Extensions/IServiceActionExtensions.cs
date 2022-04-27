using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Lombardy;

using R5T.T0062;
using R5T.T0063;


namespace R5T.Teutonia.Default
{
    public static class IServiceActionExtensions
    {
        /// <summary>
        /// Adds the <see cref="DefaultFileSystemCloningDifferencer"/> implementation of <see cref="IFileSystemCloningDifferencer"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IFileSystemCloningDifferencer> AddDefaultFileSystemCloningDifferencerAction(this IServiceAction _)
        {
            var serviceAction = _.New<IFileSystemCloningDifferencer>(services => services.AddDefaultFileSystemCloningDifferencer());
            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="DefaultFileSystemCloningOperator"/> implementation of <see cref="IFileSystemCloningOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IFileSystemCloningOperator> AddDefaultFileSystemCloningOperatorAction(this IServiceAction _,
            IServiceAction<IFileSystemCloningDifferencer> fileSystemCloningDifferencerAction,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var serviceAction = _.New<IFileSystemCloningOperator>(services => services.AddDefaultFileSystemCloningOperator(
                fileSystemCloningDifferencerAction,
                stringlyTypedPathOperatorAction));

            return serviceAction;
        }
    }
}
