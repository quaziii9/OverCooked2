using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using EventLibrary;
using EnumTypes;

public class PanOnStove : MonoBehaviour
{
    [Header("UI")]
    public GameObject canvas;
    public Slider cookingBar;
    public GameObject ingredientUI;

    [Space(10)]
    public GameObject pfxFire;

    [Header("State")]
    public bool isOnStove = false;
    public bool inSomething = false;
    public float cookingTime;

    //private Coroutine _coTimer;
    private CancellationTokenSource _cts;
    private bool pause = false;
    private bool stateIsCooked = false;

    public Sprite[] icons;

    private void Start()
    {
        EventManager<GameEvent>.StartListening(GameEvent.FireOff, pfxFireOff);
    }

    private void Update()
    {
        if (isOnStove && inSomething && !stateIsCooked)
        {
            UpdateCookingBarPosition();
            UpdateCookingBarValue();
            UpdateisIngredientState();
        }

        if (stateIsCooked)
            cookingBar.gameObject.SetActive(false);
    }

    private void pfxFireOff()
    {
        //if (issom))
        //{
            
        //}
        pfxFire.SetActive(false);      
    }

    private void UpdateisIngredientState()
    {
        if (transform.GetChild(2).gameObject != null)
        {
            stateIsCooked = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>().isCooked;
        }
    }

    private void UpdateCookingBarPosition()
    {
        cookingBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0)); // 적절한 위치 조정
    }

    private void UpdateCookingBarValue()
    {
        cookingBar.value = cookingTime;
    }

    public async void StartCooking(UnityAction EndCallBack = null)
    {
        if (_cts == null)
        {
            Debug.Log("start cooking");
            cookingBar.gameObject.SetActive(true);
            ClearTime();

            _cts = new CancellationTokenSource();
            StartCookingAsync(EndCallBack, _cts.Token).Forget();
            //_coTimer = StartCoroutine(CoStartCooking(EndCallBack));
        }
        else if (pause)
        {
            pause = false;
        }
    }

    private async UniTask StartCookingAsync(UnityAction EndCallBack = null,  CancellationToken cancellationToken = default)
    {
        pfxFire.SetActive(true);

        if (inSomething == false)
        {
            pfxFire.SetActive(false);
            return;
        }

        while (cookingTime <= 1)
        {
            if (inSomething == false)
            {
                pfxFire.SetActive(false);
                return;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            while (pause)
            {
                await UniTask.Yield(cancellationToken);
            }
            await UniTask.Delay(450, cancellationToken: cancellationToken);
            cookingTime += 0.25f;
        }
        Debug.Log("Cooking End");
        pfxFire.SetActive(false);
        EndCallBack?.Invoke();
        OffSlider();

        pause = false;
        cookingTime = 0;
        _cts.Dispose();
        _cts = null;
    }

    private void ClearTime()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
        pause = false;
    }

    public void OffSlider()
    {
        cookingBar.value = 0f;
        cookingBar.gameObject.SetActive(false);
        UpdateIngredientState();
        InstantiateUI();
    }

    private void UpdateIngredientState()
    {
        if (transform.childCount < 3)
            return;
        Ingredient Ingredient = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>();
        Ingredient.isCooked = true;

        //여기선 이제 쿡되야함
        //if (Ingredient.type == Ingredient.IngredientType.Meat || Ingredient.type == Ingredient.IngredientType.Chicken)
        //    Ingredient.isCooked = false;

        Ingredient.ChangeMesh(Ingredient.type);
    }

    public void InstantiateUI()
    {
        if (transform.childCount < 3)
            return;

        Ingredient Ingredient = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>();
        GameObject madeUI = Instantiate(ingredientUI, Vector3.zero, Quaternion.identity, canvas.transform);
        madeUI.transform.GetChild(0).gameObject.SetActive(true);
        Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
        image.sprite = GetIcon(Ingredient.type);
        madeUI.GetComponent<IngredientUI>().target = Ingredient.transform;
    }

    private Sprite GetIcon(Ingredient.IngredientType ingredientType)
    {
        switch (ingredientType)
        {
            case Ingredient.IngredientType.Fish:
                return icons[0];

            case Ingredient.IngredientType.Shrimp:
                return icons[1];

            case Ingredient.IngredientType.Tomato:
                return icons[2];

            case Ingredient.IngredientType.Lettuce:
                return icons[3];

            case Ingredient.IngredientType.Cucumber:
                return icons[4];

            case Ingredient.IngredientType.Potato:
                return icons[5];

            case Ingredient.IngredientType.Chicken:
                return icons[6];

            case Ingredient.IngredientType.SeaWeed:
                return icons[7];

            case Ingredient.IngredientType.Tortilla:
                return icons[8];

            case Ingredient.IngredientType.Rice:
                return icons[9];

            case Ingredient.IngredientType.Pepperoni:
                return icons[10];

            case Ingredient.IngredientType.Meat:
                return icons[11];

            case Ingredient.IngredientType.Dough:
                return icons[12];

            case Ingredient.IngredientType.Cheese:
                return icons[13];

            case Ingredient.IngredientType.SushiRice:
                return icons[9];

            case Ingredient.IngredientType.SushiFish:
                return icons[0];

            case Ingredient.IngredientType.SushiCucumber:
                return icons[4];

            case Ingredient.IngredientType.PizzaTomato:
                return icons[2];

            default:
                // 기본적으로 아무것도 하지 않음
                throw new ArgumentOutOfRangeException();
        }
    }
}
