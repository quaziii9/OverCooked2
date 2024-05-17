using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private GameObject[] BGMSquares; // 음량 네모네모
    [SerializeField] private GameObject[] effectSquares;

    [Header("Volume")]
    public float volumeBGM = 0.2f;
    public float volumeEffect = 0.2f;
    public float settingBGM = 0.2f;
    public float settingEffect = 0.2f;

    [Header("BGM")]
    public AudioClip introBGM;
    public AudioClip battleBGM;
    public AudioClip busMapBGM;

    [Header("Sound Effect")]
    public AudioClip UISelect;
    public AudioClip UIPop;
    public AudioClip UITick;
    public AudioClip UIStart;
    public AudioClip screenInUI;
    public AudioClip screenOutUI;

    [Header("Audio Source")]
    public AudioSource bgmAudioSource;
    public AudioSource bgmChangeAudioSource;
    public AudioSource effectAudioSource;

    [Header("MultiPlay")]
    public bool isSingle = true; //싱글 멀티 구분
    public bool alreadyPlayed = false;

    [Header("Game Play Sound Effect")]
    public AudioClip itemTake;
    public AudioClip put;
    public AudioClip fall;
    public AudioClip throwItem;
    public AudioClip ready;
    public AudioClip go;
    public AudioClip bin;
    public AudioClip right;
    public AudioClip no;
    public AudioClip cut;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  // The music mixer group
    public AudioMixerGroup effectGroup; // The effect mixer group
    //public AudioMixerGroup BgmChangeGroup

    AudioSource musicSource;            // Reference to the generated music Audio Source
    AudioSource effectSource;           // Reference to the generated effect Audio Source

    void Start()
    {
        SettingAudioVolume();
        StartCoroutine(FadeInVolume(bgmAudioSource, 8f, "Intro"));

        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();

        musicSource.outputAudioMixerGroup = musicGroup;
        effectSource.outputAudioMixerGroup = effectGroup;
        //IntroBGM();
    }

    #region settingAuido
    public void SettingAudioVolume()
    {
        bgmAudioSource.volume = 0.1f;
        bgmChangeAudioSource.volume = 0.1f;
        effectAudioSource.volume = volumeEffect;
    }

    public void BGMVolumeUp()
    {
        volumeBGM += 0.1f;
        if (volumeBGM >= 1)
        {
            volumeBGM = 1f;
        }
        bgmAudioSource.volume = volumeBGM;
        bgmChangeAudioSource.volume = volumeBGM;


        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
    }

    public void BGMVolumeDown()
    {
        volumeBGM -= 0.1f;
        if (volumeBGM <= 0)
        {
            volumeBGM = 0;
        }
        bgmAudioSource.volume = volumeBGM;
        bgmChangeAudioSource.volume = volumeBGM;

        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
    }

    public void EffectSoundVolumeUp()
    {
        volumeEffect += 0.1f;
        if (volumeEffect >= 1)
        {
            volumeEffect = 1f;
        }
        effectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }

    public void EffectSoundVolumeDown()
    {
        volumeEffect -= 0.1f;
        if (volumeEffect <= 0)
        {
            volumeEffect = 0;
        }
        effectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }

    public void SettingSave()
    {
        settingBGM = volumeBGM;
        settingEffect = volumeEffect;
        bgmAudioSource.volume = volumeBGM;
        bgmChangeAudioSource.volume = volumeBGM;
        effectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }

    public void SettingCancle()
    {
        volumeBGM = settingBGM;
        volumeEffect = settingEffect;
        bgmAudioSource.volume = volumeBGM;
        bgmChangeAudioSource.volume = volumeBGM;
        effectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }
    #endregion

    #region FadeInOut
    public void FadeInAudio(AudioSource audioSource, float waitTime, string bgmName)
    {
        StartCoroutine(FadeInVolume(audioSource, waitTime, bgmName));
    }

    public void FadeOutAudio(AudioSource audioSource, float waitTime)
    {
        StartCoroutine(FadeOutVolume(audioSource, waitTime));
    }

    IEnumerator FadeInVolume(AudioSource audioSource, float waitTime, string bgmName)
    {
        yield return new WaitForSeconds(waitTime);
        playBgm(bgmName);
        while (audioSource.volume < volumeBGM)
        {
            audioSource.volume += Time.deltaTime * 0.2f;
            yield return new WaitForSeconds(Time.deltaTime * 0.5f);
        }
    }

    IEnumerator FadeOutVolume(AudioSource audioSource, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        while (audioSource.volume > 0)
        {
           // Debug.Log(audioSource.volume);
            audioSource.volume -= Time.deltaTime * 0.2f;
            yield return new WaitForSeconds(Time.deltaTime * 0.5f);
        }
        audioSource.volume = 0;
        audioSource.Stop();
    }
    #endregion

    void playBgm(string bgmName)
    {
        switch (bgmName)
        {
            case "Intro":
                IntroBGM();
                break;
            case "Battle":
                BattleBGM();
                    break;
            case "BusMap":
                BusMapBGM();
                break;
        }
    }

    void IntroBGM()
    {
        bgmAudioSource.clip = introBGM;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }

    void BattleBGM()
    {
        bgmChangeAudioSource.clip = battleBGM;
        bgmChangeAudioSource.loop = true;
        bgmChangeAudioSource.Play();
    }

    void BusMapBGM()
    {
        bgmChangeAudioSource.clip = busMapBGM;
        bgmChangeAudioSource.loop = true;
        bgmChangeAudioSource.Play();
    }

    public void ButtonPop()
    {
        effectAudioSource.PlayOneShot(UIPop);
    }

    public void ButtonTick()
    {
        effectAudioSource.PlayOneShot(UITick);
    }

    public void OnClickButton()
    {
        effectAudioSource.PlayOneShot(UISelect);
    }

    public void StartPlay()
    {
        effectAudioSource.PlayOneShot(UIStart);
    }

    public void ScreenInUI()
    {
        effectAudioSource.PlayOneShot(screenInUI);
    }

    public void ScreenOutUI()
    {
        effectAudioSource.PlayOneShot(screenOutUI);
    }

    public void PlayEffect(string effect)
    {
        switch (effect)
        {
            case "take":
                effectAudioSource.clip = itemTake;
                break;
            case "put":
                effectAudioSource.clip = put;
                break;
            case "fall":
                effectAudioSource.clip = fall;
                break;
            case "throwItem":
                effectAudioSource.clip = throwItem;
                break;
            case "ready":
                effectAudioSource.clip = ready;
                break;
            case "go":
                effectAudioSource.clip = go;
                break;
            case "bin":
                effectAudioSource.clip = bin;
                break;
            case "right":
                effectAudioSource.clip = right;
                break;
            case "no":
                effectAudioSource.clip = no;
                break;
            case "cut":
                effectAudioSource.clip = cut;
                break;
        }
        effectAudioSource.volume = volumeEffect;
        effectAudioSource.PlayOneShot(effectAudioSource.clip);
    }
}