using System;
using System.IO;

using BuildSwitcherCS2.Configurations;

namespace BuildSwitcherCS2.Services;
internal class InstallationService {
    public InstallationService() { }
    public FileInfo GetBootConfig() {
        DirectoryInfo installationPath = this.GetInstallationPathDirectoryInfo();
        string bootConfigPath = Path.Combine(installationPath.FullName,
                                             AppConfigurationManager.Cities2DataDirectoryName,
                                             AppConfigurationManager.BootConfig);
        return new FileInfo(bootConfigPath);
    }
    public FileInfo GetUnityPlayerDll() {
        DirectoryInfo installationPath = this.GetInstallationPathDirectoryInfo();
        string bootConfigPath = Path.Combine(installationPath.FullName,
                                             AppConfigurationManager.UnityPlayerDll);
        return new FileInfo(bootConfigPath);
    }
    private string? GetInstallationPath() {
        return Environment.GetEnvironmentVariable(AppConfigurationManager.EnvironmentVariableNameCS2InstallationPath);
    }
    private DirectoryInfo GetInstallationPathDirectoryInfo() {
        string? path = this.GetInstallationPath();
        ArgumentNullException.ThrowIfNull(path);
        return new DirectoryInfo(path);
    }
}
