using System.ComponentModel;
using System.Windows;

using Prism.Commands;

namespace BuildSwitcherCS2.ViewModels;
public class MainWindowViewModel {
    public DelegateCommand<RoutedEventArgs> WindowLoadedCommand { get; }
    public DelegateCommand<CancelEventArgs> WindowClosingCommand { get; }
    public MainWindowViewModel() {
        this.WindowLoadedCommand = new DelegateCommand<RoutedEventArgs>(this.WindowLoadedCommandAction);
        this.WindowClosingCommand = new DelegateCommand<CancelEventArgs>(this.WindowClosingCommandAction);
    }
    private void WindowClosingCommandAction(CancelEventArgs args) {
        //
    }

    private void WindowLoadedCommandAction(RoutedEventArgs args) {
        //if (Application.Current.Resources[] is string ) {
        // read startup parameters if needed
        //}
    }
}
