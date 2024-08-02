using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public class LoadData : Singleton<LoadData>
{
    public OptionData optionData;

    protected override void Awake()
    {
        base.Awake();
        LoadOptionDataFromJson();
        UIManager.Instance.JsonUILoad();
        UIManager.Instance.SetResolution();
    }

    // 옵션 데이터를 JSON 파일로 저장
    [ContextMenu("To Json Data")]
    public void SaveOptionDataToJson()
    {
        try
        {
            optionData.saveWindowMode = UIManager.Instance.windowScreen;
            optionData.saveResolutionNum = UIManager.Instance.resolutionArrNum;
            optionData.saveBgmVolume = SoundManager.Instance.volumeBGM;
            optionData.saveEffectVolume = SoundManager.Instance.volumeEffect;
            //string jsonData = JsonUtility.ToJson(optionData, true);

            string jsonData = FormatJson(optionData);
            string path = GetFilePath();
            File.WriteAllText(path, jsonData);
        }
        catch (System.Exception ex)
        {
            // Debug.LogError($"옵션 데이터 저장 실패: {ex.Message}");
        }
    }


    private string FormatJson(OptionData data)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("{");
        sb.AppendLine($"    \"saveWindowMode\": {data.saveWindowMode.ToString().ToLower()},");
        sb.AppendLine($"    \"saveResolutionNum\": {data.saveResolutionNum},");
        sb.AppendLine($"    \"saveBgmVolume\": {data.saveBgmVolume.ToString("F1", CultureInfo.InvariantCulture)},");
        sb.AppendLine($"    \"saveEffectVolume\": {data.saveEffectVolume.ToString("F1", CultureInfo.InvariantCulture)}");
        sb.Append("}");
        return sb.ToString();
    }



    // JSON 파일에서 옵션 데이터를 로드
    [ContextMenu("From Json Data")]
    public void LoadOptionDataFromJson()
    {
        string path = GetFilePath();
        if (File.Exists(path))
        {
            try
            {
                // Debug.Log("옵션 데이터 불러오기 성공");
                string jsonData = File.ReadAllText(path);
                optionData = JsonUtility.FromJson<OptionData>(jsonData);
            }
            catch (System.Exception ex)
            {
                // Debug.LogError($"옵션 데이터 불러오기 실패: {ex.Message}");
                InitializeDefaultOptionData();
            }
        }
        else
        {
            // Debug.Log("옵션 데이터 파일이 존재하지 않습니다. 기본 옵션 데이터를 생성합니다.");
            InitializeDefaultOptionData();
        }
    }

    // 기본 옵션 데이터 초기화
    private void InitializeDefaultOptionData()
    {
        optionData = new OptionData
        {
            saveWindowMode = true,
            saveResolutionNum = 2,
            saveBgmVolume = 0.2f,
            saveEffectVolume = 0.2f
        };
        UIManager.Instance.JsonUILoad();
        UIManager.Instance.SetResolution();
    }

    // 옵션 데이터를 저장할 파일 경로 반환
    private string GetFilePath()
    {
        string fileName = "optionData.json";
        if (Application.platform == RuntimePlatform.Android)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
        else // PC나 다른 플랫폼의 경우
        {
            return Path.Combine(Application.dataPath, fileName);
        }
    }

    // 애플리케이션 종료 시 옵션 데이터 저장
    private void OnApplicationQuit()
    {
        SaveOptionDataToJson();
    }

    [System.Serializable]
    public class OptionData
    {
        public bool saveWindowMode;
        public int saveResolutionNum;
        public float saveBgmVolume;
        public float saveEffectVolume;
    }
}
