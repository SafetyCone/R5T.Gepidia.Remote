using System;
using System.Collections.Generic;
using System.IO;

using R5T.Lombardy;
using R5T.Pictia;


namespace R5T.Gepidia.Remote
{
    public class RemoteFileSystemOperator : IFileSystemOperator
    {
        private SftpClientWrapper SftpClientWrapper { get; }
        public IStringlyTypedPathOperator StringlyTypedPathOperator { get; }


        public RemoteFileSystemOperator(SftpClientWrapper sftpClientWrapper, IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            this.SftpClientWrapper = sftpClientWrapper;
            this.StringlyTypedPathOperator = stringlyTypedPathOperator;
        }

        public void ChangePermissions(string path, short mode)
        {
            RemoteFileSystem.ChangePermissions(this.SftpClientWrapper, path, mode);
        }

        public void Copy(Stream source, string destinationFilePath, bool overwrite = true)
        {
            RemoteFileSystem.Copy(this.SftpClientWrapper, source, destinationFilePath, overwrite);
        }

        public void Copy(string sourceFilePath, Stream destination)
        {
            RemoteFileSystem.Copy(this.SftpClientWrapper, sourceFilePath, destination);
        }

        public void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            RemoteFileSystem.CopyDirectory(this.SftpClientWrapper, sourceDirectoryPath, destinationDirectoryPath);
        }

        public void CopyFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            RemoteFileSystem.CopyFile(this.SftpClientWrapper, sourceFilePath, destinationFilePath, overwrite);
        }

        public void CreateDirectory(string directoryPath)
        {
            RemoteFileSystem.CreateDirectory(this.SftpClientWrapper, directoryPath, this.StringlyTypedPathOperator);
        }

        public Stream CreateFile(string filePath, bool overwrite = true)
        {
            var output = RemoteFileSystem.CreateFile(this.SftpClientWrapper, filePath, overwrite);
            return output;
        }

        public void DeleteDirectory(string directoryPath, bool recursive = true)
        {
            RemoteFileSystem.DeleteDirectory(this.SftpClientWrapper, directoryPath, recursive);
        }

        public void DeleteFile(string filePath)
        {
            RemoteFileSystem.DeleteFile(this.SftpClientWrapper, filePath);
        }

        public IEnumerable<string> EnumerateDirectories(string directoryPath)
        {
            var output = RemoteFileSystem.EnumerateDirectories(this.SftpClientWrapper, directoryPath);
            return output;
        }

        public IEnumerable<string> EnumerateFiles(string directoryPath)
        {
            var output = RemoteFileSystem.EnumerateFiles(this.SftpClientWrapper, directoryPath);
            return output;
        }

        public IEnumerable<string> EnumerateFileSystemEntryPaths(string directoryPath, bool recursive = false)
        {
            var output = RemoteFileSystem.EnumerateFileSystemEntryPaths(this.SftpClientWrapper, directoryPath, recursive);
            return output;
        }

        public IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(string directoryPath, bool recursive = false)
        {
            var output = RemoteFileSystem.EnumerateFileSystemEntriesFast(this.SftpClientWrapper, directoryPath, recursive);
            return output;
        }

        public bool ExistsDirectory(string directoryPath)
        {
            var output = RemoteFileSystem.ExistsDirectory(this.SftpClientWrapper, directoryPath);
            return output;
        }

        public bool ExistsFile(string filePath)
        {
            var output = RemoteFileSystem.ExistsFile(this.SftpClientWrapper, filePath);
            return output;
        }

        public FileSystemEntryType GetFileSystemEntryType(string path)
        {
            var output = RemoteFileSystem.GetFileSystemEntryType(this.SftpClientWrapper, path);
            return output;
        }

        public string GetCannotOverwriteFileExceptionMessage(string filePath)
        {
            var output = CommonFileSystem.GetCannotOverwriteFileExceptionMessage(filePath);
            return output;
        }

        public IOException GetCannotOverwriteFileIOException(string filePath)
        {
            var output = CommonFileSystem.GetCannotOverwriteFileIOException(filePath);
            return output;
        }

        public DateTime GetDirectoryLastModifiedTimeUTC(string directoryPath)
        {
            var output = RemoteFileSystem.GetDirectoryLastModifiedTimeUTC(this.SftpClientWrapper, directoryPath);
            return output;
        }

        public DateTime GetFileLastModifiedTimeUTC(string filePath)
        {
            var output = RemoteFileSystem.GetFileLastModifiedTimeUTC(this.SftpClientWrapper, filePath);
            return output;
        }

        public void MoveDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            RemoteFileSystem.MoveDirectory(this.SftpClientWrapper, sourceDirectoryPath, destinationDirectoryPath);   
        }

        public void MoveFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            RemoteFileSystem.MoveFile(this.SftpClientWrapper, sourceFilePath, destinationFilePath, overwrite);
        }

        public Stream OpenFile(string filePath)
        {
            var output = RemoteFileSystem.OpenFile(this.SftpClientWrapper, filePath);
            return output;
        }

        public Stream ReadFile(string filePath)
        {
            var output = RemoteFileSystem.ReadFile(this.SftpClientWrapper, filePath);
            return output;
        }
    }
}
