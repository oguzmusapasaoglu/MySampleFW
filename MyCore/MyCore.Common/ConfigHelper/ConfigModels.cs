namespace MyCore.Common.ConfigHelper;

public class MainSettingsConfigModel
{
    public const string SectionName = "MainSettings";
    public string ConnectionString { get; set; }
    public string LogDbConnectionString { get; set; }
    public string ServicesURL { get; set; }
}
public class MainSettingsConfigModelHelper
{
    public static MainSettingsConfigModel GetSettings()
    {
        var settings = ConfigurationHelper.GetConfig<MainSettingsConfigModel>(MainSettingsConfigModel.SectionName);
        return settings;
    }
    public static string GetConnection() => GetSettings().ConnectionString;
    public static string GetLogDbConnection() => GetSettings().LogDbConnectionString;
    public static string GetServicesURL() => GetSettings().ServicesURL;
}
