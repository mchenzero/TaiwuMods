using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

class Program
{
    public static void Main(string[] args)
    {
        var srcDllDir = FindDllDirectory(GetGameDirectory(args));
        var requiredDlls = GetRequiredDllNames();
        CheckRequiredDlls(srcDllDir, requiredDlls);

        var dstDllDir = Path.Combine(GetScriptDirectory(), "dlls");
        Directory.CreateDirectory(dstDllDir); // no error if exist

        CopyDlls(srcDllDir, requiredDlls, dstDllDir);

        DecryptAssemblyCSharp(dstDllDir);
    }

    private static string GetGameDirectory(string[] args)
    {
        string gameDir;

        if (args.Length > 0)
        {
            gameDir = args[0];
            if (!Directory.Exists(gameDir))
            {
                throw new Exception($"Game directory \"{gameDir}\" does not exist.");
            }
        }
        else if (Directory.Exists(@"C:\Program Files\Steam\steamapps\common\The Scroll Of Taiwu"))
        {
            gameDir = @"C:\Program Files\Steam\steamapps\common\The Scroll Of Taiwu";
        }
        else if (Directory.Exists(@"C:\Program Files (x86)\Steam\steamapps\common\The Scroll Of Taiwu"))
        {
            gameDir = @"C:\Program Files (x86)\Steam\steamapps\common\The Scroll Of Taiwu";
        }
        else
        {
            throw new Exception("Usage: dotnet script restore.csx <GameDirectory>");
        }

        if (!File.Exists(Path.Combine(gameDir, "UnityPlayer.dll")))
        {
            throw new Exception($"\"{gameDir}\" does not seem to be the game directory.");
        }

        return gameDir;
    }

    private static string FindDllDirectory(string gameDir)
    {
        string dllDir = null;

        foreach (var subdir in Directory.GetDirectories(gameDir))
        {
            if (File.Exists(Path.Combine(subdir, "Managed", "Assembly-CSharp.dll")))
            {
                dllDir = Path.Combine(subdir, "Managed");
                break;
            }
        }

        if (string.IsNullOrEmpty(dllDir))
        {
            throw new Exception($"Could not find Managed DLL Directory in \"{gameDir}\".");
        }

        return dllDir;
    }

    private static string GetScriptDirectory([CallerFilePath] string path = null)
    {
        return Path.GetDirectoryName(path);
    }

    private static string[] GetRequiredDllNames()
    {
        var requiredDllNames = new SortedSet<string>();
        var repoSrcDir = Path.Combine(GetScriptDirectory(), "src");

        foreach (var projDir in Directory.GetDirectories(repoSrcDir))
        {
            foreach (var projFile in Directory.GetFiles(projDir, "*.csproj"))
            {
                var content = File.ReadAllText(projFile);
                var matches = Regex.Matches(content, @"<HintPath>..[/\\]..[/\\]dlls[/\\](.*.dll)</HintPath>");
                foreach (Match match in matches)
                {
                    var dllName = match.Groups[1].Value;
                    requiredDllNames.Add(dllName);
                }
            }
        }

        return requiredDllNames.ToArray();
    }

    private static void CheckRequiredDlls(string srcDllDir, string[] requiredDlls)
    {
        var missingDlls = new List<string>();

        foreach (var dllName in requiredDlls)
        {
            var dllPath = Path.Combine(srcDllDir, dllName);
            if (!File.Exists(dllPath))
            {
                missingDlls.Add(dllPath);
            }
        }

        if (missingDlls.Count == 1)
        {
            if (Path.GetFileName(missingDlls[0]) == "0Harmony12.dll")
            {
                throw new Exception($"Could not find DLL \"{missingDlls[0]}\". Have you installed UnityModManager?");
            }
            else
            {
                throw new Exception($"Could not find DLL \"{missingDlls[0]}\".");
            }
        }
        else if (missingDlls.Count > 1)
        {
            throw new Exception("Could not find the following DLLs:\n" + string.Join("\n", missingDlls));
        }
    }

    private static void CopyDlls(string srcDllDir, string[] dllNames, string dstDllDir)
    {
        foreach (var dllName in dllNames)
        {
            File.Copy(Path.Combine(srcDllDir, dllName), Path.Combine(dstDllDir, dllName), true);
            Console.WriteLine("Copied " + dllName);
        }
    }

    private static void DecryptAssemblyCSharp(string dstDllDir)
    {
        var dllName = "Assembly-CSharp.dll";
        var dllPath = Path.Combine(dstDllDir, dllName);

        if (File.Exists(dllPath))
        {
            var data = File.ReadAllBytes(dllPath);
            var key = Encoding.ASCII.GetBytes("8moQs6YuA2VnNzNL");

            File.WriteAllBytes(dllPath, XXTEA.Decrypt(data, key));

            Console.WriteLine("Decrypted " + dllName);
        }
    }
}

class XXTEA
{
    public static byte[] Decrypt(byte[] data, byte[] key)
    {
        return ToByteArray(Decrypt(ToUInt32Array(data), ToUInt32Array(key)));
    }

    private static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
    {
        var n = v.Length;
        var d = 0x9e3779b9;
        var q = 6 + 52 / n;

        var y = v[0];
        var s = (UInt32) q * d;

        while (s != 0) {
            var e = s >> 2 & 3;
            for (var p = n - 1; p >= 0; p--)
            {
                var z = v[p > 0 ? p - 1 : n - 1];
                var mx = (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (s ^ y) + (k[p & 3 ^ e] ^ z);
                y = v[p] -= mx;
            }
            s -= d;
        }

        return v;
    }

    private static UInt32[] ToUInt32Array(byte[] data)
    {
        var n = (data.Length >> 2) + ((data.Length & 3) > 0 ? 1 : 0);
        var result = new UInt32[n];

        for (var i = 0; i < data.Length; i++)
        {
            result[i >> 2] |= (UInt32) data[i] << ((i & 3) << 3);
        }

        return result;
    }

    private static byte[] ToByteArray(UInt32[] data)
    {
        var result = new byte[data.Last()];

        for (var i = 0; i < result.Length; i++)
        {
            result[i] = (byte) (data[i >> 2] >> ((i & 3) << 3));
        }

        return result;
    }
}

try
{
    Program.Main(Args.ToArray());
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    return 1;
}
