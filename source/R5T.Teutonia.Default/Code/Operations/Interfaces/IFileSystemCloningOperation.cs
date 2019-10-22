using System;

using R5T.Gepidia;


namespace R5T.Teutonia
{
    public interface IFileSystemCloningOperation
    {
        void Execute(IFileSystemOperator sourceFileSystemOperator, IFileSystemOperator destinationFileSystemOperator);
    }
}
