using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
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
        SetBGMSquares();
    }
    public void downBgmSound()
    {
        volumeBGM -= 0.1f;
        if (volumeBGM <= 0)
        {
            volumeBGM = 0;
        }
        //BgmAudioSource.volume = volumeBGM;
        SetBGMSquares();
    }

    private void SetBGMSquares()
    {
        int j = 0;
        for (float i = 0; i < 1; i += 0.1f)
        {
            if (i < volumeBGM) //켜있는거
            {
                BGMSquares[j].transform.GetChild(0).gameObject.SetActive(false);
                BGMSquares[j].transform.GetChild(1).gameObject.SetActive(true);
            }
            else //꺼있는거
            {
                BGMSquares[j].transform.GetChild(0).gameObject.SetActive(true);
                BGMSquares[j].transform.GetChild(1).gameObject.SetActive(false);
            }
            j++;
        }
    }

    public void upEffectSound()
    {
        volumeEffect += 0.1f;
        if (volumeEffect >= 1)
        {
            volumeEffect = 1f;
        }
        //EffectAudioSource.volume = volumeEffect;
        SetEffectSquares();
    }
    public void downEffectSound()
    {
        volumeEffect -= 0.1f;
        if (volumeEffect <= 0)
        {
            volumeEffect = 0;
        }
        //EffectAudioSource.volume = volumeEffect;
        SetEffectSquares();
    }

    private void SetEffectSquares()
    {
        int j = 0;
        for (float i = 0; i < 1; i += 0.1f)
        {
            if (i < volumeEffect) 
            {
                effectSquares[j].transform.GetChild(0).gameObject.SetActive(false);
                effectSquares[j].transform.GetChild(1).gameObject.SetActive(true);
            }
            else 
            {
                effectSquares[j].transform.GetChild(0).gameObject.SetActive(true);
                effectSquares[j].transform.GetChild(1).gameObject.SetActive(false);
            }
            j++;
        }
    }
}
