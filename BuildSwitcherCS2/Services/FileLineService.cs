using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BuildSwitcherCS2.Services;
internal class FileLineService {
    public FileLineService() { }
    public async Task AppendLinesAsync(FileInfo file, string[] linesToAppend) {
        List<string> lines = (await File.ReadAllLinesAsync(file.FullName)).ToList();
        bool appended = Append(lines, linesToAppend);
        if (appended) {
            await File.WriteAllLinesAsync(file.FullName, lines);
        }
    }
    public async Task RemoveLinesAsync(FileInfo file, string[] linesToRemove) {
        List<string> lines = (await File.ReadAllLinesAsync(file.FullName)).ToList();
        bool removed = Remove(lines, linesToRemove);
        if (removed) {
            await File.WriteAllLinesAsync(file.FullName, lines);
        }
    }
    public async Task RemoveAppendLinesAsync(FileInfo file,
                                             string[] linesToRemove,
                                             string[] linesToAppend) {
        List<string> lines = (await File.ReadAllLinesAsync(file.FullName)).ToList();
        bool changed = false;
        {
            // remove
            changed = Remove(lines, linesToRemove);
        }
        {
            // append
            changed |= Append(lines, linesToAppend);
        }
        if (changed) {
            await File.WriteAllLinesAsync(file.FullName, lines);
        }
    }
    public async Task<bool> ContainsLineAsync(FileInfo file, string line) {
        List<string> lines = (await File.ReadAllLinesAsync(file.FullName)).ToList();
        return lines.Contains(line);
    }
    private static bool Append(IList<string> source, IEnumerable<string> linesToAppend) {
        bool changed = false;
        foreach (string lineToAppend in linesToAppend) {
            if (source.Contains(lineToAppend)) {
                continue;
            }
            source.Add(lineToAppend);
            changed = true;
        }
        return changed;
    }
    private static bool Remove(IList<string> source, IEnumerable<string> linesToRemove) {
        bool changed = false;
        foreach (string lineToRemove in linesToRemove) {
            changed |= source.Remove(lineToRemove);
        }
        return changed;
    }
}
