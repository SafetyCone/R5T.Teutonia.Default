using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using R5T.Gepidia.Local;
using R5T.Lombardy;
using R5T.Teutonia.Test;


namespace R5T.Teutonia.Default.Testing
{
    [TestClass]
    public class FileSystemCloningOperatorLocalToRemoteTests : FileSystemCloningOperatorTestFixture
    {
        public FileSystemCloningOperatorLocalToRemoteTests()
            : base(Utilities.GetLocalSourceSite(Utilities.ServiceProvider), Utilities.GetRemoteDestinationSite(Utilities.ServiceProvider),
                  Utilities.ServiceProvider.GetRequiredService<IFileSystemCloningOperator>(), Utilities.ServiceProvider.GetRequiredService<IStringlyTypedPathOperator>())
        {
        }
    }
}
