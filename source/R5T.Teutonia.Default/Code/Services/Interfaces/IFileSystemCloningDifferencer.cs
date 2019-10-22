using System;
using System.Collections.Generic;

using R5T.Gepidia;


namespace R5T.Teutonia
{
    public interface IFileSystemCloningDifferencer
    {
        FileSystemCloningDifference PerformDifference(
            IEnumerable<FileSystemEntry> sourceFileSystemEntries,
            IEnumerable<FileSystemEntry> destinationFileSystemEntries,
            FileSystemCloningOptions options);
    }
}
