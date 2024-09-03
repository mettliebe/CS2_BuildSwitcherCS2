using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using BuildSwitcherCS2.Configurations;

namespace BuildSwitcherCS2.Services;
internal class CheckService {
    private readonly UnityService unityService;
    private readonly InstallationService installationService;
    private readonly FileLineService fileLineService;
    private readonly FileHashService fileHashService;
    public CheckService(UnityService unityService,
                        InstallationService installationService,
                        FileLineService fileLineService,
                        FileHashService fileHashService) {
        this.unityService = unityService;
        this.installationService = installationService;
        this.fileLineService = fileLineService;
        this.fileHashService = fileHashService;
    }
    public async Task<CheckResult> CheckStateAsync() {
        CheckResult result = new CheckResult(SysState.Corrupt);
        try {
            if (await this.IsDevelopmentAsync(result)) {
                result.State = SysState.Development;
            }
            // dont use else if
            // has to do with GetMessage() and the existence-check...
            if (await this.IsProductionAsync(result)) {
                result.State = SysState.Production;
            }
        } catch (Exception ex) {
            result.Exception = ex;
        }
        return result;
    }
    public async Task<bool> IsDevelopmentAsync(CheckResult checkResult) {
        FileInfo from = this.unityService.GetUnityPlayerDllDevelopment();
        FileInfo to = this.installationService.GetUnityPlayerDll();
        FileInfo bootConfig = this.installationService.GetBootConfig();
        checkResult.SetExistencesDev(from, to, bootConfig);

        if (checkResult.AreFilesExistentDev) {

            bool isDevUnityPlayer = this.fileHashService.AreEqual(from, to);
            checkResult.IsGameUnityPlayerDev = isDevUnityPlayer;

            bool containsDebug1 = await this.fileLineService.ContainsLineAsync(bootConfig, AppConfigurationManager.PlayConnectionDebugLine1);
            checkResult.BootConfigContainsDebug1 = containsDebug1;

            if (isDevUnityPlayer && containsDebug1) {
                return true;
            }
        }
        return false;
    }
    public async Task<bool> IsProductionAsync(CheckResult checkResult) {
        FileInfo from = this.unityService.GetUnityPlayerDllDevelopmentNon();
        FileInfo to = this.installationService.GetUnityPlayerDll();
        FileInfo bootConfig = this.installationService.GetBootConfig();
        checkResult.SetExistencesProd(from, to, bootConfig);

        if (checkResult.AreFilesExistentProd) {

            bool isProdUnityPlayer = this.fileHashService.AreEqual(from, to);
            checkResult.IsGameUnityPlayerProd = isProdUnityPlayer;

            bool containsDebug1 = await this.fileLineService.ContainsLineAsync(bootConfig, AppConfigurationManager.PlayConnectionDebugLine1);
            checkResult.BootConfigContainsDebug1 = containsDebug1;

            bool containsDebug0 = await this.fileLineService.ContainsLineAsync(bootConfig, AppConfigurationManager.PlayConnectionDebugLine0);
            checkResult.BootConfigContainsDebug0 = containsDebug0;
            if (isProdUnityPlayer) {
                if (!containsDebug1
                    || containsDebug0) {
                    return true;
                }
            }
        }
        return false;
    }
    public enum SysState {
        None,
        Development,
        Production,
        Corrupt
    }
    public class CheckResult {
        public SysState State { get; set; }
        public bool ExistsUnityPlayerDev { get; set; }
        public bool ExistsUnityPlayerProd { get; set; }
        public bool ExistsUnityPlayerCS2 { get; set; }
        public bool ExistsBootConfig { get; set; }
        public bool BootConfigContainsDebug0 { get; set; }
        public bool BootConfigContainsDebug1 { get; set; }
        public bool IsGameUnityPlayerDev { get; set; }
        public bool IsGameUnityPlayerProd { get; set; }
        public bool AreFilesExistentProd => this.ExistsUnityPlayerProd && this.ExistsUnityPlayerCS2 && this.ExistsBootConfig;
        public bool AreFilesExistentDev => this.ExistsUnityPlayerDev && this.ExistsUnityPlayerCS2 && this.ExistsBootConfig;
        public Exception? Exception { get; set; }
        public bool ExceptionOccured => this.Exception is not null;
        public CheckResult(SysState state) {
            this.State = state;
        }
        public void SetExistencesDev(FileInfo unityPlayerDev, FileInfo unityPlayerCS2, FileInfo bootConfig) {
            this.ExistsUnityPlayerDev = unityPlayerDev.Exists;
            this.ExistsUnityPlayerCS2 = unityPlayerCS2.Exists;
            this.ExistsBootConfig = bootConfig.Exists;
        }
        public void SetExistencesProd(FileInfo unityPlayerProd, FileInfo unityPlayerCS2, FileInfo bootConfig) {
            this.ExistsUnityPlayerProd = unityPlayerProd.Exists;
            this.ExistsUnityPlayerCS2 = unityPlayerCS2.Exists;
            this.ExistsBootConfig = bootConfig.Exists;
        }
        public string GetMessage() {
            StringBuilder builder = new StringBuilder();
            if (!this.ExistsUnityPlayerDev || !this.ExistsUnityPlayerProd) {
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine("Is the Modding-Toolchain");
                builder.AppendLine("installed and up to date");
                builder.AppendLine("(Game got updated - Modding-Toolchain not)?");
            }
            if (!this.ExistsUnityPlayerCS2 || !this.ExistsBootConfig) {
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine("Is the Game installed?");
            }
            if (this.IsGameUnityPlayerProd && this.BootConfigContainsDebug1) {
                // no additional information needed - i guess
            }
            return builder.ToString();
        }
    }
}
