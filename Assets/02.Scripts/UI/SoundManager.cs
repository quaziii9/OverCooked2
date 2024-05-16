using DG.Tweening.Core.Easing;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private GameObject[] BGMSquares; //음량 네모네모
    [SerializeField] private GameObject[] effectSquares;

    [Header("Volume")]
    public float volumeBGM = 0.2f;
    public float volumeEffect = 0.2f;
    public float settingBGM = 0.2f;
    public float settingEffect = 0.2f;

    [Header("BGM")]
    public AudioClip introBGM;

    [Header("Sound Effect")]
    public AudioClip UISelect;
    public AudioClip UIPop;
    public AudioClip UITick;
    public AudioClip UIStart;

    [Header("Audio Source")]
    public AudioSource BgmAudioSource;
    public AudioSource EffectAudioSource;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  // The music mixer group
    public AudioMixerGroup effectGroup; // The effect mixer group

    AudioSource musicSource;            // Reference to the generated music Audio Source
    AudioSource effectSource;           // Reference to the generated effect Audio Source

    private void Awake()
    {
        SettingAudioVolume();
    }

    void Start()
    {
        StartCoroutine("FadeInBGM");

        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();

        musicSource.outputAudioMixerGroup = musicGroup;
        effectSource.outputAudioMixerGroup = effectGroup;
        //IntroBGM();
    }

    public void SettingAudioVolume()
    {
        BgmAudioSource.volume = 0f;
        EffectAudioSource.volume = volumeEffect;
    }

    public void BGMVolumeUp()
    {
        volumeBGM += 0.1f;
        if (volumeBGM >= 1)
        {
            volumeBGM = 1f;
        }
        BgmAudioSource.volume = volumeBGM;

        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
    }
    
    public void BGMVolumeDown()
    {
        volumeBGM -= 0.1f;
        if (volumeBGM <= 0)
        {
            volumeBGM = 0;
        }
        BgmAudioSource.volume = volumeBGM;
        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
    }

    public void EffectSoundVolumeUp()
    {
        volumeEffect += 0.1f;
        if (volumeEffect >= 1)
        {
            volumeEffect = 1f;
        }
        EffectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }

    public void EffectSoundVolumeDown()
    {
        volumeEffect -= 0.1f;
        if (volumeEffect <= 0)
        {
            volumeEffect = 0;
        }
        EffectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }

    public void SettingSave()
    {
        settingBGM = volumeBGM;
        settingEffect = volumeEffect;
        BgmAudioSource.volume = volumeBGM;
        EffectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }

    public void SettingCancle()
    {
        volumeBGM = settingBGM;
        volumeEffect = settingEffect;
        BgmAudioSource.volume = volumeBGM;
        EffectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }

    IEnumerator FadeInBGM()
    {
        yield return new WaitForSeconds(8f);        
        IntroBGM();
        while (BgmAudioSource.volume < volumeBGM)
        {
            BgmAudioSource.volume += Time.deltaTime * 0.2f;
            yield return new WaitForSeconds(Time.deltaTime * 0.5f);
        }
    }
    
    void IntroBGM()
    {
        BgmAudioSource.clip = introBGM;
        BgmAudioSource.loop = true;
        BgmAudioSource.Play();
    }

    public void ButtonPop()
    {
        EffectAudioSource.PlayOneShot(UIPop);
    }

    public void ButtonTick()
    {
        EffectAudioSource.PlayOneShot(UITick);
    }

    public void OnClickButton()
    {
        EffectAudioSource.PlayOneShot(UISelect);
    }

    public void StartPlay()
    {
        EffectAudioSource.PlayOneShot(UIStart);
    }

    [Header("MultiPlay")]
    public bool isSingle = true; //싱글 멀티 구분
    public bool alreadyPlayed = false;

    [Header("Game Play Sound Effect")]
    public AudioClip itemTake;
    public AudioClip put;
    public AudioClip place;
    public AudioClip fall;
    public AudioClip throwItem;
    public AudioClip ready;
    public AudioClip go;

    public void PlayEffect(string effect)
    {
        switch (effect)
        {
            case "take":
                EffectAudioSource.clip = itemTake;
                break;
            case "put":
                EffectAudioSource.clip = put;
                break;
            case "place":
                EffectAudioSource.clip = place;
                break;
            case "fall":
                EffectAudioSource.clip = fall;
                break;
            case "throwItem":
                EffectAudioSource.clip = throwItem;
                break;
            case "ready":
                EffectAudioSource.clip = ready;
                break;
            case "go":
                EffectAudioSource.clip = go;
                break;
        }
        EffectAudioSource.volume = volumeEffect;
        EffectAudioSource.PlayOneShot(EffectAudioSource.clip);
    }
}