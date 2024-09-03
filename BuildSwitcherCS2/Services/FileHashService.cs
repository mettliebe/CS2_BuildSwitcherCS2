using System;
using System.IO;
using System.Security.Cryptography;

namespace BuildSwitcherCS2.Services;
internal class FileHashService {
    private static string DASH { get; } = "-";
    private HashAlgorithm HashAlgorithm { get; }
    public FileHashService() {
        this.HashAlgorithm = SHA512.Create();
    }
    public bool AreEqual(FileInfo file, FileInfo other) {
        string fileHash = this.CalculateHash(file);
        string otherHash = this.CalculateHash(other);
        return fileHash.Equals(otherHash);
    }
    private string CalculateHash(FileInfo fileInfo) {
        using FileStream stream = fileInfo.OpenRead();
        // dont make it async; it takes too long!!!
        byte[] bytes = this.HashAlgorithm.ComputeHash(stream);
        return BitConverter.ToString(bytes).Replace(DASH, String.Empty);
    }
}
