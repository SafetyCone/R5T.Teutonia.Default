using System;

using R5T.Gepidia;


namespace R5T.Teutonia
{
    public class CopyFileOperation : IFileSystemCloningOperation
    {
        public string SourceFilePath { get; }
        public string DestinationFilePath { get; }


        public CopyFileOperation(string sourceFilePath, string destinationFilePath)
        {
            this.SourceFilePath = sourceFilePath;
            this.DestinationFilePath = destinationFilePath;
        }

        public void Execute(IFileSystemOperator sourceFileSystemOperator, IFileSystemOperator destinationFileSystemOperator)
        {
            sourceFileSystemOperator.Copy(this.SourceFilePath, destinationFileSystemOperator, this.DestinationFilePath);
        }
    }
}
