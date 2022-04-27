using System;

using R5T.T0064;


namespace R5T.Gepidia.Remote
{
    [ServiceDefinitionMarker]
    public interface IRemoteFileSystemOperator : IFileSystemOperator, IServiceDefinition
    {
    }
}
