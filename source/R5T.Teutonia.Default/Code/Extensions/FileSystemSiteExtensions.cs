using System;

using R5T.Gepidia;
using R5T.Lombardy;


namespace R5T.Teutonia.Default.Extensions
{
    public static class FileSystemSiteExtensions
    {
        public static FileSystemSite EnsureSiteDirectoryPathIsDirectoryIndicated(this FileSystemSite site, IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            var directoryPathIsDirectoryIndicated = stringlyTypedPathOperator.IsDirectoryIndicatedPath(site.DirectoryPath);
            if(directoryPathIsDirectoryIndicated)
            {
                return site;
            }
            else
            {
                var directoryIndicatedDirectoryPath = stringlyTypedPathOperator.EnsureDirectoryPathIsDirectoryIndicated(site.DirectoryPath);

                var output = new FileSystemSite(directoryIndicatedDirectoryPath, site.FileSystemOperator);
                return output;
            }
        }
    }
}
