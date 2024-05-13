using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CuttingBoard : MonoBehaviour
{
    private ObjectHighlight parentObject;  // 참조: 이 객체의 부모 컴포넌트 중 ObjectHighlight
    public Slider cookingBar;  // 요리 진행 상태를 보여주는 슬라이더 UI
    [SerializeField] private Vector3 pos;  // 슬라이더 위치 조정 값
    public Coroutine _CoTimer;  // 커팅 작업을 위한 코루틴 참조
    public bool Pause = false;  // 작업 일시 정지 여부
    public float CuttingTime;  // 현재까지 진행된 커팅 시간

    [SerializeField] private GameObject Canvas;  // UI를 표시할 캔버스
    [SerializeField] private GameObject IngredientUI;  // 재료에 대한 UI 프리팹
    [SerializeField] private Sprite[] Icons;  // 재료 종류별 아이콘 배열

    private void Start()
    {
        // 컴포넌트 초기화
        parentObject = transform.parent.GetComponent<ObjectHighlight>();
    }

    private void Update()
    {
        // 매 프레임마다 슬라이더의 위치를 카메라 상의 오브젝트 위치에 맞춰 조정
        cookingBar.transform.position = Camera.main.WorldToScreenPoint(parentObject.transform.position + pos);
        cookingBar.value = CuttingTime;  // 슬라이더 값 갱신

        // 칼의 시각적 활성화 상태 설정
        bool isCuttingEnabled = !parentObject.onSomething;
        SetKnifeVisibility(isCuttingEnabled);
    }

    // 칼의 시각적 활성화/비활성화 처리
    private void SetKnifeVisibility(bool isVisible)
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(isVisible);
    }

    // 커팅 시작 메서드
    public void StartCutting(UnityAction endCallBack = null)
    {
        if (parentObject.onSomething)
        {
            cookingBar.gameObject.SetActive(true);
            ResetTimer();  // 타이머 초기화
            _CoTimer = StartCoroutine(CoStartCutting(endCallBack));  // 커팅 코루틴 시작
        }
    }

    // 커팅 코루틴
    private IEnumerator CoStartCutting(UnityAction endCallBack = null)
    {
        while (CuttingTime <= 1)
        {
            if (Pause) yield return null;  // 일시 정지 상태일 경우 대기

            yield return new WaitForSeconds(0.45f);  // 다음 커팅까지 대기
            CuttingTime += 0.25f;  // 커팅 시간 증가
        }

        endCallBack?.Invoke();  // 콜백 함수 호출
        ResetAndHideSlider();  // 슬라이더 및 타이머 리셋
    }

    // 타이머 리셋 및 슬라이더 숨김
    private void ResetTimer()
    {
        if (_CoTimer != null)
        {
            StopCoroutine(_CoTimer);  // 진행 중인 코루틴 중지
        }

        _CoTimer = null;
        Pause = false;
    }

    // 슬라이더와 타이머 리셋 후 요리 완성 처리
    private void ResetAndHideSlider()
    {
        cookingBar.value = 0;
        cookingBar.gameObject.SetActive(false);
        ResetTimer();  // 타이머 초기화

        HandleCookingCompletion();  // 요리 완성 처리
    }

    // 요리 완성 시 호출되어 재료의 상태 변경 및 UI 생성
    private void HandleCookingCompletion()
    {
        Ingredient ingredient = transform.parent.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>();
        ingredient.isCooked = true;  // 요리 상태를 'cooked'로 변경
        ingredient.ChangeMesh(ingredient.type);  // 메쉬 변경
        InstantiateUI(ingredient);  // UI 생성
    }

    // 재료에 맞는 UI 생성
    private void InstantiateUI(Ingredient ingredient)
    {
        GameObject madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
        Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
        int iconIndex = (int)ingredient.type;  // 재료 타입에 따른 아이콘 인덱스
        image.sprite = Icons[iconIndex];  // 아이콘 설정
        madeUI.GetComponent<IngredientUI>().Target = ingredient.transform;  // 타겟 설정
        madeUI.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void PauseSlider(bool pause)
    {
        Pause = pause;
    }
}
