using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Lombardy;
using R5T.Pictia;


namespace R5T.Gepidia.Remote
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="RemoteFileSystemOperator"/> implementation of <see cref="IRemoteFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddRemoteFileSystemOperator_Old(this IServiceCollection services,
            IServiceAction<ISftpClientWrapperProvider> addSftpClientWrapperProvider,
            IServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            services
                .AddSingleton<IRemoteFileSystemOperator, RemoteFileSystemOperator>()
                .RunServiceAction(addSftpClientWrapperProvider)
                .RunServiceAction(addStringlyTypedPathOperator)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="RemoteFileSystemOperator"/> implementation of <see cref="IRemoteFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IRemoteFileSystemOperator> AddRemoteFileSystemOperatorAction_Old(this IServiceCollection services,
            IServiceAction<ISftpClientWrapperProvider> addSftpClientWrapperProvider,
            IServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            var serviceAction = new ServiceAction<IRemoteFileSystemOperator>(() => services.AddRemoteFileSystemOperator_Old(
                addSftpClientWrapperProvider,
                addStringlyTypedPathOperator));
            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="FileSystemOperator"/> implementation of <see cref="IRemoteFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddRemoteBasedFileSystemOperator_Old(this IServiceCollection services,
            IServiceAction<IRemoteFileSystemOperator> addRemoteFileSystemOperator)
        {
            services
                .AddSingleton<IFileSystemOperator, FileSystemOperator>()
                .RunServiceAction(addRemoteFileSystemOperator)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="FileSystemOperator"/> implementation of <see cref="IRemoteFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IFileSystemOperator> AddRemoteBasedFileSystemOperatorAction_Old(this IServiceCollection services,
            IServiceAction<IRemoteFileSystemOperator> addRemoteFileSystemOperator)
        {
            var serviceAction = new ServiceAction<IFileSystemOperator>(() => services.AddRemoteBasedFileSystemOperator_Old(
                addRemoteFileSystemOperator));
            return serviceAction;
        }
    }
}
