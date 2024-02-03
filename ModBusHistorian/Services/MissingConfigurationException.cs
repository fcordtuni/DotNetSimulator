namespace ModBusHistorian.Services;

public class MissingConfigurationException : Exception
{
    private MissingConfigurationException()
    {
    }

    private MissingConfigurationException(string message) : base(message)
    {
    }

    private MissingConfigurationException(string message, Exception inner) : base(message, inner)
    {
    }

    public static Exception Create(string influxdbToken)
    {
        return new MissingConfigurationException($"Missing configuration for {influxdbToken}");
    }

    public static Exception Create(IConfigurationSection section, String key)
    {
        return new MissingConfigurationException($"Missing app-configuration {section.Path}.{key}");
    }
}