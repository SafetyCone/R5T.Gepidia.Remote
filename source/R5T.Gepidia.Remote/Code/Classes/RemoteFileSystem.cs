using System;
using System.Collections.Generic;
using System.IO;

using R5T.Lombardy;
using R5T.Magyar.Extensions;
using R5T.Magyar.IO;
using R5T.Pictia;
using R5T.Pictia.Extensions;

using R5T.Gepidia.Remote.Extensions;


namespace R5T.Gepidia.Remote
{
    public static class RemoteFileSystem
    {
        public static bool Exists(SftpClientWrapper sftpClientWrapper, string path)
        {
            var output = sftpClientWrapper.SftpClient.Exists(path);
            return output;
        }

        /// <summary>
        /// Checks that the file path exists, and that it is a file.
        /// Note: requires two remote calls.
        /// </summary>
        public static bool ExistsFile(SftpClientWrapper sftpClientWrapper, string filePath)
        {
            // Does the path exist?
            var exists = RemoteFileSystem.Exists(sftpClientWrapper, filePath);
            if(!exists)
            {
                return false;
            }

            // The path exists, but is it a file?
            var isFile = RemoteFileSystem.IsFile(sftpClientWrapper, filePath);
            return isFile;
        }

        public static bool ExistsDirectory(SftpClientWrapper sftpClientWrapper, string directoryPath)
        {
            // Does the path exist?
            var exists = RemoteFileSystem.Exists(sftpClientWrapper, directoryPath);
            if (!exists)
            {
                return false;
            }

            // The path exists, but is it a file?
            var isFile = RemoteFileSystem.IsDirectory(sftpClientWrapper, directoryPath);
            return isFile;
        }

        public static bool IsFile(SftpClientWrapper sftpClientWrapper, string path)
        {
            var attributes = sftpClientWrapper.SftpClient.GetAttributes(path);

            var output = !attributes.IsDirectory;
            return output;
        }

        public static bool IsDirectory(SftpClientWrapper sftpClientWrapper, string path)
        {
            var attributes = sftpClientWrapper.SftpClient.GetAttributes(path);

            var output = attributes.IsDirectory;
            return output;
        }

        public static FileSystemEntryType GetFileSystemEntryType(SftpClientWrapper sftpClientWrapper, string path)
        {
            var isDirectory = RemoteFileSystem.IsDirectory(sftpClientWrapper, path);
            if(isDirectory)
            {
                return FileSystemEntryType.Directory;
            }
            else
            {
                return FileSystemEntryType.File;
            }
        }

        public static void CreateDirectory(SftpClientWrapper sftpClientWrapper, string directoryPath, IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            sftpClientWrapper.CreateDirectory(directoryPath, stringlyTypedPathOperator); // Idempotent. No exception thrown.
        }

        public static void CreateDirectoryOnlyIfNotExists(SftpClientWrapper sftpClientWrapper, string directoryPath, IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            RemoteFileSystem.CreateDirectory(sftpClientWrapper, directoryPath, stringlyTypedPathOperator);
        }

        public static void DeleteDirectory(SftpClientWrapper sftpClientWrapper, string directoryPath, bool recursive = true)
        {
            var exists = RemoteFileSystem.Exists(sftpClientWrapper, directoryPath);
            if (!exists)
            {
                return;
            }

            var isDirectory = RemoteFileSystem.IsDirectory(sftpClientWrapper, directoryPath);
            if(!isDirectory)
            {
                throw new Exception($"Unable to delete directory. Path was not a directory:\n{directoryPath}");
            }

            if(recursive)
            {
                sftpClientWrapper.DeleteDirectory(directoryPath);
            }
            else
            {
                sftpClientWrapper.SftpClient.DeleteDirectory(directoryPath);
            }
        }

        public static void DeleteDirectoryOnlyIfExists(SftpClientWrapper sftpClientWrapper, string directoryPath, bool recursive = true)
        {
            RemoteFileSystem.DeleteDirectory(sftpClientWrapper, directoryPath, recursive); // Idempotent, ok.
        }

        public static Stream CreateFile(SftpClientWrapper sftpClientWrapper, string filePath, bool overwrite = true)
        {
            RemoteFileSystem.CheckOverwrite(sftpClientWrapper, filePath, overwrite);

            var output = sftpClientWrapper.SftpClient.Create(filePath);
            return output;
        }

        public static Stream OpenFile(SftpClientWrapper sftpClientWrapper, string filePath)
        {
            var output = sftpClientWrapper.SftpClient.OpenWrite(filePath);
            return output;
        }

        public static Stream ReadFile(SftpClientWrapper sftpClientWrapper, string filePath)
        {
            var output = sftpClientWrapper.SftpClient.OpenRead(filePath);
            return output;
        }

