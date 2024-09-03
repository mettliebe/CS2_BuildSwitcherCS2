using System;
using System.Text;

namespace BuildSwitcherCS2.Configurations;
public static class AppConfigurationManager {
    static AppConfigurationManager() {
        SysStateCorruptText = BuildSysStateCorruptText();
    }
    private static string BuildSysStateCorruptText() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"game state seems to be corrupt");
        builder.AppendLine();
        builder.AppendLine($"a click on switch build");
        builder.AppendLine($"tries to switch to");
        builder.AppendLine($"production build");
        return builder.ToString();
    }
    public static string AppMainRegion { get; } = nameof(AppMainRegion);
    public static string ProjectUrl { get; } = "https://github.com/mettliebe/CS2_BuildSwitcherCS2";
    public static string Space { get; } = " ";
    public static string UnityPlayerDll { get; } = "UnityPlayer.dll";
    public static string Unity { get; } = nameof(Unity);
    public static string EnvironmentVariableNameCS2UnityVersion { get; } = "CSII_UNITYVERSION";
    public static string EnvironmentVariableNameCS2InstallationPath { get; } = "CSII_INSTALLATIONPATH";
    public static string Cities2DataDirectoryName { get; } = "Cities2_Data";
    public static string BootConfig { get; } = "boot.config";
    public static string PlayConnectionDebugLine1 { get; } = "player-connection-debug=1";
    public static string PlayConnectionDebugLine0 { get; } = "player-connection-debug=0";
    public static string SysStateDevelopmentText { get; } = $"next time,{Environment.NewLine}the game is started as{Environment.NewLine}development build";
    public static string SysStateProductionText { get; } = $"next time,{Environment.NewLine}the game is started as{Environment.NewLine}production build";
    public static string SysStateCorruptText { get; }
}
