using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public bool goLeft = false; // 이동 방향을 왼쪽으로 설정하는 플래그
    public float Speed = 1000f; // 이동 속도
    public Slider timer; // 타이머 슬라이더 UI
    
    private Vector3 _pos; // 오브젝트의 위치를 저장할 변수
    private float _duration; // 색상 변화 시간(초 단위)
    private const float SMOOTHNESS = 0.1f; // 색상 변화의 부드러움 정도, 값이 작을수록 더 부드러움
    private readonly Color _start = new Color(0, 192 / 255f, 5 / 255f, 255 / 255f); // 시작 색상 (초록)
    private readonly Color _middle = new Color(243 / 255f, 239 / 255f, 0, 255 / 255f); // 중간 색상 (노랑)
    private readonly Color _end = new Color(215 / 255f, 11 / 255f, 0, 1f); // 종료 색상 (빨강)
    private Color _currentColor; // 현재 색상 상태

    private void Awake()
    {
        // 타이머 슬라이더를 초기화하고, 색상 시작점을 설정
        timer = transform.GetChild(2).GetChild(0).GetComponent<Slider>();
        _duration = timer.maxValue / 2 * 30;
        _currentColor = _start;
    }

    private void Update()
    {
        // goLeft가 true일 때 오브젝트를 왼쪽으로 이동
        if (!goLeft) return;
        
        _pos = transform.position;
        _pos.x -= Speed * Time.deltaTime;
        transform.position = _pos;
    }

    private void OnEnable()
    {
        // 오브젝트가 활성화될 때 초기 위치를 설정하고, 이동을 시작하며 색상 변화 및 타이머를 시작
        transform.localPosition = GameManager.Instance.poolPos;
        goLeft = true;
        timer.value = timer.maxValue;
        StartCoroutine(TimerStart());
        StartCoroutine(LerpColor1());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 2D 충돌 발생 시 이동을 정지
        goLeft = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // 2D 트리거 충돌 발생 시 이동을 정지
        goLeft = false;
    }

    private IEnumerator LerpColor1()
    {
        // 시작 색상에서 중간 색상으로 천천히 변화
        float progress = 0; // 색상 변화 진행도
        float increment = SMOOTHNESS / _duration; // 색상 변화 정도
        while (progress < 1)
        {
            _currentColor = Color.Lerp(_start, _middle, progress);
            progress += increment;
            yield return new WaitForSeconds(SMOOTHNESS);
            // UI의 색상을 현재 색상으로 업데이트
            transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = _currentColor;
        }
        // 중간 색상에서 종료 색상으로 변화하는 코루틴 시작
        StartCoroutine(LerpColor2());
    }

    private IEnumerator LerpColor2()
    {
        // 중간 색상에서 종료 색상으로 천천히 변화
        float progress = 0; // 색상 변화 진행도
        float increment = SMOOTHNESS / _duration; // 색상 변화 정도
        while (progress < 1)
        {
            _currentColor = Color.Lerp(_middle, _end, progress);
            progress += increment;
            yield return new WaitForSeconds(SMOOTHNESS);
            // UI의 색상을 현재 색상으로 업데이트
            transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().color = _currentColor;
        }
    }

    private IEnumerator TimerStart()
    {
        // 타이머 슬라이더의 값을 감소시키는 코루틴
        while (timer.value > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timer.value -= 1f;
        }
        // 타이머가 0이 되면 실패 처리 (메뉴 실패 메서드 호출)
        GameManager.Instance.MenuFail(gameObject);
    }
}
