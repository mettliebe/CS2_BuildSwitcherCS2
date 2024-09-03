using System.Globalization;
using System.Windows;
using System.Windows.Markup;

using BuildSwitcherCS2.Configurations;
using BuildSwitcherCS2.Services;
using BuildSwitcherCS2.Views;
using BuildSwitcherCS2.Views.Contents;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using Prism.Unity;

namespace BuildSwitcherCS2;
/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication {
    public App() {
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag
                    )
                )
            );
    }/// <summary>
     ///     Step: 1
     /// </summary>
    protected override void OnStartup(StartupEventArgs e) {
        if (e.Args is not null && e.Args.Length > 0) {
            string arg = e.Args[0];
            // ability to add start parameters
            //App.Current.Resources.Add(, arg);
        }
        base.OnStartup(e);
    }

    /// <summary>
    ///     Step: 2
    /// </summary>
    protected override void RegisterTypes(IContainerRegistry containerRegistry) {
        containerRegistry.RegisterSingleton<UnityService>();
        containerRegistry.RegisterSingleton<InstallationService>();
        containerRegistry.RegisterSingleton<FileReplacerService>();
        containerRegistry.RegisterSingleton<FileLineService>();
        containerRegistry.RegisterSingleton<FileHashService>();
        containerRegistry.RegisterSingleton<CheckService>();
        containerRegistry.RegisterSingleton<SwitchService>();
    }

    /// <summary>
    ///     Step: 3
    ///     <br/>
    ///     <br/>
    ///     <seealso href="https://docs.prismlibrary.com/docs/modularity/index.html"/>
    ///     <br/>
    ///     <seealso href="https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Code"/>
    ///     <br/>
    ///     <seealso href="https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Directory"/>
    /// </summary>
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) {
        base.ConfigureModuleCatalog(moduleCatalog);
    }

    /// <summary>
    ///     Step: 4
    /// </summary>
    /// <returns></returns>
    protected override Window CreateShell() {
        MainWindow mainWindow = this.Container.Resolve<MainWindow>();
        return mainWindow;
    }
    /// <summary>
    ///     Step: 5
    /// </summary>
    protected override void OnInitialized() {
        IRegionManager regionManager = this.Container.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion<AppView>(AppConfigurationManager.AppMainRegion);

        //regionManager.RegisterViewWithRegion<>();
        // modules are initilized
        regionManager.RequestNavigate(AppConfigurationManager.AppMainRegion, nameof(AppView));
        base.OnInitialized();
    }
}

