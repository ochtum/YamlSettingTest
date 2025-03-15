using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YamlSettingTest.Modules
{
    public class SimpleYamlParser
    {
        private enum YamlNodeType
        {
            Section,
            KeyValue,
            ListItem,
            Collection
        }
        public static Dictionary<string, object> Parse(string yamlContent)
        {
            var result = new Dictionary<string, object>();
            var sections = new Dictionary<int, string>();
            var section = string.Empty;
            int sectionLevel = 0;
            YamlNodeType nodeType = YamlNodeType.Section;
            int curretnSection = 0;
            var listSection = string.Empty;
            string[]? parts = null;
            List<string> itemList = null;

            var yamlData = File.ReadAllLines(yamlContent);

            foreach (var line in yamlData)
            {

                sectionLevel = GetSectionLevel(line);
                nodeType = GetNodeType(line);

                switch (nodeType)
                {
                    case YamlNodeType.Section:
                        listSection = string.Empty;
                        curretnSection = sectionLevel;
                        parts = line.Trim().Split(':', 2);
                        sections[curretnSection] = parts[0].Trim();

                        break;
                    case YamlNodeType.KeyValue:
                        listSection = string.Empty;
                        parts = line.Trim().Split(':', 2);
                        section = string.Empty;

                        if (curretnSection == sectionLevel)
                        {
                            curretnSection--;
                        }

                        for (int i  = curretnSection; i >= 0; i--)
                        {
                            if (sections.ContainsKey(i))
                            {
                                if (i == curretnSection)
                                {
                                    section = sections[i];
                                }
                                else
                                {
                                    section = sections[i] + ":" + section;
                                }
                            }
                        }
                        section += ":" + parts[0].Trim();
                        result[section] = parts[1].Trim();

                        break;
                    case YamlNodeType.ListItem:
                        if (string.IsNullOrEmpty(listSection))
                        {
                            listSection = sections[curretnSection];
                            curretnSection--;
                            section = string.Empty;

                            for (int i = curretnSection; i >= 0; i--)
                            {
                                if (sections.ContainsKey(i))
                                {
                                    if (i == curretnSection)
                                    {
                                        section = sections[i];
                                    }
                                    else
                                    {
                                        section = sections[i] + ":" + section;
                                    }
                                }
                            }

                            section += ":" + listSection;
                            parts = line.Trim().Split('-', 2);
                            itemList = [parts[1].Trim()];
                            result[section] = itemList;
                        }
                        else
                        {
                            parts = line.Trim().Split('-', 2);
                            if (result[section] is List<string> list)
                            {
                                list.Add(parts[1].Trim());
                            }
                        }

                        break;
                    case YamlNodeType.Collection:
                        if (line.Contains(":") && !line.StartsWith("\""))
                        {
                            parts = line.Trim().Split(':', 2);
                            section = string.Empty;

                            for (int i = curretnSection; i >= 0; i--)
                            {
                                if (sections.ContainsKey(i))
                                {
                                    if (i == curretnSection)
                                    {
                                        section = sections[i];
                                    }
                                    else
                                    {
                                        section = sections[i] + ":" + section;
                                    }
                                }
                            }
                            section += ":" + parts[0].Trim();
                            itemList = new List<string>();

                            if (parts[1].Trim().StartsWith("["))
                            {
                                var item = parts[1].Trim().TrimStart('[').TrimEnd(']');

                                if (item.Trim().StartsWith("\""))
                                {
                                    var items = item.Split(',');

                                    foreach (var itemValue in items)
                                    {
                                        if (!string.IsNullOrEmpty(itemValue))
                                        {
                                            itemList.Add(itemValue.Trim());
                                        }
                                    }
                                }
                            }
                            result[section] = itemList;
                        }
                        else
                        {
                            var item = line.Trim().TrimStart('[').TrimEnd(']');
                            if (result[section] is List<string> list)
                            {
                                var items = item.Split(',');
                                foreach (var itemValue in items)
                                {
                                    if (!string.IsNullOrEmpty(itemValue))
                                    {
                                        list.Add(itemValue.Trim());
                                    }
                                }
                            }
                        }
                        break;
                }

            }

            return result;
        }

        private static object ParseValue(string value)
        {
            if (int.TryParse(value, out int intValue)) return intValue;
            if (bool.TryParse(value, out bool boolValue)) return boolValue;
            return value; // 文字列として扱う
        }
        private static YamlNodeType GetNodeType(string line)
        {
            var trimmed = line.Trim();

            if (trimmed.StartsWith("- "))
            {
                return YamlNodeType.ListItem;
            }

            if (!trimmed.Contains(":"))
            {
                return YamlNodeType.Collection;
            }
            if (trimmed.StartsWith("\""))
            {
                return YamlNodeType.Collection;
            }

            var parts = trimmed.Split(':', 2);

            if (string.IsNullOrEmpty(parts[1].Trim()))
            {
                return YamlNodeType.Section;
            }
            if (parts[1].Trim().StartsWith("["))
            {
                return YamlNodeType.Collection;
            }
            return YamlNodeType.KeyValue;
        }

        private static int GetSectionLevel(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return 0;
            }

            var level = 0;
            foreach (var c in line)
            {
                if (c == ' ')
                {
                    level++;
                }
                else
                {
                    break;
                }
            }
            return level / 2;
        }
    }
}