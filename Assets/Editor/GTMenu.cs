using System.IO;
using UnityEditor;
using GT.Asset;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class GTMenu
{
    public const string CLIENT = "CLIENT";
    public const string GT_DEVELOPMENT = "GT_DEVELOPMENT";

    /// <summary>
    /// Note: if switching doesn't work make sure build target platform is set.
    /// </summary>
    [MenuItem("Game/Set define")]
    private static void SwitchToDealerMode()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL,
            $"{GT_DEVELOPMENT};{CLIENT};");
    }

    private const string AssetExtension = ".asset";
    private static string _activeConfigPath = "Assets/Resources/ActiveConfig";

    [MenuItem("Game/Scene/BootScene %&i")]
    public static void OpenBootScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/{SceneAddress.BootScene}.unity");
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Game/Scene/Game  %&g")]
    public static void OpenWheelRoomScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene($"Assets/Scenes/{SceneAddress.GameScene}.unity");
    }

    [MenuItem("File/Open Project Directory", false, 0)]
    public static void OpenProjectDir()
    {
        Application.OpenURL("file://");
    }

    public static void ClearBuildDir()
    {
        if (Directory.Exists("build"))
            Directory.Delete("build", true);    
    }
}