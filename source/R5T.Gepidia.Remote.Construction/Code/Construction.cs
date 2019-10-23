using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;


namespace R5T.Gepidia.Remote.Construction
{
    public static class Construction
    {
        public static void SubMain()
        {
            Construction.TestEnumerationOfFileSystemEntryPaths();
        }

        private static void TestEnumerationOfFileSystemEntryPaths()
        {
            var serviceProvider = Program.GetServiceProvider();

            var remoteFileSystemOperator = serviceProvider.GetRequiredService<RemoteFileSystemOperator>();

            var directoryPath = @"/home/ec2-user/";

            var remotePaths = remoteFileSystemOperator.EnumerateFileSystemEntryPaths(directoryPath, true).ToList();
            foreach (var remotePath in remotePaths)
            {
                Console.WriteLine(remotePath);
            }
        }
    }
}
