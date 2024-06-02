using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using static Ingredient;

public class CuttingBoard : MonoBehaviour
{
    private ObjectHighlight parentObject;

    [Header("UI")]
    [SerializeField] private GameObject Canvas;
    public Slider cookingBar;
    [SerializeField] private GameObject IngredientUI;

    [Space(10)]
    [SerializeField] Vector3 pos;
    public Coroutine _CoTimer;
    public bool Pause = false;
    public float CuttingTime;

    [SerializeField] private Sprite[] Icons;

    private PlayerInteractController player1Controller;
    private Player2InteractController player2Controller;

    private void Start()
    {
        parentObject = transform.parent.GetComponent<ObjectHighlight>();
        player1Controller = FindObjectOfType<PlayerInteractController>();
        player2Controller = FindObjectOfType<Player2InteractController>();
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
        cookingBar.transform.position = Camera.main.WorldToScreenPoint(parentObject.transform.position + pos);
    }

    private void UpdateCookingBarValue()
    {
        // 칼질 진행 상태를 나타내는 UI 바의 값을 업데이트
        cookingBar.value = CuttingTime;
    }

    private void ToggleKnife()
    {
        // 객체가 활성화된 상태에 따라 칼 UI를 토글
        transform.GetChild(0).GetChild(0).gameObject.SetActive(!parentObject.onSomething);
    }

    public void StartCutting1(UnityAction EndCallBack = null)
    {
        // 플레이어 1의 칼질을 시작
        StartCutting(player1Controller, EndCallBack);
    }

    public void StartCutting2(UnityAction EndCallBack = null)
    {
        // 플레이어 2의 칼질을 시작
        StartCutting(player2Controller, EndCallBack);
    }

    private void StartCutting(object playerController, UnityAction EndCallBack = null)
    {
        if (parentObject.onSomething)
        {
            SoundManager.Instance.PlayEffect("cut");
            cookingBar.gameObject.SetActive(true);
            ClearTime();
            _CoTimer = StartCoroutine(CoStartCutting(EndCallBack));
            if (playerController is PlayerInteractController player1)
            {
                player1.anim.SetTrigger("startCut");
                player1.anim.SetBool("canCut", true);
            }
            else if (playerController is Player2InteractController player2)
            {
                player2.anim.SetTrigger("startCut");
                player2.anim.SetBool("canCut", true);
            }
        }
    }

    public void PauseSlider(bool pause)
    {
        // 칼질 진행을 일시 중지하거나 재개
        Pause = pause;
    }

    private IEnumerator CoStartCutting(UnityAction EndCallBack = null)
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
        EndCallBack?.Invoke();
        OffSlider();
        _CoTimer = null;
        Pause = false;
        CuttingTime = 0;
    }

    private void ClearTime()
    {
        // 칼질 타이머를 초기화
        if (_CoTimer != null)
        {
            StopCoroutine(_CoTimer);
            _CoTimer = null;
        }
        Pause = false;
    }

    public void OffSlider()
    {
        // 칼질 진행 UI를 비활성화
        cookingBar.value = 0f;
        cookingBar.gameObject.SetActive(false);
        UpdateCanCutState(player1Controller);
        UpdateCanCutState(player2Controller);
        UpdateIngredientState();
        InstantiateUI();
    }

    private void UpdateCanCutState(object playerController)
    {
        // 플레이어의 칼질 가능 상태를 업데이트
        if (playerController is PlayerInteractController player1 && player1.interactObject == transform.parent.gameObject)
        {
            player1.anim.SetBool("canCut", false);
        }
        else if (playerController is Player2InteractController player2 && player2.interactObject == transform.parent.gameObject)
        {
            player2.anim.SetBool("canCut", false);
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

    public void OffAnim1()
    {
        
    }

    public void InstantiateUI()
    {
        // 재료 UI를 생성
        Ingredient ingredient = transform.parent.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>();
        GameObject madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);

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
            case IngredientType.Fish: return Icons[0];
            case IngredientType.Shrimp: return Icons[1];
            case IngredientType.Tomato: return Icons[2];
            case IngredientType.Lettuce: return Icons[3];
            case IngredientType.Cucumber: return Icons[4];
            case IngredientType.Potato: return Icons[5];
            case IngredientType.Chicken: return Icons[6];
            case IngredientType.SeaWeed: return Icons[7];
            case IngredientType.Tortilla: return Icons[8];
            case IngredientType.Rice: return Icons[9];
            case IngredientType.Pepperoni: return Icons[10];
            case IngredientType.Meat: return Icons[11];
            case IngredientType.Dough: return Icons[12];
            case IngredientType.Cheese: return Icons[13];
            case IngredientType.SushiRice: return Icons[9];
            case IngredientType.SushiFish: return Icons[0];
            case IngredientType.SushiCucumber: return Icons[4];
            case IngredientType.PizzaTomato: return Icons[2];
            default: throw new ArgumentOutOfRangeException();
        }
    }
}
