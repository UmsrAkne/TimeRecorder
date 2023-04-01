using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TimeRecorder.Models;

public class ApplicationSetting
{
    public static string AppSettingFileName => "applicationSettings.json";

    public string DefaultComment { get; set; } = "Default Comment";

    public string DefaultAutoComment { get; set; } = "Default Comment (Auto )";

    public string RunAppMessage { get; set; } = "アプリ起動";

    public static void WriteApplicationSetting(ApplicationSetting setting)
    {
        var jsonSerializeSetting = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
        };

        string data = JsonConvert.SerializeObject(setting, jsonSerializeSetting);

        using StreamWriter sw = File.CreateText(AppSettingFileName);
        sw.Write(data);
    }

    public static ApplicationSetting ReadApplicationSetting(string jsonFilePath)
    {
        using var reader = new StreamReader(jsonFilePath);
        return new JsonSerializer().Deserialize<ApplicationSetting>(new JsonTextReader(reader));
    }
}