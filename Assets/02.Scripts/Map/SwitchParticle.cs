using System.Collections;
using UnityEngine;

public class SwitchParticle : MonoBehaviour
{
    public float speed = 50f;
    public float totalTime = 0.4f;
    public ParticleSystem spark;
    public ParticleSystem line;
    public ParticleSystem switchBurst;

    public void SwitchPlayer(Transform start, Transform end)
    {
        StartCoroutine(MoveTowardsTarget(start, end));
    }

    private IEnumerator MoveTowardsTarget(Transform start, Transform end)
    {
        float elapsedTime = 0f;
        var sparkMain = spark.main;
        var lineMain = line.main;

        // 파티클 루프 설정 및 재생
        sparkMain.loop = true;
        lineMain.loop = true;
        spark.Play();
        line.Play();

        // 목표 지점으로 이동
        for (float t = 0; t < totalTime; t += Time.deltaTime)
        {
            // 보간값 계산 및 위치 설정
            float progress = t / totalTime;
            transform.position = Vector3.Lerp(start.position, end.position, progress);
            yield return null; // 다음 프레임까지 대기
        }
        
        // 이동 완료 후 최종 위치 설정
        transform.position = end.position;
        SwitchEnd();
    }

    private void SwitchEnd()
    {
        var sparkMain = spark.main;
        var lineMain = line.main;

        // 파티클 루프 중지 및 정지
        sparkMain.loop = false;
        lineMain.loop = false;
        spark.Stop();
        line.Stop();

        // switchBurst 파티클 재생
        switchBurst.Play();

        // 2초 후 객체 파괴
        Invoke(nameof(DestroyObject), 2f);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}