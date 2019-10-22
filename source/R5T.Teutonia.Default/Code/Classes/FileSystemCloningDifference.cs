using System;
using System.Collections.Generic;

using R5T.Gepidia;


namespace R5T.Teutonia
{
    public class FileSystemCloningDifference
    {
        public List<string> RelativeDirectoryPathsToCreate { get; set; } = new List<string>();
        public List<string> RelativeDirectoryPathsToDelete { get; set; } = new List<string>();
        public List<string> RelativeFilePathsToCopy { get; set; } = new List<string>();
        public List<string> RelativeFilePathsToUpdate { get; set; } = new List<string>();
        public List<string> RelativeFilePathsToDelete { get; set; } = new List<string>();
    }
}
