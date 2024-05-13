using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField] private GameObject[] BGMSquares; //음량 네모네모
    [SerializeField] private GameObject[] effectSquares;

    [Header("음량")]
    public float volumeBGM = 0.2f;
    public float volumeEffect = 0.2f;
    public float settingBGM = 0.2f;
    public float settingEffect = 0.2f;

    [Header("배경음악")]
    public AudioClip introBGM;
    //public AudioClip mapBGM;
    //public AudioClip stage1BGM;
    //public AudioClip stage2BGM;
    //public AudioClip stage3BGM;
    //public AudioClip resultBGM;

    [Header("효과음")]
    public AudioClip UISelect;
    public AudioClip UIPop;
    public AudioClip UITick;
    public AudioClip UIStart;




    public AudioSource BgmAudioSource;
    public AudioSource EffectAudioSource;

    private void Awake()
    {
        SettingAudioVolume();
    }

    void Start()
    {

        StartCoroutine("playIntro");
        //IntroBGM();
    }

    
    void Update()
    {
    }

    public void SettingAudioVolume()
    {
        BgmAudioSource.volume = 0f;
        EffectAudioSource.volume = volumeEffect;
    }

    public void upBgmSound()
    {
        volumeBGM += 0.1f;
        if (volumeBGM >= 1)
        {
            volumeBGM = 1f;
        }
        BgmAudioSource.volume = volumeBGM;

        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
    }
    
    public void downBgmSound()
    {
        volumeBGM -= 0.1f;
        if (volumeBGM <= 0)
        {
            volumeBGM = 0;
        }
        BgmAudioSource.volume = volumeBGM;
        UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
    }

    public void upEffectSound()
    {
        volumeEffect += 0.1f;
        if (volumeEffect >= 1)
        {
            volumeEffect = 1f;
        }
        EffectAudioSource.volume = volumeEffect;
        UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }
    public void downEffectSound()
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

    IEnumerator playIntro()
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
}
