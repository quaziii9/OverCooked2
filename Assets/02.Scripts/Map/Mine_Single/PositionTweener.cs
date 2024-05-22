using UnityEngine;
using DG.Tweening;

public class PositionTweener : MonoBehaviour
{
    void Start()
    {
        // 시퀀스 생성
        Sequence sequence = DOTween.Sequence();

        // 15초 동안 대기
        sequence.AppendInterval(15f);

        // 첫 번째 트윈 (-0.048에서 -0.108로 이동)
        sequence.Append(transform.DOLocalMoveX(-0.108f, 5f).SetEase(Ease.InOutQuad));

        // 15초 동안 대기
        sequence.AppendInterval(15f);

        // 두 번째 트윈 (-0.108에서 -0.048로 이동)
        sequence.Append(transform.DOLocalMoveX(-0.048f, 5f).SetEase(Ease.InOutQuad));

        // 시퀀스를 무한 반복
        sequence.SetLoops(-1, LoopType.Restart);
    }
}
