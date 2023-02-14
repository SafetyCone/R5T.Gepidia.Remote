using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Burgundy.Extensions;


namespace R5T.Gepidia.Remote.Construction
{
    class Program
    {
        static void Main(string[] args)
        {
            Construction.SubMain();

            //Program.SubMain();
        }

        private static void SubMain()
        {
            Console.WriteLine("Hello World!");
        }

        public static IServiceProvider GetServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                .UseSftpClientWrapper_Old()
                .AddSingleton<RemoteFileSystemOperator>()

                .BuildServiceProvider()
                ;

            return serviceProvider;
        }
    }
}
