using System;
using System.IO;
using System.Linq;
using UnityEditor;

public class BuildScript
{
    private static readonly string _versionNumber;
    private static readonly string _buildNumber;

    static BuildScript()
    {
        _versionNumber = "1.0.0.0";
        _buildNumber = "1";

        PlayerSettings.bundleVersion = _versionNumber;
    }

    static void CheckDir(string dir)
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    static string[] GetScenes()
    {
        return EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();
    }

    [MenuItem("Custom/Build Android")]
    static void Android()
    {
        CheckDir("build");
        int versionCode;
        int.TryParse(_buildNumber, out versionCode);
        PlayerSettings.Android.bundleVersionCode = versionCode;
        //PlayerSettings.Android.keyaliasName = "StickmanEPIC";
        //PlayerSettings.Android.keyaliasPass =
        //    PlayerSettings.Android.keystorePass = "hcents3@";
        //PlayerSettings.Android.keystoreName = Path.GetFullPath(@"NuGet\Android\Epic2.keystore").Replace('\\', '/');
        BuildPipeline.BuildPlayer(GetScenes(), "build/UnityBuild.apk", BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    [MenuItem("Custom/Build iOS")]
    static void iOS()
    {
        CheckDir("/Users/konradhanus/Desktop/BuildJenkins/GTA7/Scratch/Xcode");
        BuildPipeline.BuildPlayer(GetScenes(), "/Users/konradhanus/Desktop/BuildJenkins/GTA7/Scratch/Xcode", BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
    }
}