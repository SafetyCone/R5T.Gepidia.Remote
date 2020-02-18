using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Lombardy;
using R5T.Pictia;


namespace R5T.Gepidia.Remote
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="RemoteFileSystemOperator"/> implementation of <see cref="IRemoteFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddRemoteFileSystemOperator(this IServiceCollection services,
            ServiceAction<SftpClientWrapper> addSftpClientWrapper,
            ServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            services
                .AddSingleton<IRemoteFileSystemOperator, RemoteFileSystemOperator>()
                .RunServiceAction(addSftpClientWrapper)
                .RunServiceAction(addStringlyTypedPathOperator)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="RemoteFileSystemOperator"/> implementation of <see cref="IRemoteFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static ServiceAction<IRemoteFileSystemOperator> AddRemoteFileSystemOperatorAction(this IServiceCollection services,
            ServiceAction<SftpClientWrapper> addSftpClientWrapper,
            ServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            var serviceAction = new ServiceAction<IRemoteFileSystemOperator>(() => services.AddRemoteFileSystemOperator(
                addSftpClientWrapper,
                addStringlyTypedPathOperator));
            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="FileSystemOperator"/> implementation of <see cref="IRemoteFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddRemoteBasedFileSystemOperator(this IServiceCollection services,
            ServiceAction<IRemoteFileSystemOperator> addRemoteFileSystemOperator)
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
        public static ServiceAction<IFileSystemOperator> AddRemoteBasedFileSystemOperatorAction(this IServiceCollection services,
            ServiceAction<IRemoteFileSystemOperator> addRemoteFileSystemOperator)
        {
            var serviceAction = new ServiceAction<IFileSystemOperator>(() => services.AddRemoteBasedFileSystemOperator(
                addRemoteFileSystemOperator));
            return serviceAction;
        }
    }
}
