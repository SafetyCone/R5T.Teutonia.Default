using System;

using R5T.Gepidia;


namespace R5T.Teutonia
{
    public class DeleteFileOperation : IFileSystemCloningOperation
    {
        public string FilePath { get; }


        public DeleteFileOperation(string filePath)
        {
            this.FilePath = filePath;
        }

        public void Execute(IFileSystemOperator sourceFileSystemOperator, IFileSystemOperator destinationFileSystemOperator)
        {
            destinationFileSystemOperator.DeleteFileOnlyIfExists(this.FilePath);
        }
    }
}
