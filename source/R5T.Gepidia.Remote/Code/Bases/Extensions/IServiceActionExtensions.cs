using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Lombardy;
using R5T.Pictia;

using R5T.T0062;
using R5T.T0063;


namespace R5T.Gepidia.Remote
{
    public static class IServiceActionExtensions
    {
        /// <summary>
        /// Adds the <see cref="RemoteFileSystemOperator"/> implementation of <see cref="IRemoteFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IRemoteFileSystemOperator> AddRemoteFileSystemOperatorAction(this IServiceAction _,
            IServiceAction<ISftpClientWrapperProvider> sftpClientWrapperProviderAction,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var serviceAction = _.New<IRemoteFileSystemOperator>(services => services.AddRemoteFileSystemOperator(
                sftpClientWrapperProviderAction,
                stringlyTypedPathOperatorAction));

            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="FileSystemOperator"/> implementation of <see cref="IRemoteFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IFileSystemOperator> AddRemoteBasedFileSystemOperatorAction(this IServiceAction _,
            IServiceAction<IRemoteFileSystemOperator> addRemoteFileSystemOperator)
        {
            var serviceAction = _.New<IFileSystemOperator>(services => services.AddRemoteBasedFileSystemOperator(
                addRemoteFileSystemOperator));

            return serviceAction;
        }
    }
}
