using System;
using System.Collections.Generic;
using System.Linq;

using R5T.Gepidia;
using R5T.Lombardy;


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
            var sourceDirectoryPath = this.StringlyTypedPathOperator.EnsureDirectoryPathIsDirectoryIndicated(source.DirectoryPath);

            // Get all source file-system entries.
            var sourceFileSystemEntries = source.FileSystemOperator.EnumerateFileSystemEntries(sourceDirectoryPath);

            // Get all destination file-system entries.
            var destinationFileSystemEntries = destination.FileSystemOperator.EnumerateFileSystemEntries(destination.DirectoryPath);

            // Create relative-path source and destination file-system entries.
            FileSystemEntry MakeRelativeEntry(string baseDirectoryPath, FileSystemEntry entry)
            {
                var sourceBaseDirectoryRelativeEntryPath = this.StringlyTypedPathOperator.GetRelativePath(source.DirectoryPath, entry.Path);

                var relativeEntry = FileSystemEntry.New(sourceBaseDirectoryRelativeEntryPath, entry.Type, entry.LastModifiedUTC);
                return relativeEntry;
            }

            var sourceBaseDirectoryRelativePathEntries = sourceFileSystemEntries.Select(entry => MakeRelativeEntry(source.DirectoryPath, entry));
            var destinationBaseDirectoryRelativePathEntries = destinationFileSystemEntries.Select(entry => MakeRelativeEntry(source.DirectoryPath, entry));

            // Get the file-system cloning difference.
            var difference = this.FileSystemCloningDifferencer.PerformDifference(sourceBaseDirectoryRelativePathEntries, destinationBaseDirectoryRelativePathEntries, options);

            // Create a list of operations, using absolute paths.
            var operations = new List<IFileSystemCloningOperation>();

            foreach (var directoryToCreate in difference.RelativeDirectoryPathsToCreate)
            {
                string destinationDirectoryToCreate = this.StringlyTypedPathOperator.Combine(destination.DirectoryPath, directoryToCreate);

                var createDirectoryOperation = new CreateDirectoryOperation(destinationDirectoryToCreate);

                operations.Add(createDirectoryOperation);
            }

            foreach (var directoryToDelete in difference.RelativeDirectoryPathsToDelete)
            {
                string destinationDirectoryToDelete = this.StringlyTypedPathOperator.Combine(destination.DirectoryPath, directoryToDelete);

                var deleteDirectoryOperation = new DeleteDirectoryOperation(destinationDirectoryToDelete);

                operations.Add(deleteDirectoryOperation);
            }

            foreach (var fileToCopy in difference.RelativeFilePathsToCopy)
            {
                string sourceFilePath = this.StringlyTypedPathOperator.Combine(source.DirectoryPath, fileToCopy);
                string destinationFilePath = this.StringlyTypedPathOperator.Combine(destination.DirectoryPath, fileToCopy);

                var copyFileOperation = new CopyFileOperation(sourceFilePath, destinationFilePath);

                operations.Add(copyFileOperation);
            }

            foreach (var fileToUpdate in difference.RelativeFilePathsToUpdate)
            {
                string sourceFilePath = this.StringlyTypedPathOperator.Combine(source.DirectoryPath, fileToUpdate);
                string destinationFilePath = this.StringlyTypedPathOperator.Combine(destination.DirectoryPath, fileToUpdate);

                var copyFileOperation = new CopyFileOperation(sourceFilePath, destinationFilePath);

                operations.Add(copyFileOperation);
            }

            foreach (var fileToDelete in difference.RelativeFilePathsToDelete)
            {
                string destinationFilePath = this.StringlyTypedPathOperator.Combine(destination.DirectoryPath, fileToDelete);

                var deleteFileOperation = new DeleteFileOperation(destinationFilePath);

                operations.Add(deleteFileOperation);
            }

            // Write out and allow approval of the list of operations?

            // Execute the list of operations.
            foreach (var operation in operations)
            {
                operation.Execute(source.FileSystemOperator, destination.FileSystemOperator);
            }
        }
    }
}
