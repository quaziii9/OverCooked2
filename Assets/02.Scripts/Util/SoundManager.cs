using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EnumTypes;
using EventLibrary;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private GameObject[] BGMSquares; // 음량 네모네모
    [SerializeField] private GameObject[] effectSquares;
    [SerializeField] private GameObject[] mobileBGMSquares; 
    [SerializeField] private GameObject[] mobileEffectSquares;
    
    [Header("Audio Source")]
    public AudioSource bgmAudioSource;
    public AudioSource bgmChangeAudioSource;
    public AudioSource effectAudioSource;
    public AudioSource stageBackGroundAudioSource;
    public AudioSource stageAudioSource;
    public AudioSource stageEffectAudioSource;
    public AudioSource vanAudioSource;

    [Header("Volume")]
    public float volumeBGM = 0.2f;
    public float volumeEffect = 0.2f;
    public float settingBGM = 0.2f;
    public float settingEffect = 0.2f;

    [Header("BGM")]
    public AudioClip introBGM;
    public AudioClip battleBGM;
    public AudioClip worldMapBGM;
    public AudioClip stageBGM;
    public AudioClip sushiBGM;
    public AudioClip mineBGM;
    public AudioClip wizardBGM;


    [Header("Stage Sound Bgm & Effect")]
    public AudioClip stageBackGroundHole;
    public AudioClip stageBackInNPC;
    public AudioClip stageBackSushi;
    public AudioClip mineCameraShake;
    public AudioClip wizardCounterUp;
    public AudioClip wizardCounterDown;

    [Header("UI Effect")]
    public AudioClip UISelect;
    public AudioClip UIPop;
    public AudioClip UITick;
    public AudioClip UIStart;
    public AudioClip screenInUI;
    public AudioClip screenOutUI;
    public AudioClip flagOn;
    public AudioClip flagOff;
    public AudioClip recipeUIPopIn;
    public AudioClip recipeUIPopOut;

    [Header("MultiPlay")]
    public bool isSingle = true; //싱글 멀티 구분
    public bool alreadyPlayed = false;

    [Header("Game Play Sound Effect")]
    public AudioClip itemPickUp;
    public AudioClip itemputDown;
    public AudioClip knifeChop;
    public AudioClip fall;
    public AudioClip throwItem;
    public AudioClip ready;
    public AudioClip go;
    public AudioClip bin;
    public AudioClip right;
    public AudioClip no;
    public AudioClip cut;
    public AudioClip changeChef;
    public AudioClip recipeTimeOut;
    public AudioClip imCooked;
    public AudioClip dash;
    public AudioClip spawn;
    public AudioClip panSizzle;
    public AudioClip potSizzle;
    public AudioClip move;
    public AudioClip timesUp;
    public AudioClip beep;


    [Header("Result Sound Effect")]
    public AudioClip LevelVictorySound;
    public AudioClip RoundResult;
    public AudioClip RoundResult_Description;
    public AudioClip RoundResult_FinalScore;
    public AudioClip RoundResult_Star1;
    public AudioClip RoundResult_Star2;
    public AudioClip RoundResult_Star3;


    [Header("Van")]
    public AudioClip boing;
    public AudioClip van;
    public AudioClip vanBooster;
    public AudioClip vanShutter;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;  // The music mixer group
    public AudioMixerGroup effectGroup; // The effect mixer group
    //public AudioMixerGroup BgmChangeGroup

    AudioSource musicSource;            // Reference to the generated music Audio Source
    AudioSource effectSource;           // Reference to the generated effect Audio Source

    public void Load()
    {
        bgmAudioSource.volume = LoadData.Instance.optionData.saveBgmVolume;
        bgmChangeAudioSource.volume = LoadData.Instance.optionData.saveBgmVolume;
        stageAudioSource.volume = LoadData.Instance.optionData.saveBgmVolume;
        stageBackGroundAudioSource.volume = LoadData.Instance.optionData.saveBgmVolume * 0.2f;

        effectAudioSource.volume = LoadData.Instance.optionData.saveEffectVolume;
        stageEffectAudioSource.volume = LoadData.Instance.optionData.saveEffectVolume;
        vanAudioSource.volume = LoadData.Instance.optionData.saveEffectVolume * 0.2f;

        volumeBGM = LoadData.Instance.optionData.saveBgmVolume;
        settingBGM = LoadData.Instance.optionData.saveBgmVolume;
        volumeEffect = LoadData.Instance.optionData.saveEffectVolume;
        settingEffect = LoadData.Instance.optionData.saveEffectVolume;

        UIManager.Instance.SetSoundSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetSoundSquares(volumeEffect, effectSquares);
        UIManager.Instance.SetSoundSquares(volumeBGM, mobileBGMSquares);
        UIManager.Instance.SetSoundSquares(volumeEffect, mobileEffectSquares);
    }

    void Start()
    {
        SettingAudioVolume();

        FadeInVolumeAsync(bgmAudioSource, 8f, "Intro").Forget();

        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();

        musicSource.outputAudioMixerGroup = musicGroup;
        effectSource.outputAudioMixerGroup = effectGroup;
        //IntroBGM();
    }

    private void OnEnable()
    {
        EventManager<SoundEvents>.StartListening(SoundEvents.MineBgmPlay, MineBgm);
        EventManager<SoundEvents>.StartListening(SoundEvents.StageBgmFadeOut, StageBgmFadeOut);
    }

    #region settingAuido
    public void SettingAudioVolume()
    {
        bgmAudioSource.volume = 0.1f;
        bgmChangeAudioSource.volume = 0.1f;
        SetAllEffectVolume();
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
        stageAudioSource.volume = volumeBGM;

        UIManager.Instance.SetSoundSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetSoundSquares(volumeBGM, mobileBGMSquares);
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
        stageAudioSource.volume = volumeBGM;

        UIManager.Instance.SetSoundSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetSoundSquares(volumeBGM, mobileBGMSquares);
    }

    public void EffectSoundVolumeUp()
    {
        volumeEffect += 0.1f;
        if (volumeEffect >= 1)
        {
            volumeEffect = 1f;
        }
        SetAllEffectVolume();
        UIManager.Instance.SetSoundSquares(volumeEffect, effectSquares);
        UIManager.Instance.SetSoundSquares(volumeEffect, mobileEffectSquares);
    }

    public void EffectSoundVolumeDown()
    {
        volumeEffect -= 0.1f;
        if (volumeEffect <= 0)
        {
            volumeEffect = 0;
        }
        SetAllEffectVolume();
        UIManager.Instance.SetSoundSquares(volumeEffect, effectSquares);
        UIManager.Instance.SetSoundSquares(volumeEffect, mobileEffectSquares);
    }
    
    public void SettingSave()
    {
        settingBGM = volumeBGM;
        settingEffect = volumeEffect;
        bgmAudioSource.volume = volumeBGM;
        bgmChangeAudioSource.volume = volumeBGM;
        stageAudioSource.volume = volumeBGM;
        SetAllEffectVolume();

        UIManager.Instance.SetSoundSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetSoundSquares(volumeEffect, effectSquares);
        UIManager.Instance.SetSoundSquares(volumeBGM, mobileBGMSquares);
        UIManager.Instance.SetSoundSquares(volumeEffect, mobileEffectSquares);
    }

    public void SettingCancle()
    {
        volumeBGM = settingBGM;
        volumeEffect = settingEffect;
        bgmAudioSource.volume = volumeBGM;
        bgmChangeAudioSource.volume = volumeBGM;
        stageAudioSource.volume = volumeBGM;
        SetAllEffectVolume();

        UIManager.Instance.SetSoundSquares(volumeBGM, BGMSquares);
        UIManager.Instance.SetSoundSquares(volumeEffect, effectSquares);
        UIManager.Instance.SetSoundSquares(volumeBGM, mobileBGMSquares);
        UIManager.Instance.SetSoundSquares(volumeEffect, mobileEffectSquares);
    }

    private void SetAllEffectVolume()
    {
        effectAudioSource.volume = volumeEffect;
        stageBackGroundAudioSource.volume = volumeBGM * 0.2f;
        stageEffectAudioSource.volume = volumeEffect;
        vanAudioSource.volume = volumeEffect * 0.2f;
    }

    #endregion

    #region FadeInOut
    public void FadeInAudio(AudioSource audioSource, float waitTime, string bgmName)
    {
        FadeInVolumeAsync(audioSource, waitTime, bgmName).Forget();
    }

    public void FadeOutAudio(AudioSource audioSource, float waitTime)
    {
        FadeOutVolumeAsync(audioSource, waitTime).Forget();
    }

    async UniTask FadeInVolumeAsync(AudioSource audioSource, float waitTime, string bgmName)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        playBgm(audioSource, bgmName);

        if(audioSource == stageBackGroundAudioSource)
        {
            while (audioSource.volume < volumeBGM * 0.2f)
            {
                audioSource.volume += Time.deltaTime * 0.2f;
                await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime * 0.5f));
            }
        }
        else
        {
            while (audioSource.volume < volumeBGM)
            {
                audioSource.volume += Time.deltaTime * 0.2f;
                await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime * 0.5f));
            }
        }

   
    }

    async UniTask FadeOutVolumeAsync(AudioSource audioSource, float waitTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime * 0.2f;
            await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime * 0.5f));
        }
        audioSource.volume = 0;
        audioSource.Stop();
    }
    #endregion

    void playBgm(AudioSource audioSource , string bgmName)
    {
        switch (bgmName)
        {
            case "Intro":
                IntroBGM(audioSource);
                break;
            case "Battle":
                BattleBGM(audioSource);
                    break;
            case "WorldMap":
                WorldMap(audioSource);
                break;
            case "StageMap":
                StageMapBGM(audioSource);
                break;
            case "Wizard":
                WizardBGM(audioSource);
                break;
            case "Mine":
                MineBackGroundBGM(audioSource);
                //MineBGM(audioSource);
                break;
            case "Sushi":
                NeonBGM(audioSource);
                break;
        }
    }
    void StageBgmFadeOut()
    {
        FadeOutVolumeAsync(stageBackGroundAudioSource, 0).Forget();
        FadeOutVolumeAsync(stageAudioSource, 0).Forget();
    }


    void IntroBGM(AudioSource audioSource)
    {
        audioSource.clip = introBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    void BattleBGM(AudioSource audioSource)
    {
        audioSource.clip = battleBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    void WorldMap(AudioSource audioSource)
    {
        audioSource.clip = worldMapBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    void StageMapBGM(AudioSource audioSource)
    {
        audioSource.clip = stageBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    void WizardBGM(AudioSource audioSource)
    {
        audioSource.clip = wizardBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    void MineBackGroundBGM(AudioSource audioSource)
    {
        audioSource.clip = stageBackInNPC;
        audioSource.loop = true;
        audioSource.Play();
    }

    void MineBgm()
    {
        Debug.Log("minebgm");
        stageAudioSource.volume = volumeBGM;
        stageAudioSource.clip = mineBGM;
        stageAudioSource.loop = true;
        stageAudioSource.Play();
    }
    
    void NeonBGM(AudioSource audioSource)
    {
        audioSource.clip = sushiBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    
    void MineBGM(AudioSource audioSource)
    {
        audioSource.clip = mineBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void MineCameraShake()
    {
        stageEffectAudioSource.PlayOneShot(mineCameraShake);
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

    public void RecipeUIPopIn()
    {
        effectAudioSource.PlayOneShot(recipeUIPopIn);
    }

    public void RecipeUIPopOut()
    {
        effectAudioSource.PlayOneShot(recipeUIPopOut);
    }

    public void VanShutter()
    {
        vanAudioSource.PlayOneShot(vanShutter);
    }

    public async void ResultBgm()
    {
        bgmChangeAudioSource.volume = volumeBGM;
        bgmChangeAudioSource.PlayOneShot(LevelVictorySound);

        await Task.Delay((int)(5f));

        bgmChangeAudioSource.clip = RoundResult;
        bgmChangeAudioSource.loop = true;
        bgmChangeAudioSource.Play();
    }

    public void PlayEffect(string effect)
    {
        switch (effect)
        {
            case "itemPickUp":
                effectAudioSource.clip = itemPickUp;
                break;
            case "itemputDown":
                effectAudioSource.clip = itemputDown;
                break;
            case "knifeChop":
                effectAudioSource.clip = knifeChop;
                break;
            case "fall":
                effectAudioSource.clip = fall;
                break;
            case "dash":
                effectAudioSource.clip = dash;
                break;
            case "spawn":
                effectAudioSource.clip = spawn;
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
            case "changeChef":
                effectAudioSource.clip = changeChef;
                break;           
            case "recipeTimeOut":
                effectAudioSource.clip = recipeTimeOut;
                break; 
            case "imCooked":
                effectAudioSource.clip = imCooked;
                break;
            case "potSizzle":
                effectAudioSource.clip = potSizzle;
                break;
            case "panSizzle":
                effectAudioSource.clip = panSizzle;
                break;
            case "move":
                effectAudioSource.clip = move;
                break;
            case "timesUp":
                effectAudioSource.clip = timesUp;
                break;
            case "beep":
                effectAudioSource.clip = beep;
                break;
            case "boing":
                effectAudioSource.clip = boing;
                effectAudioSource.volume = volumeEffect;
                effectAudioSource.PlayOneShot(boing);
                break;
            case "RoundResults_Description":
                effectAudioSource.clip = RoundResult_Description;
                break;
            case "RoundResults_FinalScore":
                effectAudioSource.clip = RoundResult_FinalScore;
                break;
            case "RoundResult_Star1":
                effectAudioSource.clip = RoundResult_Star1;
                break;
            case "RoundResult_Star2":
                effectAudioSource.clip = RoundResult_Star2;
                break;
            case "RoundResult_Star3":
                effectAudioSource.clip = RoundResult_Star3;
                break;

        }
        effectAudioSource.volume = volumeEffect;
        effectAudioSource.PlayOneShot(effectAudioSource.clip);
    }
}