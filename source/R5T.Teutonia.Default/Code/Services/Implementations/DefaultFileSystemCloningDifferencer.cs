using System;
using System.Collections.Generic;
using System.Linq;

using R5T.Gepidia;

using R5T.T0064;


namespace R5T.Teutonia.Default
{
    /// <summary>
    /// Assumes that all paths, both source and destination, are relative to the same base path.
    /// </summary>
    [ServiceImplementationMarker]
    public class DefaultFileSystemCloningDifferencer : IFileSystemCloningDifferencer, IServiceImplementation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Ok to assume that we can delete files and directories in directories that are already deleted. (Operations will be executed in idempotent manner.)
        /// </remarks>
        public FileSystemCloningDifference PerformDifference(
            IEnumerable<FileSystemEntry> sourceFileSystemEntries,
            IEnumerable<FileSystemEntry> destinationFileSystemEntries,
            FileSystemCloningOptions options)
        {
            var sourceDirectoryEntries = sourceFileSystemEntries.Where(x => x.IsDirectory());
            var sourceFileEntries = sourceFileSystemEntries.Where(x => x.IsFile());
            var destinationDirectoryEntries = destinationFileSystemEntries.Where(x => x.IsDirectory());
            var destinationFileEntries = destinationFileSystemEntries.Where(x => x.IsFile());

            var pathOnlyEqualityComparer = new PathOnlyFileSystemEntryEqualityComparer();

            var difference = new FileSystemCloningDifference();

            // Create all directories that exist in the source, but not in the destination.
            var directoriesToCreate = sourceDirectoryEntries.Except(destinationDirectoryEntries, pathOnlyEqualityComparer);

            difference.RelativeDirectoryPathsToCreate.AddRange(directoriesToCreate.Select(x => x.Path));

            if(options.DeleteExtraneousDestinationDirectories)
            {
                // Delete all directories that exist in the destination, but not in the source. (Make this an option!)
                var directoriesToDelete = destinationDirectoryEntries.Except(sourceDirectoryEntries, pathOnlyEqualityComparer);

                difference.RelativeDirectoryPathsToDelete.AddRange(directoriesToDelete.Select(x => x.Path));
            }

            // Copy all files that exist in the source, but not in the destination.
            var filesToCopy = sourceFileEntries.Except(destinationFileEntries, pathOnlyEqualityComparer);

            difference.RelativeFilePathsToCopy.AddRange(filesToCopy.Select(x => x.Path));

            // Copy all files that exist in both the source and the destination, but the last modified date in the source is greater than the last modified date in the destination.
            var filesToUpdate = sourceFileEntries
                .Join(destinationFileEntries, source => source.Path, destination => destination.Path, (source, destination) => (Destination: destination, Source: source))
                .Where(x => x.Destination.LastModifiedUTC < x.Source.LastModifiedUTC)
                .Select(x => x.Source);

            difference.RelativeFilePathsToUpdate.AddRange(filesToUpdate.Select(x => x.Path));

            if (options.DeleteExtraneousDestinationFiles)
            {
                // Delete all files that are in the destination, but not the source. (Make this an option!)
                var filesToDelete = destinationFileEntries.Except(sourceFileEntries, pathOnlyEqualityComparer);

                difference.RelativeFilePathsToDelete.AddRange(filesToDelete.Select(x => x.Path));
            }

            return difference;
        }
    }
}
