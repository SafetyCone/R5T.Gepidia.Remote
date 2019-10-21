using System;

using R5T.Lombardy;
using R5T.Pictia;


namespace R5T.Gepidia.Remote
{
    public static class SftpClientWrapperExtensions
    {
        /// <summary>
        /// Creates a directory.
        /// Idempotent, can be called multiple times (does not throw an exception if the directory already exists).
        /// Creates all required intermediate directories for a nested directory path.
        /// </summary>
        public static void CreateDirectory(this SftpClientWrapper clientWrapper, string directoryPath, IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            clientWrapper.SftpClient.CreateDirectoryOkIfIntermediatesAndExists(directoryPath, stringlyTypedPathOperator);
        }

        /// <summary>
        /// Creates a directory.
        /// Not idempotent, cannot be called multiple times (throws an exception if the directory already exists).
        /// </summary>
        public static void CreateDirectoryThrowIfExists(this SftpClientWrapper clientWrapper, string directoryPath)
        {
            clientWrapper.SftpClient.CreateDirectory(directoryPath);
        }

        /// <summary>
        /// Deletes a directory.
        /// Idempotent, can be called multiple times (does not throw an exception if the directory does not exist).
        /// Note, this is different than the <see cref="System.IO.Directory.Delete(string)"/> behavior.
        /// Deletes a directory even if the the directory has contents.
        /// </summary>
        public static void DeleteDirectory(this SftpClientWrapper clientWrapper, string directoryPath)
        {
            clientWrapper.SftpClient.DeleteDirectoryRobust(directoryPath);
        }

        /// <summary>
        /// Deletes a directory.
        /// Not idempotent, cannot be called multiple times. If an attempt is made to delete a non-existent directory, "Renci.SshNet.Common.SftpPathNotFoundException: 'No such file'" is thrown.
        /// Note, this is consistent with the <see cref="System.IO.Directory.Delete(string)"/> behavior.
        /// The directory must be empty.
        /// </summary>
        public static void DeleteDirectoryThrowIfNotExistsOrHasContents(this SftpClientWrapper clientWrapper, string directoryPath)
        {
            clientWrapper.SftpClient.DeleteDirectory(directoryPath);
        }
    }
}
