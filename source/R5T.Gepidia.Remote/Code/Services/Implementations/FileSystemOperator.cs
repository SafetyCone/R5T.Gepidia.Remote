using System;
using System.Collections.Generic;
using System.IO;

using R5T.T0064;


namespace R5T.Gepidia.Remote
{
    [ServiceImplementationMarker]
    public class FileSystemOperator : IFileSystemOperator, IServiceImplementation
    {
        private IRemoteFileSystemOperator RemoteFileSystemOperator { get; }


        public FileSystemOperator(IRemoteFileSystemOperator remoteFileSystemOperator)
        {
            this.RemoteFileSystemOperator = remoteFileSystemOperator;
        }

        public bool ExistsFile(string filePath)
        {
            var output = this.RemoteFileSystemOperator.ExistsFile(filePath);
            return output;
        }

        public bool ExistsDirectory(string directoryPath)
        {
            var output = this.RemoteFileSystemOperator.ExistsDirectory(directoryPath);
            return output;
        }

        public FileSystemEntryType GetFileSystemEntryType(string path)
        {
            var output = this.RemoteFileSystemOperator.GetFileSystemEntryType(path);
            return output;
        }

        public void DeleteFile(string filePath)
        {
            this.RemoteFileSystemOperator.DeleteFile(filePath);
        }

        public void DeleteDirectory(string directoryPath, bool recursive = true)
        {
            this.RemoteFileSystemOperator.DeleteDirectory(directoryPath, recursive);
        }

        public Stream CreateFile(string filePath, bool overwrite = true)
        {
            var output = this.RemoteFileSystemOperator.CreateFile(filePath, overwrite);
            return output;
        }

        public Stream OpenFile(string filePath)
        {
            var output = this.RemoteFileSystemOperator.OpenFile(filePath);
            return output;
        }

        public Stream ReadFile(string filePath)
        {
            var output = this.RemoteFileSystemOperator.ReadFile(filePath);
            return output;
        }

        public void CreateDirectory(string directoryPath)
        {
            this.RemoteFileSystemOperator.CreateDirectory(directoryPath);
        }

        public IEnumerable<string> EnumerateFileSystemEntryPaths(string directoryPath, bool recursive = false)
        {
            var output = this.RemoteFileSystemOperator.EnumerateFileSystemEntryPaths(directoryPath, recursive);
            return output;
        }

        public IEnumerable<string> EnumerateDirectories(string directoryPath)
        {
            var output = this.RemoteFileSystemOperator.EnumerateDirectories(directoryPath);
            return output;
        }

        public IEnumerable<string> EnumerateFiles(string directoryPath)
        {
            var output = this.RemoteFileSystemOperator.EnumerateFiles(directoryPath);
            return output;
        }

        public IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(string directoryPath, bool recursive = false)
        {
            var output = this.RemoteFileSystemOperator.EnumerateFileSystemEntries(directoryPath, recursive);
            return output;
        }

        public DateTime GetDirectoryLastModifiedTimeUTC(string directoryPath)
        {
            var output = this.RemoteFileSystemOperator.GetDirectoryLastModifiedTimeUTC(directoryPath);
            return output;
        }

        public DateTime GetFileLastModifiedTimeUTC(string filePath)
        {
            var output = this.RemoteFileSystemOperator.GetFileLastModifiedTimeUTC(filePath);
            return output;
        }

        public void ChangePermissions(string path, short mode)
        {
            this.RemoteFileSystemOperator.ChangePermissions(path, mode);
        }

        public void CopyFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            this.RemoteFileSystemOperator.CopyFile(sourceFilePath, destinationFilePath, overwrite);
        }

        public void CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            this.RemoteFileSystemOperator.CopyDirectory(sourceDirectoryPath, destinationDirectoryPath);
        }

        public void Copy(Stream source, string destinationFilePath, bool overwrite = true)
        {
            this.RemoteFileSystemOperator.Copy(source, destinationFilePath, overwrite);
        }

        public void Copy(string sourceFilePath, Stream destination)
        {
            this.RemoteFileSystemOperator.Copy(sourceFilePath, destination);
        }

        public void MoveFile(string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            this.RemoteFileSystemOperator.MoveFile(sourceFilePath, destinationFilePath, overwrite);
        }

        public void MoveDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            this.RemoteFileSystemOperator.MoveDirectory(sourceDirectoryPath, destinationDirectoryPath);
        }
    }
}
