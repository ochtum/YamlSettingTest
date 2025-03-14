using YamlSettingTest.Modules;

namespace YamlSettingTest.Services
{
    public class YamlConfigService : IYamlConfigService
    {
        private readonly Dictionary<string, object> _configuration;

        public YamlConfigService()
        {
            // YAMLファイルのパスを指定
            _configuration = SimpleYamlParser.Parse("appsettings.yml");
        }

        public Dictionary<string, object> GetConfiguration()
        {
            return _configuration;
        }
    }
}
