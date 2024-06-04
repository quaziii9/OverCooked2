using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static Ingredient;

public class CuttingBoard : MonoBehaviour
{
    private ObjectHighlight _parentObject;

    [FormerlySerializedAs("Canvas")]
    [Header("UI")]
    [SerializeField] private GameObject canvas;
    public Slider cookingBar;
    [FormerlySerializedAs("IngredientUI")] [SerializeField] private GameObject ingredientUI;

    [Space(10)]
    [SerializeField] Vector3 pos;
    public Coroutine CoTimer;
    [field: FormerlySerializedAs("Pause")] public bool Pause { get; set; }

    [field: FormerlySerializedAs("CuttingTime")]
    public float CuttingTime { get; set; }

    [FormerlySerializedAs("Icons")] [SerializeField] private Sprite[] icons;

    private PlayerInteractController _player1Controller;
    private Player2InteractController _player2Controller;
    private static readonly int StartCut = Animator.StringToHash("startCut");
    private static readonly int CanCut = Animator.StringToHash("canCut");
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _parentObject = transform.parent.GetComponent<ObjectHighlight>();
        _player1Controller = FindObjectOfType<PlayerInteractController>();
        _player2Controller = FindObjectOfType<Player2InteractController>();
    }

    private void Update()
    {
        UpdateCookingBarPosition();
        UpdateCookingBarValue();
        ToggleKnife();
    }

    private void UpdateCookingBarPosition()
    {
        // 칼을 사용하는 UI 바의 위치를 업데이트
        if (_camera)
            cookingBar.transform.position = _camera.WorldToScreenPoint(_parentObject.transform.position + pos);
    }

    private void UpdateCookingBarValue()
    {
        // 칼질 진행 상태를 나타내는 UI 바의 값을 업데이트
        cookingBar.value = CuttingTime;
    }

    private void ToggleKnife()
    {
        // 객체가 활성화된 상태에 따라 칼 UI를 토글
        transform.GetChild(0).GetChild(0).gameObject.SetActive(!_parentObject.onSomething);
    }

    public void StartCutting1(UnityAction endCallBack = null)
    {
        // 플레이어 1의 칼질을 시작
        StartCutting(_player1Controller, endCallBack);
    }

    public void StartCutting2(UnityAction endCallBack = null)
    {
        // 플레이어 2의 칼질을 시작
        StartCutting(_player2Controller, endCallBack);
    }

    private void StartCutting(object playerController, UnityAction endCallBack = null)
    {
        if (!_parentObject.onSomething) return;
        
        SoundManager.Instance.PlayEffect("cut");
        cookingBar.gameObject.SetActive(true);
        ClearTime();
        CoTimer = StartCoroutine(CoStartCutting(endCallBack));
        
        switch (playerController)
        {
            case PlayerInteractController player1:
                player1.Animator.SetTrigger(StartCut);
                player1.Animator.SetBool(CanCut, true);
                break;
            case Player2InteractController player2:
                player2.Animator.SetTrigger(StartCut);
                player2.Animator.SetBool(CanCut, true);
                break;
        }
    }

    public void PauseSlider(bool pause)
    {
        // 칼질 진행을 일시 중지하거나 재개
        this.Pause = pause;
    }

    private IEnumerator CoStartCutting(UnityAction endCallBack = null)
    {
        // 칼질을 일정 시간 동안 진행
        while (CuttingTime <= 1)
        {
            while (Pause)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.45f);
            CuttingTime += 0.25f;
        }
        endCallBack?.Invoke();
        OffSlider();
        CoTimer = null;
        Pause = false;
        CuttingTime = 0;
    }

    private void ClearTime()
    {
        // 칼질 타이머를 초기화
        if (CoTimer != null)
        {
            StopCoroutine(CoTimer);
            CoTimer = null;
        }
        Pause = false;
    }

    private void OffSlider()
    {
        // 칼질 진행 UI를 비활성화
        cookingBar.value = 0f;
        cookingBar.gameObject.SetActive(false);
        UpdateCanCutState(_player1Controller);
        UpdateCanCutState(_player2Controller);
        UpdateIngredientState();
        InstantiateUI();
    }

    private void UpdateCanCutState(object playerController)
    {
        // 플레이어의 칼질 가능 상태를 업데이트
        if (playerController is PlayerInteractController player1 && player1.interactObject == transform.parent.gameObject)
        {
            player1.Animator.SetBool("canCut", false);
        }
        else if (playerController is Player2InteractController player2 && player2.interactObject == transform.parent.gameObject)
        {
            player2.Animator.SetBool("canCut", false);
        }
    }

    private void UpdateIngredientState()
    {
        // 재료의 상태를 업데이트
        Ingredient ingredient = transform.parent.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>();
        ingredient.isCooked = true;

        if (ingredient.type == IngredientType.Meat || ingredient.type == IngredientType.Chicken)
            ingredient.isCooked = false;

        ingredient.ChangeMesh(ingredient.type);
    }

    private void InstantiateUI()
    {
        // 재료 UI를 생성
        Ingredient ingredient = transform.parent.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>();
        GameObject madeUI = Instantiate(ingredientUI, Vector3.zero, Quaternion.identity, canvas.transform);

        madeUI.transform.GetChild(0).gameObject.SetActive(true);
        Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
        image.sprite = GetIcon(ingredient.type);
        madeUI.GetComponent<IngredientUI>().target = ingredient.transform;
    }

    private Sprite GetIcon(IngredientType ingredientType)
    {
        // 재료 유형에 따른 아이콘을 반환
        switch (ingredientType)
        {
            case IngredientType.Fish: return icons[0];
            case IngredientType.Shrimp: return icons[1];
            case IngredientType.Tomato: return icons[2];
            case IngredientType.Lettuce: return icons[3];
            case IngredientType.Cucumber: return icons[4];
            case IngredientType.Potato: return icons[5];
            case IngredientType.Chicken: return icons[6];
            case IngredientType.SeaWeed: return icons[7];
            case IngredientType.Tortilla: return icons[8];
            case IngredientType.Rice: return icons[9];
            case IngredientType.Pepperoni: return icons[10];
            case IngredientType.Meat: return icons[11];
            case IngredientType.Dough: return icons[12];
            case IngredientType.Cheese: return icons[13];
            case IngredientType.SushiRice: return icons[9];
            case IngredientType.SushiFish: return icons[0];
            case IngredientType.SushiCucumber: return icons[4];
            case IngredientType.PizzaTomato: return icons[2];
            default: throw new ArgumentOutOfRangeException();
        }
    }
}
