namespace JLServer
{
    public static class JLConfigurationManager
    {
        public static IConfiguration GetConfiguration(string environment)
        {
            if (string.IsNullOrEmpty(environment))
                throw new ArgumentNullException(nameof(environment));

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory);

            switch (environment)
            {
                case "Development":
                    configurationBuilder.AddJsonFile("appsettings-Development.json", true, true);
                    break;
                case "Production":
                    configurationBuilder.AddJsonFile("appsettings-Production.json", true, true);
                    break;
            }

            return configurationBuilder.Build();
        }
    }
}
