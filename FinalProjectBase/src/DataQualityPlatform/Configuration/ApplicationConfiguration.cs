namespace DataQualityPlatform.Configuration;

public sealed class ApplicationConfiguration
{
    private static readonly Lazy<ApplicationConfiguration> LazyInstance =
        new(() => new ApplicationConfiguration());

    private ApplicationConfiguration()
    {
    }

    public static ApplicationConfiguration Instance => LazyInstance.Value;

    public string InputDirectory { get; set; } = Path.Combine("Data", "Input");

    public string OutputDirectory { get; set; } = "Output";

    public char CsvSeparator { get; set; } = ',';
}
