using DG.Tweening.Core.Easing;
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


    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  // The music mixer group
    public AudioMixerGroup effectGroup; // The effect mixer group
    //public AudioMixerGroup BgmChangeGroup


    AudioSource musicSource;            // Reference to the generated music Audio Source
    AudioSource effectSource;           // Reference to the generated effect Audio Source

    private void Awake()
    {
        SettingAudioVolume();
    }

    void Start()
    {
        StartCoroutine(FadeInVolume(bgmAudioSource, 8f));

        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();

        musicSource.outputAudioMixerGroup = musicGroup;
        effectSource.outputAudioMixerGroup = effectGroup;
        //IntroBGM();
    }

    public void SettingAudioVolume()
    {
        bgmAudioSource.volume = 0.1f;
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
        effectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }

    public void SettingCancle()
    {
        volumeBGM = settingBGM;
        volumeEffect = settingEffect;
        bgmAudioSource.volume = volumeBGM;
        effectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }

    IEnumerator FadeInVolume(AudioSource AudioSource, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        IntroBGM();
        while (AudioSource.volume < volumeBGM)
        {
            AudioSource.volume += Time.deltaTime * 0.2f;
            yield return new WaitForSeconds(Time.deltaTime * 0.5f);
        }
    }


    void IntroBGM()
    {
        bgmAudioSource.clip = introBGM;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
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
}