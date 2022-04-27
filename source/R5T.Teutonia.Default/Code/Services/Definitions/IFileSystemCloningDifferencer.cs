using System;
using System.Collections.Generic;

using R5T.Gepidia;

using R5T.T0064;


namespace R5T.Teutonia
{
    [ServiceDefinitionMarker]
    public interface IFileSystemCloningDifferencer : IServiceDefinition
    {
        FileSystemCloningDifference PerformDifference(
            IEnumerable<FileSystemEntry> sourceFileSystemEntries,
            IEnumerable<FileSystemEntry> destinationFileSystemEntries,
            FileSystemCloningOptions options);
    }
}
