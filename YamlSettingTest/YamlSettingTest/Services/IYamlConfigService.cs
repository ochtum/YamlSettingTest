namespace YamlSettingTest.Services
{
    public interface IYamlConfigService
    {
        Dictionary<string, object> GetConfiguration();
    }
}
