using System;

using Renci.SshNet.Sftp;

using R5T.Lombardy;


namespace R5T.Gepidia.Remote.Extensions
{
    public static class SftpFileExtensions
    {
        public static FileSystemEntryType GetFileSystemEntryType(this SftpFile sftpFile)
        {
            if(sftpFile.IsDirectory)
            {
                return FileSystemEntryType.Directory;
            }
            else
            {
                return FileSystemEntryType.File;
            }
        }

        /// <summary>
        /// Gets a directory-indicated path if the <see cref="SftpFile"/> is a directory.
        /// Otherwise, just return the <see cref="SftpFile.FullName"/>.
        /// The <see cref="SftpFile.FullName"/> does NOT return directory-indicated directory paths, but file paths are file-indicated.
        /// </summary>
        public static string GetPathDirectoryIndicatedIfDirectory(this SftpFile sftpFile, IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            var output = sftpFile.IsDirectory ? stringlyTypedPathOperator.EnsureDirectoryPathIsDirectoryIndicated(sftpFile.FullName) : sftpFile.FullName;
            return output;
        }

        /// <summary>
        /// Gets a directory-indicated path if the <see cref="SftpFile"/> is a directory.
        /// Uses <see cref="SftpFileExtensions.GetPathDirectoryIndicatedIfDirectory(SftpFile, IStringlyTypedPathOperator)"/>.
        /// </summary>
        public static string GetPath(this SftpFile sftpFile, IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            var output = sftpFile.GetPathDirectoryIndicatedIfDirectory(stringlyTypedPathOperator);
            return output;
        }
    }
}
