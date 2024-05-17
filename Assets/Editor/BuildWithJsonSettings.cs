using UnityEditor;
using UnityEngine;
using System.IO;

public class BuildWithJsonSettings : MonoBehaviour
{
    [MenuItem("Build/Build With JSON Settings")]
    public static void BuildWithSettings()
    {  
        // JSON 파일 경로
        string path = Path.Combine(Application.dataPath, "optionData.json");

        // JSON 파일 읽기
        if (File.Exists(path))
        {
            // 해상도 설정
            SetResolution(LoadData.Instance.optionData.saveResolutionNum, LoadData.Instance.optionData.saveWindowMode);

            // 빌드 대상 및 경로 설정
            BuildTarget target = BuildTarget.StandaloneWindows;
            string buildPath = "Builds/MyGame.exe";

            // 빌드 옵션 설정
            BuildOptions options = BuildOptions.None;

            // 빌드 실행
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath, target, options);
        }
        else
        {
            SetResolution(3, true);

            Debug.LogError("Settings file not found: " + path);
        }
    }
    
    private static void SetResolution(int resolutionArrNum, bool windowScreen)
    {
        switch (resolutionArrNum)
        {
            case 0:
                PlayerSettings.defaultScreenWidth = 1280;
                PlayerSettings.defaultScreenHeight = 720;
                PlayerSettings.fullScreenMode = windowScreen ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
                UIManager.Instance.resolutionText.text = UIManager.Instance.resolutionTextArr[resolutionArrNum];
                UIManager.Instance.windowScreen = windowScreen;
                break;
            case 1:
                PlayerSettings.defaultScreenWidth = 1280;
                PlayerSettings.defaultScreenHeight = 800;
                PlayerSettings.fullScreenMode = windowScreen ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
                UIManager.Instance.resolutionText.text = UIManager.Instance.resolutionTextArr[resolutionArrNum];
                UIManager.Instance.windowScreen = windowScreen;
                break;
            case 2:
                PlayerSettings.defaultScreenWidth = 1680;
                PlayerSettings.defaultScreenHeight = 1050;
                PlayerSettings.fullScreenMode = windowScreen ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
                UIManager.Instance.windowScreen = windowScreen;
                UIManager.Instance.resolutionText.text = UIManager.Instance.resolutionTextArr[resolutionArrNum];
                break;
            case 3:
                PlayerSettings.defaultScreenWidth = 1920;
                PlayerSettings.defaultScreenHeight = 1080;
                PlayerSettings.fullScreenMode = windowScreen ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
                UIManager.Instance.resolutionText.text = UIManager.Instance.resolutionTextArr[resolutionArrNum];
                UIManager.Instance.windowScreen = windowScreen;
                break;
            case 4:
                PlayerSettings.defaultScreenWidth = 1920;
                PlayerSettings.defaultScreenHeight = 1200;
                PlayerSettings.fullScreenMode = windowScreen ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
                UIManager.Instance.resolutionText.text = UIManager.Instance.resolutionTextArr[resolutionArrNum];
                UIManager.Instance.windowScreen = windowScreen;
                break;
            case 5:
                PlayerSettings.defaultScreenWidth = 2560;
                PlayerSettings.defaultScreenHeight = 1600;
                PlayerSettings.fullScreenMode = windowScreen ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
                UIManager.Instance.resolutionText.text = UIManager.Instance.resolutionTextArr[resolutionArrNum];
                UIManager.Instance.windowScreen = windowScreen;
                break;
            case 6:
                PlayerSettings.defaultScreenWidth = 3070;
                PlayerSettings.defaultScreenHeight = 1920;
                PlayerSettings.fullScreenMode = windowScreen ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
                UIManager.Instance.resolutionText.text = UIManager.Instance.resolutionTextArr[resolutionArrNum];
                UIManager.Instance.windowScreen = windowScreen;
                break;
            default:
                Debug.LogWarning("Invalid resolution index. Using default resolution.");
                PlayerSettings.defaultScreenWidth = 1920;
                PlayerSettings.defaultScreenHeight = 1080;
                PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
                UIManager.Instance.resolutionText.text = UIManager.Instance.resolutionTextArr[resolutionArrNum];

                break;
        }
    }
}
