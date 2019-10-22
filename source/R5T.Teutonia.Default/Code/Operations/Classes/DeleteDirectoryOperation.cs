using System;

using R5T.Gepidia;


namespace R5T.Teutonia
{
    public class DeleteDirectoryOperation : IFileSystemCloningOperation
    {
        public string DirectoryPath { get; }


        public DeleteDirectoryOperation(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }

        public void Execute(IFileSystemOperator sourceFileSystemOperator, IFileSystemOperator destinationFileSystemOperator)
        {
            destinationFileSystemOperator.DeleteDirectoryOnlyIfExists(this.DirectoryPath);
        }
    }
}
