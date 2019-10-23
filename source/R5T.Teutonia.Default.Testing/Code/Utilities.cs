using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Gepidia;
using R5T.Gepidia.Local;
using R5T.Lombardy;

using R5T.Teutonia.Default.Extensions;


namespace R5T.Teutonia.Default.Testing
{
    public static class Utilities
    {
        public static readonly IServiceProvider ServiceProvider = Utilities.GetServiceProvider();


        public static IServiceProvider GetServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<LocalFileSystemOperator>()
                .AddSingleton<IStringlyTypedPathOperator, StringlyTypedPathOperator>()
                .UseDefaultFileSystemCloningOperator()

                .BuildServiceProvider()
                ;

            return serviceProvider;
        }

        private static FileSystemSite GetFileSystemSite(string directoryPath, IFileSystemOperator fileSystemOperator, IServiceProvider serviceProvider)
        {
            var stringlyTypedPathOperator = serviceProvider.GetRequiredService<IStringlyTypedPathOperator>();

            var ensuredDirectoryPath = stringlyTypedPathOperator.EnsureDirectoryPathIsDirectoryIndicated(directoryPath);

            var site = new FileSystemSite(ensuredDirectoryPath, fileSystemOperator);
            return site;
        }

        public static FileSystemSite GetLocalSourceSite(IServiceProvider serviceProvider)
        {
            var directoryPath = Constants.LocalSourceDirectoryPath;
            var localFileSystemOperator = serviceProvider.GetRequiredService<LocalFileSystemOperator>();

            var output = Utilities.GetFileSystemSite(directoryPath, localFileSystemOperator, serviceProvider);
            return output;
        }

        public static FileSystemSite GetLocalDestinationSite(IServiceProvider serviceProvider)
        {
            var directoryPath = Constants.LocalDestinationDirectoryPath;
            var localFileSystemOperator = serviceProvider.GetRequiredService<LocalFileSystemOperator>();

            var output = Utilities.GetFileSystemSite(directoryPath, localFileSystemOperator, serviceProvider);
            return output;
        }

        public static FileSystemSite GetRemoteDestinationSite(IServiceProvider serviceProvider)
        {
            var directoryPath = Constants.RemoteDestinationDirectoryPath;
            var localFileSystemOperator = serviceProvider.GetRequiredService<>();

            var output = Utilities.GetFileSystemSite(directoryPath, localFileSystemOperator, serviceProvider);
            return output;
        }
    }
}
