using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Threading;

public class PanOnStove : MonoBehaviour
{
    [Header("UI")]
    public GameObject canvas; // UI 캔버스
    public Slider cookingBar; // 요리 진행 바
    public GameObject ingredientUI; // 재료 UI

    [Space(10)]
    public GameObject pfxFire; // 불 이펙트

    [Header("State")]
    public bool isOnStove; // 팬이 스토브 위에 있는지 여부
    public bool inSomething; // 팬에 재료가 있는지 여부
    public float cookingTime; // 요리 시간

    [Space(10)]
    public Material[] friedMaterials; // 요리된 재료의 재질 배열

    private CancellationTokenSource _cts; // 비동기 작업 취소 토큰
    private bool pause; // 요리 일시 정지 여부
    private bool stateIsCooked; // 재료가 요리되었는지 여부

    private void Update()
    {
        // 팬 위에 음식이 있을 때 불을 켜고, 없으면 끕니다.
        pfxFire.SetActive(isOnStove && inSomething);

        // 요리 중인 상태 업데이트
        if (isOnStove && inSomething && !stateIsCooked)
        {
            UpdateCookingBarPosition();
            UpdateCookingBarValue();
            UpdateIsIngredientState();
        }

        // 요리가 완료되었거나 재료가 없을 때 요리 진행 바 비활성화
        if (stateIsCooked || !inSomething)
        {
            cookingBar.gameObject.SetActive(false);
        }

        // 요리 시간이 1 이상일 때 요리된 재료로 변경
        if (cookingTime >= 1)
        {
            ChangeCookedMaterial();
        }
    }

    // 재료의 상태 업데이트
    private void UpdateIsIngredientState()
    {
        if (transform.childCount > 2)
        {
            stateIsCooked = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>().isCooked;
        }
    }

    // 요리 진행 바의 위치 업데이트
    private void UpdateCookingBarPosition()
    {
        cookingBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0)); // 적절한 위치 조정
    }

    // 요리 진행 바의 값 업데이트
    private void UpdateCookingBarValue()
    {
        cookingBar.value = cookingTime;
    }

    // 요리를 시작합니다.
    public async void StartCooking(UnityAction EndCallBack = null)
    {
        Debug.Log("cooking!");
        if (_cts == null)
        {
            Debug.Log("start cooking");
            cookingBar.gameObject.SetActive(true);
            ClearTime();

            _cts = new CancellationTokenSource();
            StartCookingAsync(EndCallBack, _cts.Token).Forget();
        }
        else if (pause)
        {
            pause = false; // 일시 정지를 해제합니다.
        }
    }

    // 비동기적으로 요리를 시작합니다.
    private async UniTask StartCookingAsync(UnityAction EndCallBack = null, CancellationToken cancellationToken = default)
    {
        if (!inSomething)
        {
            pfxFire.SetActive(false); // 재료가 없으면 불을 끕니다.
            return;
        }

        // 요리 시간을 증가시키며 요리 상태를 업데이트합니다.
        while (cookingTime <= 1)
        {
            if (!inSomething || cancellationToken.IsCancellationRequested)
            {
                pfxFire.SetActive(false);
                return;
            }

            while (pause)
            {
                await UniTask.Yield(cancellationToken); // 일시 정지된 동안 대기합니다.
            }

            await UniTask.Delay(450, cancellationToken: cancellationToken); // 요리 시간이 증가하는 간격
            cookingTime += 0.25f;
        }

        Debug.Log("Cooking End");
        pfxFire.SetActive(false); // 요리가 끝나면 불을 끕니다.
        EndCallBack?.Invoke();
        OffSlider();

        pause = false;
        cookingTime = 0;
        stateIsCooked = true; // 요리가 끝난 후 상태를 요리됨으로 설정
        _cts.Dispose(); // 취소 토큰 소스를 해제합니다.
        _cts = null;
    }

    // 요리 시간을 초기화합니다.
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

    // 요리 진행 바를 비활성화합니다.
    public void OffSlider()
    {
        cookingBar.value = 0f;
        cookingBar.gameObject.SetActive(false);
        UpdateIngredientState();
        InstantiateUI();
    }

    // 재료의 상태를 업데이트합니다.
    private void UpdateIngredientState()
    {
        if (transform.childCount < 3) return;

        Ingredient ingredient = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>();
        ingredient.isCooked = true;
        ingredient.ChangeMesh(ingredient.type);
    }

    // 재료 UI를 생성합니다.
    private void InstantiateUI()
    {
        if (transform.childCount < 3) return;

        Ingredient ingredient = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>();
        GameObject madeUI = Instantiate(ingredientUI, Vector3.zero, Quaternion.identity, canvas.transform);
        madeUI.transform.GetChild(0).gameObject.SetActive(true);
        Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
        image.sprite = IconManager.Instance.GetIcon(ingredient.type);
        madeUI.GetComponent<IngredientUI>().target = ingredient.transform;
    }

    // 요리된 재료의 재질을 변경합니다.
    private void ChangeCookedMaterial()
    {
        if (transform.childCount < 3) return;

        MeshFilter meshFilter = transform.GetChild(2).GetChild(0).GetComponent<MeshFilter>();
        MeshRenderer mr = transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>();
        string meshFileName = meshFilter.sharedMesh.name;

        mr.material = meshFileName switch
        {
            "m_ingredients_chicken_sliced_01_0" => friedMaterials[0],
            "m_ingredients_meat_sliced_01_0" => friedMaterials[1],
            _ => mr.material
        };
    }
}
