using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using R5T.Gepidia;
using R5T.Lombardy;

using R5T.Gepidia.Lombardy.Extensions;


namespace R5T.Teutonia.Default
{
    public class DefaultFileSystemCloningOperator : IFileSystemCloningOperator
    {
        private IStringlyTypedPathOperator StringlyTypedPathOperator { get; }
        private IFileSystemCloningDifferencer FileSystemCloningDifferencer { get; }


        public DefaultFileSystemCloningOperator(IStringlyTypedPathOperator stringlyTypedPathOperator, IFileSystemCloningDifferencer fileSystemCloningDifferencer)
        {
            this.StringlyTypedPathOperator = stringlyTypedPathOperator;
            this.FileSystemCloningDifferencer = fileSystemCloningDifferencer;
        }

        public void Clone(FileSystemSite source, FileSystemSite destination, FileSystemCloningOptions options)
        {
            // Ensure the source and destination directories are directory indicated.
            var ensuredSource = source.EnsureSiteDirectoryPathIsDirectoryIndicated(this.StringlyTypedPathOperator);
            var ensuredDestination = destination.EnsureSiteDirectoryPathIsDirectoryIndicated(this.StringlyTypedPathOperator);

            // Get all source file-system entries.
            var sourceFileSystemEntries = ensuredSource.FileSystemOperator.EnumerateFileSystemEntries(ensuredSource.DirectoryPath, true)
                .ToList();

            // Get all destination file-system entries.
            var destinationFileSystemEntries = ensuredDestination.FileSystemOperator.EnumerateFileSystemEntries(ensuredDestination.DirectoryPath, true)
                .ToList();

            // Create relative-path source and destination file-system entries.
            FileSystemEntry MakeRelativeEntry(string baseDirectoryPath, FileSystemEntry entry)
            {
                var sourceBaseDirectoryRelativeEntryPath = this.StringlyTypedPathOperator.GetRelativePath(baseDirectoryPath, entry.Path);

                var relativeEntry = FileSystemEntry.New(sourceBaseDirectoryRelativeEntryPath, entry.Type, entry.LastModifiedUTC);
                return relativeEntry;
            }

            var sourceBaseDirectoryRelativePathEntries = sourceFileSystemEntries.Select(entry => MakeRelativeEntry(ensuredSource.DirectoryPath, entry))
                .Select(fileSystemEntry =>
                {
                    // Make sure we are using a common path format.
                    var standardPathFileSystemEntry = fileSystemEntry.GetStandardPathFormatEntry(this.StringlyTypedPathOperator);
                    return standardPathFileSystemEntry;
                })
                .ToList();

            var destinationBaseDirectoryRelativePathEntries = destinationFileSystemEntries.Select(entry => MakeRelativeEntry(ensuredDestination.DirectoryPath, entry))
                .Select(fileSystemEntry =>
                {
                    // Make sure we are using a common path format.
                    var standardPathFileSystemEntry = fileSystemEntry.GetStandardPathFormatEntry(this.StringlyTypedPathOperator);
                    return standardPathFileSystemEntry;
                })
                .ToList();

            // Write out source and destination data.

            // Get the file-system cloning difference.
            var difference = this.FileSystemCloningDifferencer.PerformDifference(sourceBaseDirectoryRelativePathEntries, destinationBaseDirectoryRelativePathEntries, options);

            // Create a list of operations, using absolute paths.
            var operations = new List<IFileSystemCloningOperation>();

            // Special case: the destination directory does not exist. If so, make sure it is created first to allow files to be copied into it!
            var destinationDirectoryExists = ensuredDestination.FileSystemOperator.ExistsDirectory(ensuredDestination.DirectoryPath);
            if(!destinationDirectoryExists)
            {
                var createDestinationDirectoryOperation = new CreateDirectoryOperation(ensuredDestination.DirectoryPath);

                operations.Add(createDestinationDirectoryOperation);
            }

            foreach (var directoryToCreate in difference.RelativeDirectoryPathsToCreate)
            {
                string destinationDirectoryToCreate = this.StringlyTypedPathOperator.Combine(ensuredDestination.DirectoryPath, directoryToCreate);

                var createDirectoryOperation = new CreateDirectoryOperation(destinationDirectoryToCreate);

                operations.Add(createDirectoryOperation);
            }

            foreach (var directoryToDelete in difference.RelativeDirectoryPathsToDelete)
            {
                string destinationDirectoryToDelete = this.StringlyTypedPathOperator.Combine(ensuredDestination.DirectoryPath, directoryToDelete);

                var deleteDirectoryOperation = new DeleteDirectoryOperation(destinationDirectoryToDelete);

                operations.Add(deleteDirectoryOperation);
            }

            foreach (var fileToCopy in difference.RelativeFilePathsToCopy)
            {
                string sourceFilePath = this.StringlyTypedPathOperator.Combine(ensuredSource.DirectoryPath, fileToCopy);
                string destinationFilePath = this.StringlyTypedPathOperator.Combine(ensuredDestination.DirectoryPath, fileToCopy);

                var copyFileOperation = new CopyFileOperation(sourceFilePath, destinationFilePath);

                operations.Add(copyFileOperation);
            }

            foreach (var fileToUpdate in difference.RelativeFilePathsToUpdate)
            {
                string sourceFilePath = this.StringlyTypedPathOperator.Combine(ensuredSource.DirectoryPath, fileToUpdate);
                string destinationFilePath = this.StringlyTypedPathOperator.Combine(ensuredDestination.DirectoryPath, fileToUpdate);

                var copyFileOperation = new CopyFileOperation(sourceFilePath, destinationFilePath);

                operations.Add(copyFileOperation);
            }

            foreach (var fileToDelete in difference.RelativeFilePathsToDelete)
            {
                string destinationFilePath = this.StringlyTypedPathOperator.Combine(ensuredDestination.DirectoryPath, fileToDelete);

                var deleteFileOperation = new DeleteFileOperation(destinationFilePath);

                operations.Add(deleteFileOperation);
            }

            // Write out and allow approval of the list of operations?

            // Execute the list of operations.
            foreach (var operation in operations)
            {
                operation.Execute(ensuredSource.FileSystemOperator, ensuredDestination.FileSystemOperator);
            }
        }

        public Task CloneAsync(FileSystemSite source, FileSystemSite destination, FileSystemCloningOptions options, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
