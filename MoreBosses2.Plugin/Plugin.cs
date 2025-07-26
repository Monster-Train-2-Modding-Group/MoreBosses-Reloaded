using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System.Text;
using TrainworksReloaded.Core;
using TrainworksReloaded.Core.Extensions;

namespace MoreBosses2.Plugin
{
    public struct ScenarioSet
    {
        public string Scenario;
        public string Boss;
        public List<string> Enemies;
        public string Sins;

        public readonly void CollectReferencedFiles(ICollection<string> referencedFiles)
        {
            if (Scenario != null)
                referencedFiles.Add(Scenario);
            if (Boss != null)
                referencedFiles.Add(Boss);
            if (Sins != null)
                referencedFiles.Add(Sins);
            foreach (var enemy in Enemies)
                referencedFiles.Add(enemy);
        }
    }

    class ConfigDescriptionBuilder
    {
        public string English { get; set; } = "";
        public string French { get; set; } = "";
        public string German { get; set; } = "";
        public string Russian { get; set; } = "";
        public string Portuguese { get; set; } = "";
        public string Chinese { get; set; } = "";
        public string Spanish { get; set; } = "";
        public string ChineseTraditional { get; set; } = "";
        public string Korean { get; set; } = "";
        public string Japanese { get; set; } = "";

        public override string ToString()
        {
            StringBuilder builder = new();
            if (!string.IsNullOrEmpty(English)) builder.AppendLine(English);
            if (!string.IsNullOrEmpty(French)) builder.AppendLine(French);
            if (!string.IsNullOrEmpty(German)) builder.AppendLine(German);
            if (!string.IsNullOrEmpty(Russian)) builder.AppendLine(Russian);
            if (!string.IsNullOrEmpty(Portuguese)) builder.AppendLine(Portuguese);
            if (!string.IsNullOrEmpty(Chinese)) builder.AppendLine(Chinese);
            if (!string.IsNullOrEmpty(Spanish)) builder.AppendLine(Spanish);
            if (!string.IsNullOrEmpty(ChineseTraditional)) builder.AppendLine(ChineseTraditional);
            if (!string.IsNullOrEmpty(Korean)) builder.AppendLine(Korean);
            if (!string.IsNullOrEmpty(Japanese)) builder.AppendLine(Japanese);
            return builder.ToString().TrimEnd();
        }
    }

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);
        ConfigEntry<bool>? ArkionSoulCrusher;
        public List<ScenarioSet> Scenarios = [];

        public void Awake()
        {
            Logger = base.Logger;

            GatherConfig();
            GatherScenarios();
            List<string> files = [];
            foreach (var scenario in Scenarios)
            {
                scenario.CollectReferencedFiles(files);
            }

            var builder = Railhead.GetBuilder();
            builder.Configure(
                MyPluginInfo.PLUGIN_GUID,
                c => { c.AddMergedJsonFile(files); }
            );

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        public void GatherConfig()
        {
            ArkionSoulCrusher = Config.Bind<bool>("Scenarios", "Enable Arkion Soul Crusher", true,
                new ConfigDescriptionBuilder
                {
                    English = "Enable Arkion Soul Crusher Scenario (Slay: Apply Dazed 1 to your units).",
                }.ToString());
        }

        public void GatherScenarios()
        {
            if (ArkionSoulCrusher!.Value)
            {
                Scenarios.Add(new ScenarioSet
                {
                    Scenario = "json/scenarios/ArkionSoulCrusher.json",
                    Boss = "json/bosses/ArkionSoulCrusher.json",
                    Enemies = [
                        "json/enemies/ChosenChampion.json",
                    ],  
                    Sins = "json/sins/SoulCrusher.json"
                });
            }
        }
    }
}
