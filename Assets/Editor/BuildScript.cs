using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    private static readonly string _versionNumber;
    private static readonly string _buildNumber;

    static BuildScript()
    {
        _versionNumber = "4.0.1";
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
        try
        {
            CheckDir("build");
            int versionCode;
            int.TryParse(_buildNumber, out versionCode);
            PlayerSettings.Android.bundleVersionCode = versionCode;
            //PlayerSettings.Android.keyaliasName = "StickmanEPIC";
            //PlayerSettings.Android.keyaliasPass =
            //    PlayerSettings.Android.keystorePass = "hcents3@";
            //PlayerSettings.Android.keystoreName = Path.GetFullPath(@"NuGet\Android\Epic2.keystore")
            //    .Replace('\\', '/');
            BuildPipeline.BuildPlayer(GetScenes(), "build/UnityBuild.apk", BuildTarget.Android, BuildOptions.None);
        }
        catch (Exception ex)
        {
            // Zapisz wyjątek do pliku
            Debug.Log("Exception occurred in Android build:");
            Debug.Log(ex.ToString());
            Debug.Log("---------------------------------------");

            string logFilePath = "build_error_log.txt";
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine("Exception occurred in Android build:");
                writer.WriteLine(ex.ToString());
                writer.WriteLine("---------------------------------------");
            }

            // Wyrzuć wyjątek ponownie, aby program został zakończony z odpowiednim kodem wyjścia
            throw;
        }
    }

    [MenuItem("Custom/Build iOS")]
   static void iOS()
    {
        string buildPath = "/Users/konradhanus/Desktop/GIT/_builds2";

        if (!Directory.Exists(buildPath) || Directory.GetFiles(buildPath).Length == 0)
        {
            CheckDir(buildPath);
            Debug.Log("Folder is empty or does not exist. Setting BuildOptions to None.");
            BuildPipeline.BuildPlayer(GetScenes(), buildPath, BuildTarget.iOS, BuildOptions.None);
        }
        else
        {
            Debug.Log("Folder contains files. Proceeding with AcceptExternalModificationsToPlayer.");
            BuildPipeline.BuildPlayer(GetScenes(), buildPath, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
        }
    }
}