        public static void DeleteFile(SftpClientWrapper sftpClientWrapper, string filePath)
        {
            sftpClientWrapper.SftpClient.DeleteFileOkIfNotExists(filePath);
        }

        public static void DeleteFileOnlyIfExists(SftpClientWrapper sftpClientWrapper, string filePath)
        {
            RemoteFileSystem.DeleteFile(sftpClientWrapper, filePath); // Idempotent, ok.
        }

        public static DateTime GetDirectoryLastModifiedTimeUTC(SftpClientWrapper sftpClientWrapper, string directoryPath)
        {
            var output = sftpClientWrapper.SftpClient.GetLastWriteTimeUtc(directoryPath);
            return output;
        }

        public static DateTime GetFileLastModifiedTimeUTC(SftpClientWrapper sftpClientWrapper, string filePath)
        {
            var output = sftpClientWrapper.SftpClient.GetLastWriteTimeUtc(filePath);
            return output;
        }

        public static void ChangePermissions(SftpClientWrapper sftpClientWrapper, string path, short mode)
        {
            sftpClientWrapper.SftpClient.ChangePermissions(path, mode);
        }

        public static void Copy(SftpClientWrapper sftpClientWrapper, Stream source, string destinationFilePath, bool overwrite = true)
        {
            RemoteFileSystem.CheckOverwrite(sftpClientWrapper, destinationFilePath, overwrite);

            using (var destination = sftpClientWrapper.SftpClient.Create(destinationFilePath))
            {
                source.CopyTo(destination);
            }
        }

        public static void Copy(SftpClientWrapper sftpClientWrapper, string sourceFilePath, Stream destination)
        {
            using (var source = sftpClientWrapper.SftpClient.OpenRead(sourceFilePath))
            {
                source.CopyTo(destination);
            }
        }

        public static void CopyFile(SftpClientWrapper sftpClientWrapper, string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            var fileExists = RemoteFileSystem.ExistsFile(sftpClientWrapper, sourceFilePath);
            if(!fileExists)
            {
                throw new Exception($"Unable to copy file. Source file does not exist:\n{sourceFilePath}");
            }

            using (var sshClient = sftpClientWrapper.SftpClient.ConnectionInfo.GetSshClient())
            {
                var commandText = $"cp \"{sourceFilePath}\" \"{destinationFilePath}\"";
                using (var command = sshClient.RunCommand(commandText))
                {
                    if (command.ExitStatus != 0)
                    {
                        throw new Exception($"Command failed. Result:\n{command.Result}");
                    }
                }
            }
        }

        public static void CopyDirectory(SftpClientWrapper sftpClientWrapper, string sourceDirectoryPath, string destinationDirectoryPath)
        {
            var directoryExists = RemoteFileSystem.ExistsDirectory(sftpClientWrapper, sourceDirectoryPath);
            if (!directoryExists)
            {
                throw new Exception($"Unable to copy directory. Source directory does not exist:\n{sourceDirectoryPath}");
            }

            using (var sshClient = sftpClientWrapper.SftpClient.ConnectionInfo.GetSshClient())
            {
                var commandText = $"cp -r \"{sourceDirectoryPath}\" \"{destinationDirectoryPath}\"";
                using (var command = sshClient.RunCommand(commandText))
                {
                    if (command.ExitStatus != 0)
                    {
                        throw new Exception($"Command failed. Result:\n{command.Result}");
                    }
                }
            }
        }

        public static void MoveDirectory(SftpClientWrapper sftpClientWrapper, string sourceDirectoryPath, string destinationDirectoryPath)
        {
            var directoryExists = RemoteFileSystem.ExistsDirectory(sftpClientWrapper, sourceDirectoryPath);
            if (!directoryExists)
            {
                throw new Exception($"Unable to move directory. Source directory does not exist:\n{sourceDirectoryPath}");
            }

            using (var sshClient = sftpClientWrapper.SftpClient.ConnectionInfo.GetSshClient())
            {
                var commandText = $"mv \"{sourceDirectoryPath}\" \"{destinationDirectoryPath}\"";
                using (var command = sshClient.RunCommand(commandText))
                {
                    if (command.ExitStatus != 0)
                    {
                        throw new Exception($"Command failed. Result:\n{command.Result}");
                    }
                }
            }
        }

        public static void MoveFile(SftpClientWrapper sftpClientWrapper, string sourceFilePath, string destinationFilePath, bool overwrite = true)
        {
            var fileExists = RemoteFileSystem.ExistsFile(sftpClientWrapper, sourceFilePath);
            if (!fileExists)
            {
                throw new Exception($"Unable to move file. Source file does not exist:\n{sourceFilePath}");
            }

            RemoteFileSystem.CheckOverwrite(sftpClientWrapper, destinationFilePath, overwrite);

            using (var sshClient = sftpClientWrapper.SftpClient.ConnectionInfo.GetSshClient())
            {
                var commandText = $"mv \"{sourceFilePath}\" \"{destinationFilePath}\"";
                using (var command = sshClient.RunCommand(commandText))
                {
                    if (command.ExitStatus != 0)
                    {
                        throw new Exception($"Command failed. Result:\n{command.Result}");
                    }
                }
            }
        }

