using System.Diagnostics;

namespace BuildSwitcherCS2.Helpers;
public static class URLHelper {
    public static void Open(string url) {
        Process.Start(new ProcessStartInfo(url) {
            UseShellExecute = true
        });
    }
}
