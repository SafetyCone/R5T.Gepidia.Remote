using System;

using R5T.Lombardy;
using R5T.Magyar;


namespace R5T.Gepidia.Remote.Testing
{
    public static class Utilities
    {
        public static string GetRemoteTestingRootDirectoryPath(IStringlyTypedPathOperator stringlyTypedPathOperator)
        {
            // Get user from secrets directory.

            var userDirectoryPath = PathHelper.UserProfileDirectoryPathValue;

            var randomDirectoryName = PathHelper.GetRandomDirectoryName();

            var rootDirectoryPath = stringlyTypedPathOperator.GetDirectoryPath(userDirectoryPath, randomDirectoryName);
            return rootDirectoryPath;
        }
    }
}
