using System.IO;

namespace BuildSwitcherCS2.Services;
internal class FileReplacerService {
    public FileReplacerService() { }
    public void Replace(FileInfo from, FileInfo to) {
        File.Copy(from.FullName,
                  to.FullName,
                  true);
    }
}