        /// <summary>
        /// Returns directory-indicated directory paths.
        /// Non-recursive.
        /// </summary>
        public static IEnumerable<string> EnumerateDirectories(SftpClientWrapper sftpClientWrapper, IStringlyTypedPathOperator stringlyTypedPathOperator, string directoryPath)
        {
            var sftpFiles = sftpClientWrapper.SftpClient.ListDirectory(directoryPath);
            foreach (var sftpFile in sftpFiles)
            {
                if(sftpFile.IsDirectory)
                {
                    var entryDirectoryPath = stringlyTypedPathOperator.EnsureDirectoryPathIsDirectoryIndicated(sftpFile.FullName); // SSH.NET does NOT return directory-indicated directory paths.
                    yield return entryDirectoryPath;
                }
            }
        }

        /// <summary>
        /// Returns file paths.
        /// Non-recursive.
        /// </summary>
        public static IEnumerable<string> EnumerateFiles(SftpClientWrapper sftpClientWrapper, string directoryPath)
        {
            var sftpFiles = sftpClientWrapper.SftpClient.ListDirectory(directoryPath);
            foreach (var sftpFile in sftpFiles)
            {
                if (!sftpFile.IsDirectory)
                {
                    yield return sftpFile.FullName; // SSH.NET DOES return file-indicated file paths.
                }
            }
        }

