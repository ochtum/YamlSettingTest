# YamlSettingTest

## 目的
設定をJSONで管理すると、変更するときや設定値を追加するときに面倒くさいので、YAMLで管理できるようにParserを実装し、DIで呼び出せるようにしました。

## YAMLパーサー

- YAML-Parser
  - YamlSettingTest\YamlSettingTest\Modules\YamlParser.cs

- YAML-Service
  - YamlSettingTest\YamlSettingTest\Services\IYamlConfigService.cs
  - YamlSettingTest\YamlSettingTest\Services\YamlConfigService.cs
 
## サンプルソース

- 設定ファイルサンプル
  - YamlSettingTest\YamlSettingTest\appsettings.yml
- DIサンプル
  - YamlSettingTest\YamlSettingTest\Program.cs
  - YamlSettingTest\YamlSettingTest\Components\Pages\Weather.razor
