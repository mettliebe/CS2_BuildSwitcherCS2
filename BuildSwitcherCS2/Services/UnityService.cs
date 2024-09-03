using System;
using System.IO;

using BuildSwitcherCS2.Configurations;

namespace BuildSwitcherCS2.Services;
internal class UnityService {
    private string PathWithin { get; } = "Editor\\Data\\PlaybackEngines\\windowsstandalonesupport\\Variations\\win64_player_{0}development_mono";
    private string Non { get; } = nameof(Non).ToLower();
    public UnityService() { }
    public FileInfo GetUnityPlayerDllDevelopment() {
        return this.GetUnityPlayerDll(true);
    }
    public FileInfo GetUnityPlayerDllDevelopmentNon() {
        return this.GetUnityPlayerDll(false);
    }
    private string? GetVersion() {
        return Environment.GetEnvironmentVariable(AppConfigurationManager.EnvironmentVariableNameCS2UnityVersion);
    }
    private DirectoryInfo GetProgramFilesDirectoryInfo() {
        string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        return new DirectoryInfo(programFilesPath);
    }
    private FileInfo GetUnityPlayerDll(bool dev) {
        DirectoryInfo programFilesDirectoryInfo = this.GetProgramFilesDirectoryInfo();
        string? version = this.GetVersion();
        ArgumentNullException.ThrowIfNull(version);
        string unityDirectory = AppConfigurationManager.Unity + AppConfigurationManager.Space + version;
        string inject = dev ? String.Empty : this.Non;
        string pathWithin = String.Format(this.PathWithin, inject);
        string from = Path.Combine(programFilesDirectoryInfo.FullName,
                                   unityDirectory,
                                   pathWithin,
                                   AppConfigurationManager.UnityPlayerDll);
        return new FileInfo(from);
    }
}
