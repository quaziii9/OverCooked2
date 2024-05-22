using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // 흔들림 지속 시간
    public float shakeStrength = 0.3f; // 흔들림 강도
    public float shakeInterval = 15f; // 흔들림 주기
    public int shakeVibrato = 10; // 흔들림 진동 횟수

    private Camera mainCamera;
    private Vector3 originalPosition;

    void Start()
    {
        mainCamera = Camera.main;
        originalPosition = mainCamera.transform.localPosition;
        StartCoroutine(ShakeLoop());
    }

    IEnumerator ShakeLoop()
    {
        // 처음 15초 동안 대기
        yield return new WaitForSeconds(shakeInterval);

        while (true)
        {
            ShakeCamera();
            yield return new WaitForSeconds(shakeInterval);
        }
    }

    void ShakeCamera()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.DOKill(); // 이전 트윈이 있으면 중지
            mainCamera.transform.localPosition = originalPosition; // 원래 위치로 복귀
            mainCamera.transform.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, 0), shakeVibrato, 0, false, true).OnComplete(() =>
            {
                mainCamera.transform.localPosition = originalPosition; // 흔들림이 끝나면 원래 위치로 복귀
            });
        }
    }
}
