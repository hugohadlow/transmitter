using Microsoft.Extensions.Configuration;

namespace TransmitterTest
{
    public class TestUtils
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                //.AddEnvironmentVariables()
                .Build();
            return config;
        }
    }
}