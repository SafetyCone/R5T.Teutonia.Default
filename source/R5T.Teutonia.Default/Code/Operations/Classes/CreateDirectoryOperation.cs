using System;

using R5T.Gepidia;


namespace R5T.Teutonia
{
    public class CreateDirectoryOperation : IFileSystemCloningOperation
    {
        public string DirectoryPath { get; }


        public CreateDirectoryOperation(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }

        public void Execute(IFileSystemOperator sourceFileSystemOperator, IFileSystemOperator destinationFileSystemOperator)
        {
            destinationFileSystemOperator.CreateDirectoryOnlyIfNotExists(this.DirectoryPath);
        }
    }
}
