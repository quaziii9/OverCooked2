using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private GameObject[] BGMSquares; //음량 네모네모
    [SerializeField] private GameObject[] effectSquares;

    [Header("음량")]
    public float volumeBGM = 0.2f;
    public float volumeEffect = 1f;

    [Header("배경음악")]
    public AudioSource BgmAudioSource;

    [Header("효과음")]
    public AudioSource EffectAudioSource;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void upBgmSound()
    {
        volumeBGM += 0.1f;
        if (volumeBGM >= 1)
        {
            volumeBGM = 1f;
        }
        //BgmAudioSource.volume = volumeBGM;


        // UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
    }
    public void downBgmSound()
    {
        volumeBGM -= 0.1f;
        if (volumeBGM <= 0)
        {
            volumeBGM = 0;
        }
        //BgmAudioSource.volume = volumeBGM;
        // UIManager.Instance.SetBGMSquares(volumeBGM, BGMSquares);
    }

    public void upEffectSound()
    {
        volumeEffect += 0.1f;
        if (volumeEffect >= 1)
        {
            volumeEffect = 1f;
        }
        //EffectAudioSource.volume = volumeEffect;
        // UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }
    public void downEffectSound()
    {
        volumeEffect -= 0.1f;
        if (volumeEffect <= 0)
        {
            volumeEffect = 0;
        }
        //EffectAudioSource.volume = volumeEffect;
        // UIManager.Instance.SetEffectSquares(volumeEffect, effectSquares);
    }
}