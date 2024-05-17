using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadData : Singleton<LoadData>
{
    public OptionData optionData;


    private void Start()
    {      
        LoadOptionDataFromJson();
    }

    [ContextMenu("To Json Data")]
    public void SaveOptionDataToJson()
    {
        optionData.saveWindowMode = UIManager.Instance.windowScreen;
        optionData.saveResolutionNum = UIManager.Instance.resolutionArrNum;
        optionData.saveBgmVolume = SoundManager.Instance.volumeBGM;
        optionData.saveEffectVolume = SoundManager.Instance.volumeEffect;
        string jsonData = JsonUtility.ToJson(optionData, true);
        string path = Path.Combine(Application.dataPath, "optionData.json");
        File.WriteAllText(path, jsonData);
        Debug.Log("저장");
    }

    [ContextMenu("From Json Data")]
    public void LoadOptionDataFromJson()
    {       
        string path = Path.Combine(Application.dataPath, "optionData.json");
        if(File.Exists(path))
        {
            Debug.Log("불러오기 성공");
            string jsonData = File.ReadAllText(path);
            optionData = JsonUtility.FromJson<OptionData>(jsonData);
        }
        else
        {
            Debug.Log("새로운 파일 생성");
            
            optionData.saveWindowMode = UIManager.Instance.windowScreen;
            optionData.saveResolutionNum = UIManager.Instance.resolutionArrNum;
            optionData.saveBgmVolume = SoundManager.Instance.volumeBGM;
            optionData.saveEffectVolume = SoundManager.Instance.volumeEffect;
        }      
    }


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
