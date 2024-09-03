using System.IO;
using System.Threading.Tasks;

using BuildSwitcherCS2.Configurations;

using static BuildSwitcherCS2.Services.CheckService;

namespace BuildSwitcherCS2.Services;
internal class SwitchService {
    private readonly FileLineService fileLineService;
    private readonly FileReplacerService fileReplacerService;
    private readonly InstallationService installationService;
    private readonly UnityService unityService;
    public SwitchService(FileLineService fileLineService,
                         FileReplacerService fileReplacerService,
                         InstallationService installationService,
                         UnityService unityService) {
        this.fileLineService = fileLineService;
        this.fileReplacerService = fileReplacerService;
        this.installationService = installationService;
        this.unityService = unityService;
    }
    public async Task SwitchAsync(SysState state) {
        switch (state) {
            case SysState.Corrupt:
            case SysState.Development:
                await this.SwitchToProductionAsync();
                break;
            case SysState.Production:
                await this.SwitchToDevelopmentAsync();
                break;
        }
    }
    private async Task SwitchToDevelopmentAsync() {
        FileInfo unityPlayerNew = this.unityService.GetUnityPlayerDllDevelopment();
        FileInfo unityPlayer = this.installationService.GetUnityPlayerDll();
        this.fileReplacerService.Replace(unityPlayerNew, unityPlayer);
        FileInfo bootConfig = this.installationService.GetBootConfig();
        await this.fileLineService.RemoveAppendLinesAsync(bootConfig,
                                                          [AppConfigurationManager.PlayConnectionDebugLine0, AppConfigurationManager.PlayConnectionDebugLine1],
                                                          [AppConfigurationManager.PlayConnectionDebugLine1]);
    }

    private async Task SwitchToProductionAsync() {
        FileInfo unityPlayerNew = this.unityService.GetUnityPlayerDllDevelopmentNon();
        FileInfo unityPlayer = this.installationService.GetUnityPlayerDll();
        this.fileReplacerService.Replace(unityPlayerNew, unityPlayer);
        FileInfo bootConfig = this.installationService.GetBootConfig();
        await this.fileLineService.RemoveLinesAsync(bootConfig,
                                                    [AppConfigurationManager.PlayConnectionDebugLine0, AppConfigurationManager.PlayConnectionDebugLine1]);
    }
}