        /// <summary>
        /// Produces paths where directory paths are directory-indicated, and file paths are file-indicated.
        /// Returns all file-entries in sorted order.
        /// Does not return hidden entries.
        /// </summary>
        /// <remarks>
        /// For a remote directory containing N directories (including the base directory), this method requires N remote calls to list directories.
        /// TODO: rework to use SSH-client command output parsing allowing just one call.
        /// </remarks>
        public static IEnumerable<string> EnumerateFileSystemEntryPaths(SftpClientWrapper sftpClientWrapper, IStringlyTypedPathOperator stringlyTypedPathOperator, string directoryPath, bool recursive = false)
        {
            var sftpFiles = sftpClientWrapper.SftpClient.ListDirectoryEntriesOnly(directoryPath);
            foreach (var sftpFile in sftpFiles)
            {
                var entryPath = sftpFile.GetPath(stringlyTypedPathOperator);
                yield return entryPath;

                if (sftpFile.IsDirectory)
                {
                    if(recursive)
                    {
                        var subDirectoryFileSystemEntries = RemoteFileSystem.EnumerateFileSystemEntryPaths(sftpClientWrapper, stringlyTypedPathOperator, sftpFile.FullName, true); // Use SftpFile.FullName.
                        foreach (var subDirectoryFileSystemEntry in subDirectoryFileSystemEntries)
                        {
                            yield return subDirectoryFileSystemEntry;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Produces paths where directory paths are directory-indicated, and file paths are file-indicated.
        /// Returns all file-entries in sorted order.
        /// </summary>
        /// <remarks>
        /// For a remote directory containing N directories (including the base directory), this method requires N remote calls to list directories.
        /// For a faster implementation, use <see cref="RemoteFileSystem.EnumerateFileSystemEntriesFast(SftpClientWrapper, IStringlyTypedPathOperator, string, bool)"/>.
        /// </remarks>
        public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntriesSimple(SftpClientWrapper sftpClientWrapper, IStringlyTypedPathOperator stringlyTypedPathOperator, string directoryPath, bool recursive = false)
        {
            var sftpFiles = sftpClientWrapper.SftpClient.ListDirectoryEntriesOnly(directoryPath);
            foreach (var sftpFile in sftpFiles)
            {
                var fileSystemEntryType = sftpFile.GetFileSystemEntryType();
                var fileSystemEntryPath = sftpFile.GetPath(stringlyTypedPathOperator);
                var lastModifedUTC = sftpFile.LastWriteTimeUtc;

                var fileSystemEntry = FileSystemEntry.New(fileSystemEntryPath, fileSystemEntryType, lastModifedUTC);
                yield return fileSystemEntry;

                if (recursive && sftpFile.IsDirectory)
                {
                    var subDirectoryFileSystemEntries = RemoteFileSystem.EnumerateFileSystemEntries(sftpClientWrapper, stringlyTypedPathOperator, sftpFile.FullName, true); // Use SftpFile.FullName.
                    foreach (var subDirectoryFileSystemEntry in subDirectoryFileSystemEntries)
                    {
                        yield return subDirectoryFileSystemEntry;
                    }
                }
            }
        }

        /// <summary>
        /// Produces paths where directory paths are directory-indicated, and file paths are file-indicated.
        /// Returns all file-entries in sorted order.
        /// </summary>
        public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntriesFast(SftpClientWrapper sftpClientWrapper, IStringlyTypedPathOperator stringlyTypedPathOperator, string directoryPath, bool recursive = false)
        {
            if(!recursive)
            {
                // If not recusive, use the simple implementation that 
                var output = RemoteFileSystem.EnumerateFileSystemEntries(sftpClientWrapper, stringlyTypedPathOperator, directoryPath, recursive); // If only this directory, then only do this directory.
                return output;
            }

            using (var sshClientWrapper = sftpClientWrapper.GetSshClientWrapper())
            {
                var commandText = $"find \"{directoryPath}\" -print -ls"; // Produces two lines for each file-system entry, the path of the entry, and information about the entry.
                using (var command = sshClientWrapper.SshClient.CreateCommand(commandText))
                {
                    var commandOutput = command.Execute();

                    var output = RemoteFileSystem.ParseListDirectoryContentsRecursive(stringlyTypedPathOperator, commandOutput);
                    return output;
                }
            }
        }

        /// <summary>
        /// Produces paths where directory paths are directory-indicated, and file paths are file-indicated.
        /// Returns all file-entries in sorted order.
        /// </summary>
        private static IEnumerable<FileSystemEntry> ParseListDirectoryContentsRecursive(IStringlyTypedPathOperator stringlyTypedPathOperator, string commandOutput)
        {
            // Example command output:
            // /home/user/Directory
            // 266158    0 drwxrwxr-x   2 user user      224 Oct 14 23:18 /home/user/Directory
            // /home/user/Directory/File.txt
            // 266158    4 -rw-rw-r--   1 user user      752 Oct 14 23:18 /home/user/Directory/File.txt

            var isFirstLine = true; // Used to ignore the first line.

            var separators = new char[] { ' ' };
            using (var stringReader = new StringReader(commandOutput))
            {
                while (stringReader.ReadLine(out var pathLine))
                {
                    var infoLine = stringReader.ReadLine();

                    if(isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var infoTokens = infoLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    var permissionsToken = infoTokens[2];

                    var isDirectory = permissionsToken[0] == 'd';
                    var fileSystemEntryType = isDirectory ? FileSystemEntryType.Directory : FileSystemEntryType.File;

                    // Get the last modified date.
                    var infoLineWithoutPath = infoLine.ExceptLast(pathLine.Length + 1); // Remove the trailing space.

                    var infoLineWithoutPathTokens = infoLineWithoutPath.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    var monthToken = infoLineWithoutPathTokens.NthToLast(3);
                    var dayToken = infoLineWithoutPathTokens.NthToLast(2);
                    var timeToken = infoLineWithoutPathTokens.NthToLast(1);

                    var lastModifiedDateUTC = DateTime.Parse($"{timeToken} {monthToken}/{dayToken}/{DateTime.UtcNow.Year}");

                    var entryPath = isDirectory ? stringlyTypedPathOperator.EnsureDirectoryPathIsDirectoryIndicated(pathLine) : pathLine; // The 'find' command does NOT produce directory-indicated directory paths. But file paths are file-indicated.

                    var fileSystemEntry = FileSystemEntry.New(entryPath, fileSystemEntryType, lastModifiedDateUTC);
                    yield return fileSystemEntry;
                }
            }
        }

        /// <summary>
        /// Produces paths where directory paths are directory-indicated, and file paths are file-indicated.
        /// Returns all file-entries in sorted order.
        /// Uses the fast implementation <see cref="RemoteFileSystem.EnumerateFileSystemEntriesFast(SftpClientWrapper, IStringlyTypedPathOperator, string, bool)"/>.
        /// </summary>
        public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(SftpClientWrapper sftpClientWrapper, IStringlyTypedPathOperator stringlyTypedPathOperator, string directoryPath, bool recursive = false)
        {
            var output = RemoteFileSystem.EnumerateFileSystemEntriesFast(sftpClientWrapper, stringlyTypedPathOperator, directoryPath, recursive);
            return output;
        }


        #region Miscellaneous

        public static void CheckOverwrite(SftpClientWrapper sftpClientWrapper, string filePath, bool overwrite)
        {
            if (!overwrite && RemoteFileSystem.ExistsFile(sftpClientWrapper, filePath))
            {
                var exception = CommonFileSystem.GetCannotOverwriteFileIOException(filePath);
                throw exception;
            }
        }

        #endregion
    }
}
