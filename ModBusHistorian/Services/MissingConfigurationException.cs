namespace ModBusHistorian.Services;

public class MissingConfigurationException : Exception
{
    private MissingConfigurationException(string message) : base(message)
    {
    }

    public static Exception Create(string influxdbToken)
    {
        return new MissingConfigurationException($"Missing configuration for {influxdbToken}");
    }

    public static Exception Create(IConfigurationSection section, string key)
    {
        return new MissingConfigurationException($"Missing app-configuration {section.Path}.{key}");
    }
}