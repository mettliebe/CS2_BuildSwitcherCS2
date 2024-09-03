using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using BuildSwitcherCS2.Configurations;
using BuildSwitcherCS2.Helpers;
using BuildSwitcherCS2.Services;

using Prism.Commands;
using Prism.Mvvm;

using static BuildSwitcherCS2.Services.CheckService;

namespace BuildSwitcherCS2.ViewModels.Contents;
internal class AppViewModel : BindableBase {
    private readonly CheckService checkService;
    private readonly SwitchService switchService;



    private SysState _CurrentState = SysState.None;
    public SysState CurrentState {
        get => this._CurrentState;
        set => this.SetProperty(ref this._CurrentState, value, this.OnStateChanged);
    }

    private string _Source = "/BuildSwitcherCS2;component/Assets/Icons/warning.png";
    public string Source {
        get => this._Source;
        set => this.SetProperty(ref this._Source, value);
    }

    private string _Text = String.Empty;
    public string Text {
        get => this._Text;
        set => this.SetProperty(ref this._Text, value, () => this.Tooltip = this.Text);
    }

    private string _Tooltip = String.Empty;
    public string Tooltip {
        get => this._Tooltip;
        set => this.SetProperty(ref this._Tooltip, value);
    }

    private Brush _Background = Brushes.Transparent;
    public Brush Background {
        get => this._Background;
        set => this.SetProperty(ref this._Background, value);
    }

    private bool _IsEnabled = false;
    public bool IsEnabled {
        get => this._IsEnabled;
        set => this.SetProperty(ref this._IsEnabled, value);
    }



    public DelegateCommand OnLoadedCommand { get; }
    public DelegateCommand SwitchCommand { get; }
    public DelegateCommand HelpCommand { get; }
    public AppViewModel(CheckService checkService,
                        SwitchService switchService) {
        this.checkService = checkService;
        this.switchService = switchService;
        this.OnLoadedCommand = new DelegateCommand(this.OnLoadedCommandAction);
        this.SwitchCommand = new DelegateCommand(this.SwitchCommandAction);
        this.HelpCommand = new DelegateCommand(this.HelpCommandAction);
    }
    private async void OnLoadedCommandAction() {
        await this.CheckAction();
        this.Enable();
    }
    private async Task CheckAction() {
        CheckResult result = await this.checkService.CheckStateAsync();
        this.CurrentState = result.State;
        string message = result.GetMessage();
        if (!String.IsNullOrEmpty(message)
            && !String.IsNullOrWhiteSpace(message)) {
            this.CurrentState = SysState.Corrupt;
            this.Text += message;
        }
        if (result.ExceptionOccured) {
            if (this.CurrentState is not SysState.Corrupt) {
                this.CurrentState = SysState.Corrupt;
            }
            this.HandleException(result.Exception);
        }
    }
    private void Enable() {
        this.IsEnabled = true;
    }
    private void Disable() {
        this.IsEnabled = false;
    }
    private void HelpCommandAction() {
        URLHelper.Open(AppConfigurationManager.ProjectUrl);
    }
    private async void SwitchCommandAction() {
        this.Disable();
        await Task.Factory.StartNew(this.SwitchAction);
    }
    private async Task SwitchAction() {
        try {
            await this.switchService.SwitchAsync(this.CurrentState);
            await this.CheckAction();
        } catch (Exception ex) {
            this.CurrentState = SysState.Corrupt;
            this.HandleException(ex);
        } finally {
            this.Enable();
        }
    }


    private void OnStateChanged() {
        switch (this.CurrentState) {
            case SysState.None:
                break;
            case SysState.Development:
                this.Background = Brushes.DodgerBlue;
                this.Source = "/BuildSwitcherCS2;component/Assets/Icons/desktop_pulse.png";
                this.Text = AppConfigurationManager.SysStateDevelopmentText;
                break;
            case SysState.Production:
                this.Background = Brushes.DarkGreen;
                this.Source = "/BuildSwitcherCS2;component/Assets/Icons/desktop.png";
                this.Text = AppConfigurationManager.SysStateProductionText;
                break;
            default:
                this.Background = Brushes.DarkRed;
                this.Source = "/BuildSwitcherCS2;component/Assets/Icons/warning.png";
                this.Text = AppConfigurationManager.SysStateCorruptText;
                break;
        }
    }

    private void HandleException(Exception? ex) {
        if (ex is null) {
            return;
        }
        StringBuilder builder = new StringBuilder();
        builder.AppendLine();
        builder.AppendLine("Something went very wrong");
        builder.AppendLine();
        builder.AppendLine(ex.Message);
        this.Text += builder.ToString();
    }
}
