using System;

using Renci.SshNet.Sftp;


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
    }
}
