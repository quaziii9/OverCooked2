using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteractController : MonoBehaviour
{
    // 애니메이션
    public Animator Animator;

    // 상호작용 할 수 있는 오브젝트
    public GameObject interactObject;
    public ObjectHighlight objectHighlight;
    public GameObject nextInteractObject;

    // 변경되는 Bool값
    public bool IsHolding { get; set; }

    // 재료를 제외한 모든 오브젝트 활성화 확인값
    public bool CanActive { get; set; }

    [Header("Throw")] [SerializeField] private GameObject throwArrow;
    [SerializeField] private Vector3 throwPower;

    [Header("Grab Object Control")] [SerializeField]
    private GameObject idleR;

    [SerializeField] private GameObject idleL;
    [SerializeField] private GameObject grabR;
    [SerializeField] private GameObject grabL;
    [SerializeField] private GameObject knife;

    [Header("Player Input System")] [SerializeField]
    private GameObject playerInputSystem;

    private PlayerMasterController2 _masterController;
    private PlayerMoveController _moveController;
    private GameManager _gameManager;

    [Header("Mobile Button")] public Button pickupButton; // 모바일 줍기/놓기 버튼
    public Button cookButton; // 모바일 요리/던지기 버튼

    private Vector3 _placeTransform = Vector3.zero;

    private void Awake()
    {
        _masterController = playerInputSystem.GetComponent<PlayerMasterController2>();
        _moveController = gameObject.GetComponent<PlayerMoveController>();
        _gameManager = FindObjectOfType<GameManager>();

        if (pickupButton != null)
        {
            pickupButton.onClick.AddListener(MobilePickupOrPlace); // 버튼 클릭 이벤트에 MobileCookOrThrow 메서드 연결
        }

        if (cookButton != null)
        {
            cookButton.onClick.AddListener(MobileCookOrThrow); // 버튼 클릭 이벤트에 MobileCookOrThrow 메서드 연결

            EventTrigger trigger = cookButton.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            pointerDownEntry.callback.AddListener((data) => { OnButtonPress(); });
            trigger.triggers.Add(pointerDownEntry);

            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
            pointerUpEntry.callback.AddListener((data) => { OnButtonRelease(); });
            trigger.triggers.Add(pointerUpEntry);
        }
    }

    private void OnButtonPress()
    {
        _moveController.moveSpeed = 0;
        throwArrow.SetActive(true);
    }

    private void OnButtonRelease()
    {
        _moveController.moveSpeed = 15;
        throwArrow.SetActive(false);

        // 기존 코드 실행
        if (CheckInteractObject())
        {
            if (ShouldStartCutting())
                StartCuttingProcess();
            else
                SoundManager.Instance.PlayEffect("no");
        }
        else
        {
            if (IsHolding && CanThrowIngredient())
            {
                ThrowIngredient();
            }
        }
    }

    private void Update()
    {
        SetHand();
    }

    #region OnSwitch

    public void OnSwitch(InputValue inputValue)
    {
        playerInputSystem.GetComponent<PlayerMasterController2>().SwitchPlayerComponent();
        SoundManager.Instance.PlayEffect("changeChef");
    }

    #endregion

    #region OnCookOrThrow

    public void OnCookOrThrow(InputValue inputValue)
    {
        bool isPressed = inputValue.isPressed;
        CookOrThrow(isPressed);
    }

    private void MobileCookOrThrow()
    {
        CookOrThrow(true); // 모바일에서는 터치가 눌린 것으로 처리
    }

    private void CookOrThrow(bool isPressed)
    {
        if (CheckInteractObject())
        {
            if (ShouldStartCutting())
            {
                StartCuttingProcess();
            }
        }
        else
        {
            if (IsHolding && CanThrowIngredient())
            {
                if (isPressed)
                {
                    _moveController.moveSpeed = 0; // 버튼이 눌렸을 때의 동작 수행
                    throwArrow.SetActive(true);
                }
                else // 버튼이 떼어졌을 때만 동작 수행
                {
                    ThrowIngredient();
                    _moveController.moveSpeed = 15; // 버튼이 떼어졌을 때의 동작 수행
                    throwArrow.SetActive(false);
                }
            }
        }
    }

    private bool CheckInteractObject()
    {
        if (interactObject != null)
        {
            if (interactObject.GetComponent<ObjectHighlight>().objectType == ObjectHighlight.ObjectType.Ingredient)
                return false;
            else
                return true;
        }

        return false;
    }

    bool ShouldStartCutting()
    {
        return objectHighlight.objectType == ObjectHighlight.ObjectType.Board &&
               interactObject.transform.parent.childCount > 2 &&
               !interactObject.transform.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>()
                   .isCooked &&
               !IsHolding &&
               interactObject.transform.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>().type !=
               Ingredient.IngredientType.Rice &&
               interactObject.transform.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>().type !=
               Ingredient.IngredientType.SeaWeed &&
               interactObject.transform.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>().type !=
               Ingredient.IngredientType.Tortilla;
    }

    void StartCuttingProcess()
    {
        var cuttingBoard = interactObject.transform.GetChild(0).GetComponent<CuttingBoard>();
        var ingredient = cuttingBoard.transform.parent.parent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Ingredient>();
        
        if (cuttingBoard.CoTimer == null && ingredient.currentState != Ingredient.IngredientState.Chopped) // 한번도 실행 안된거면 시작 가능
        {
            GameObject ingredientObj = interactObject.transform.parent.GetChild(2).GetChild(0).gameObject;
            MeshFilter meshFilter = ingredientObj.transform.GetComponent<MeshFilter>();
            string meshFileName = meshFilter.sharedMesh.name;
            if (meshFileName.Equals("m_ingredients_chicken_sliced_01_0") ||
                meshFileName.Equals("m_ingredients_meat_sliced_01_0"))
                
            cuttingBoard.Pause = false;
            cuttingBoard.CuttingTime = 0;
            cuttingBoard.StartCutting1();
            Animator.SetTrigger("startCut");
        }
        else if (cuttingBoard.Pause) // 실행되다 만거라면
        {
            Animator.SetTrigger("startCut");
            cuttingBoard.PauseSlider(false);
        }
    }

    bool CanThrowIngredient()
    {
        return transform.GetChild(1).GetComponent<Ingredient>() == null;
    }

    void ThrowIngredient()
    {
        PlayThrowSound();
        SetThrowAnimation();
        UpdateHoldingStatus(false);
        AdjustIngredientPosition();
        ApplyThrowForce();
        EnableIngredientCollision();
        ReleaseIngredient();
    }

    void PlayThrowSound()
    {
        SoundManager.Instance.PlayEffect("throwItem");
    }

    void SetThrowAnimation()
    {
        Animator.SetTrigger("throw");
    }

    void UpdateHoldingStatus(bool status)
    {
        IsHolding = status;
        Animator.SetBool("isHolding", IsHolding);
    }

    void AdjustIngredientPosition()
    {
        Transform ingredient = GetIngredientTransform();
        ingredient.localPosition += new Vector3(0, 0.3f, 0);
    }

    Transform GetIngredientTransform()
    {
        return transform.GetChild(1).GetChild(0);
    }

    void ApplyThrowForce()
    {
        Transform ingredient = GetIngredientTransform();
        Rigidbody ingreRigid = ingredient.GetComponent<Rigidbody>();
        Vector3 dir = transform.TransformDirection(throwPower);
        ingreRigid.AddForce(dir, ForceMode.Impulse);
    }

    void EnableIngredientCollision()
    {
        Transform ingredient = GetIngredientTransform();
        MeshCollider ingreCollider = ingredient.GetComponent<MeshCollider>();
        ingreCollider.isTrigger = false;
        Rigidbody ingreRigid = ingredient.GetComponent<Rigidbody>();
        // ingreRigid.constraints = RigidbodyConstraints.None;
        ingreRigid.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    void ReleaseIngredient()
    {
        transform.GetChild(1).SetParent(transform.parent);
    }

    #endregion

    #region OnPickupOrPlace

    public void OnPickupOrPlace(InputValue inputValue)
    {
        ProcessInteraction();
        //SetHand();
    }

    public void MobilePickupOrPlace()
    {
        ProcessInteraction();
        //SetHand();
    }

    private void ProcessInteraction()
    {
        if (interactObject == null && !IsHolding) return;

        if (IsHolding && objectHighlight == null)
        {
            // 뭔가 들고있고 앞에 상호작용 객체가 없을때
            HoldingItemDropObject();
            return;
        }

        switch (objectHighlight.objectType)
        {
            case ObjectHighlight.ObjectType.CounterTop:
            case ObjectHighlight.ObjectType.Board:
            case ObjectHighlight.ObjectType.Return:
                HandleCounterTopOrBoardInteraction();
                break;
            case ObjectHighlight.ObjectType.Craft:
                HandleCraftInteraction();
                break;
            case ObjectHighlight.ObjectType.Bin:
                HandleBinInteraction();
                break;
            case ObjectHighlight.ObjectType.Station:
                HandleStationInteraction();
                break;
            case ObjectHighlight.ObjectType.Oven:
                HandleOvenInteraction();
                break;
            default:
                HandleGeneralObjectInteraction();
                break;
        }
    }

    private void HandleOvenInteraction()
    {
        var ingredient = transform.GetChild(1).gameObject.GetComponent<Ingredient>();
        
        // Ingredient 컴포넌트가 존재하고, 그 타입이 Plate인지 확인
        if (IsHolding && ingredient != null && ingredient.type == Ingredient.IngredientType.Plate)
        {
            var plateComponent = transform.GetChild(1).gameObject; // Plates Object
            var Oven = objectHighlight.transform.parent.gameObject;
            bool isDough = plateComponent.transform.GetChild(9).gameObject.activeSelf;
            if (Oven.transform.childCount == 2 && !plateComponent.transform.GetComponent<Ingredient>().pizzaIsCooked && isDough)
            {
                plateComponent.transform.SetParent(Oven.transform);

                // 위치 설정
                plateComponent.transform.localPosition = new Vector3(0f, 0.01f, 0f);

                // 회전 설정
                plateComponent.transform.localRotation = Quaternion.Euler(0f, 2.281f, 0f);

                // 크기 조정
                plateComponent.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                Oven.GetComponent<Oven>().StartCooking();

                IsHolding = false;
                Animator.SetBool("isHolding", IsHolding);
            }
        }
        else if (!IsHolding)
        {
            // 다시 꺼냄. 근데 요리중일땐 못꺼냄.
            var Oven = objectHighlight.transform.parent.gameObject;
            var plateComponent = Oven.transform.GetChild(2).gameObject;

            plateComponent.transform.SetParent(transform);

            // 위치 설정
            plateComponent.transform.localPosition = new Vector3(-0.4100023f, 0.4699999f, 1.840027f);

            // 회전 설정
            plateComponent.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            // 크기 조정
            plateComponent.transform.localScale = new Vector3(312.5f, 312.5f, 312.5f);

            IsHolding = true;
            Animator.SetBool("isHolding", IsHolding);
        }
    }

    // Station
    private void HandleStationInteraction()
    {
        var ingredient = transform.GetChild(1).gameObject.GetComponent<Ingredient>();
        if (!IsHolding || ingredient == null || ingredient.type != Ingredient.IngredientType.Plate) return;
        var plateComponent = transform.GetChild(1).gameObject.GetComponent<Plates>(); // Plates 컴포넌트를 가져옴

        if (_gameManager.CheckMenu(plateComponent.containIngredients))
        {
            // 접시의 재료가 메뉴와 일치하면
            SoundManager.Instance.PlayEffect("right"); // 성공 효과음 재생
            _gameManager.MakeOrder(); // 주문을 만듦
        }
        else
        {
            // 접시의 재료가 메뉴와 일치하지 않으면
            SoundManager.Instance.PlayEffect("no"); // 실패 효과음 재생
            //TriggerFailureEffect();  // 실패 시 빨간색 불 들어오는 함수 호출 (추가 구현 필요)
        }

        Destroy(transform.GetChild(1).gameObject); // 접시 전체를 삭제 (추후 재활용을 고려)
        IsHolding = false; // 아이템을 들고 있는 상태를 해제
        Animator.SetBool("isHolding", IsHolding); // 애니메이션 상태를 업데이트
        _gameManager.PlateReturn(); // 접시 반환 처리
        // Ingredient 컴포넌트가 존재하고, 그 타입이 Plate인지 확인
    }

    // CounterTop, Board
    private void HandleCounterTopOrBoardInteraction()
    {
        switch (CanActive)
        {
            case true when IsHolding && !objectHighlight.onSomething:
                // 내가 뭘 들고있고, 테이블이나 찹핑테이블 위에 없을떄
                TablePlaceOrDropObject(false);
                break;
            case true when IsHolding && objectHighlight.onSomething:
                // 내가 뭘 들고있고, 테이블이나 찹핑테이블 리턴 위에 있을때
                TablePlaceOrDropObject(true);
                break;
            case true when interactObject.GetComponent<ObjectHighlight>().onSomething:
            {
                GameObject handleThing = interactObject.transform.parent.GetChild(2).gameObject;
                ;

                if (interactObject.transform.parent.GetChild(2).name.Equals("PFX_PanFire"))
                {
                    handleThing = interactObject.transform.parent.GetChild(3).gameObject;
                }

                if (handleThing.CompareTag("Ingredient") &&
                    objectHighlight.objectType == ObjectHighlight.ObjectType.Board &&
                    interactObject.transform.GetChild(0).GetComponent<CuttingBoard>().cookingBar.IsActive())
                {
                    // 손질 중인 재료는 집을 수 없음
                }
                else
                {
                    //화덕에서 onSomething을 끄지않고 꺼내야함.

                    TryPickupObject(handleThing);
                }

                break;
            }
        }
    }

    // Craft
    private void HandleCraftInteraction()
    {
        // Craft 위에 뭔가 없을때
        if (!objectHighlight.onSomething)
        {
            if (!IsHolding)
            {
                PickupFromCraft();
            }
            else
            {
                //PlaceOrDropObject(false);
            }
        }
        else if (!IsHolding) // 테이블 위에 뭔가 있는데, 손에 든게 아무것도 없을때
        {
            PickupFromCraft();
        }
    }

    // Bin
    private void HandleBinInteraction()
    {
        if (IsHolding && transform.GetChild(1).GetComponent<Ingredient>()?.type == Ingredient.IngredientType.Plate)
        {
            DisposePlate();
        }
        else if (transform.childCount > 1)
        {
            DisposeObject();
        }
    }

    private void HandleGeneralObjectInteraction()
    {
        if (IsHolding) return;
        
        if (interactObject.CompareTag("Ingredient"))
        {
            PickupIngredient();
        }
        else if (interactObject.CompareTag("Plate"))
        {
            PickupPlate();
        }
        else if ((interactObject.CompareTag("Pot") || interactObject.CompareTag("Pan")))
        {
            PickupPot();
        }
    }

    private void HoldingItemDropObject()
    {
        // 떨어트리기 로직 상세 구현 필요
        var handlingThing = transform.GetChild(1).gameObject;
    
        // 객체를 내려놓을 때의 로직
        if (handlingThing.CompareTag("Plate"))
        {
            // 떨구는 객체가 접시면.
            handlingThing.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
        }
        else if (handlingThing.CompareTag("Pot") || handlingThing.CompareTag("Pan"))
        {
            handlingThing.transform.GetComponent<BoxCollider>().isTrigger = false;
            handlingThing.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            var childRigidbody = handlingThing.transform.GetChild(0).GetComponent<Rigidbody>();
            var childCollider = handlingThing.transform.GetChild(0).GetComponent<MeshCollider>();

            childRigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
            childCollider.isTrigger = false;
        }

        handlingThing.transform.SetParent(null); // 부모 설정 해제
        Animator.SetBool("isHolding", false);
        IsHolding = false;
    }

    private void TryPickupObject(GameObject handleThing)
    {
        SoundManager.Instance.PlayEffect("itemPickUp");
        objectHighlight.onSomething = false;
        // isHolding = true;
        // Animator.SetBool("isHolding", isHolding);
        HandleObject(handleThing);
    }

    // 내가 뭔가를 들고있을 때
    // true 테이블 위에 뭔가 있음 , false 테이블 위에 뭔가 없음
    private void TablePlaceOrDropObject(bool drop)
    {
        SoundManager.Instance.PlayEffect(drop ? "itemPickUp" : "itemputDown");
        if (drop)
        {
            // true 테이블 위에 뭔가 있는데 내가 가진게 접시고, 음식이면 담음
            if (CanPlaceIngredient())
            {
                PlaceIngredient();
            }

            if (objectHighlight.transform.parent.GetChild(2))
            {
                var ingredientObj = transform.GetChild(1).gameObject;
                var ingredient = ingredientObj.transform.GetChild(0).GetChild(0).GetComponent<Ingredient>().type;

                var potAndPan = objectHighlight.transform.parent.GetChild(2).gameObject;
                if (potAndPan.name.Equals("PFX_PanFire")) potAndPan = objectHighlight.transform.parent.GetChild(3).gameObject;

                // 테이블에 있는게, Pan이고 내가 든게 미트, 닭고기면 실행
                if (potAndPan.CompareTag("Pan") && (ingredient == Ingredient.IngredientType.Meat ||
                                                    ingredient == Ingredient.IngredientType.Chicken))
                {
                    var meshFilter = ingredientObj.transform.GetChild(0).GetComponent<MeshFilter>();
                    
                    string meshFileName = meshFilter.sharedMesh.name;

                    ingredientObj.transform.SetParent(potAndPan.transform);
                    ingredientObj.transform.localPosition = new Vector3(2e-05f, -0.00017f, 0.00013f); // 위치 설정
                    ingredientObj.transform.localRotation = Quaternion.Euler(0f, -168.905f, 0f); // 회전 설정

                    potAndPan.GetComponent<PanOnStove>().inSomething = true;
                    potAndPan.GetComponent<PanOnStove>().AddNewIngredient();

                    Animator.SetBool("isHolding", false);
                    IsHolding = false;
                }

                // 테이블에 있는게, Pot이고 내가 든게 쌀이고 화덕이면 Pot에 붙이기
                if (potAndPan.CompareTag("Pot") && ingredient == Ingredient.IngredientType.Rice)
                {
                    ingredientObj.transform.SetParent(potAndPan.transform);
                    ingredientObj.transform.localPosition = new Vector3(2e-05f, -0.00017f, 0.00013f);  // 위치 설정
                    ingredientObj.transform.localRotation = Quaternion.Euler(0f, -168.905f, 0f); // 회전 설정

                    potAndPan.GetComponent<PotOnStove>().inSomething = true;
                    potAndPan.GetComponent<PotOnStove>().AddNewIngredient();

                    Animator.SetBool("isHolding", false);
                    IsHolding = false;
                }
            }
        }
        else
        {
            // false 테이블 위에 뭔가 없음 => 놓기.
            GameObject handlingThing = transform.GetChild(1).gameObject;
            HandleObject(handlingThing, false);
        }
    }

    private bool CanPlaceIngredient()
    {
        return CanActive && objectHighlight.onSomething
                         && interactObject.transform.parent.childCount > 2
                         && IsPlate(interactObject.transform.parent.GetChild(2))
                         && IsHolding
                         && IsHoldingCookedIngredient();
    }

    private bool IsPlate(Transform obj)
    {
        var handle = obj.GetComponent<Ingredient>();
        return handle != null && handle.type == Ingredient.IngredientType.Plate;
    }

    private bool IsHoldingCookedIngredient()
    {
        var holdingObj = transform.GetChild(1).GetChild(0);
        if (holdingObj.childCount > 0)
        {
            var handle = holdingObj.GetChild(0).GetComponent<Ingredient>();
            bool checkIsCooked = handle.isCooked;

            // 김은 조리 안되어도 접시 올라감
            if (handle.type == Ingredient.IngredientType.SeaWeed || handle.type == Ingredient.IngredientType.Tortilla)
            {
                checkIsCooked = true;
            }

            // 미트 치킨은 조리안하면 못올라감
            //if (handle.type == Ingredient.IngredientType.Meat ||
            //    handle.type == Ingredient.IngredientType.Chicken)
            //{
            //    checkisCooked = false;
            //}

            return handle != null && checkIsCooked;
        }

        return false;
    }

    private void PlaceIngredient()
    {
        var plate = interactObject.transform.parent.GetChild(2).GetComponent<Plates>();
        var ingredient = transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Ingredient>().type;
        if (!plate.AddIngredient(ingredient)) return;
        
        SoundManager.Instance.PlayEffect("itemputDown");
        plate.InstantiateUI();
        Destroy(transform.GetChild(1).gameObject);
        IsHolding = false;
        Animator.SetBool("isHolding", false);
    }

    private void HandleObject(GameObject obj, bool isPickingUp = true)
{
    if (isPickingUp)
    {
        PickupObject(obj);
    }
    else
    {
        DropObject();
    }
}

private void PickupObject(GameObject obj)
{
    if (obj.CompareTag("Plate"))
    {
        HandlePlatePickup(obj);
    }
    else if (obj.CompareTag("Pan") || obj.CompareTag("Pot"))
    {
        HandlePanOrPotPickup(obj);
    }
    else
    {
        HandleGenericPickup(obj);
    }
}

private void HandlePlatePickup(GameObject obj)
{
    obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    obj.transform.SetParent(transform);
    SetPositionBetweenPlayerAndObject(obj);
    Animator.SetBool("isHolding", true);
    IsHolding = true;
}

private void HandlePanOrPotPickup(GameObject obj)
{
    if (IsCookedIngredient(obj))
    {
        HandleCookedIngredient(obj);
    }
    else
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        Ingredient ingredient = obj.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            ingredient.HandleIngredient(transform, ingredient.type, true);
        }
        obj.transform.SetParent(transform);
        // SetPositionBetweenPlayerAndObject(obj); // Uncomment if necessary
        Animator.SetBool("isHolding", true);
        IsHolding = true;

        if (obj.CompareTag("Pan"))
        {
            obj.transform.GetChild(0).GetComponent<BoxCollider>().size *= 2f;
        }
    }
}

private void HandleGenericPickup(GameObject obj)
{
    var childRigidbody = obj.transform.GetChild(0).GetComponent<Rigidbody>();
    if (childRigidbody != null)
    {
        childRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    var ingredient = obj.transform.GetChild(0).GetChild(0).GetComponent<Ingredient>();
    if (ingredient != null)
    {
        ingredient.HandleIngredient(transform, ingredient.type, true);
    }
    obj.transform.SetParent(transform);
    SetPositionBetweenPlayerAndObject(obj);
    Animator.SetBool("isHolding", true);
    IsHolding = true;
}

private void DropObject()
{
    SoundManager.Instance.PlayEffect("itemputDown");
    GameObject handleThing = transform.GetChild(1).gameObject;

    Vector3 offset = new Vector3(0, 0, 0);
    if (interactObject.transform.parent.CompareTag("MineCounter"))
    {
        offset = new Vector3(0.072f, 0.006f, 0.024f);
    }
    else if (interactObject.transform.parent.CompareTag("WizardCounter"))
    {
        offset = new Vector3(0.10746f, 0.00500000005f, 0.0235699993f);
    }
    else if (interactObject.transform.parent.CompareTag("MineBoard"))
    {
        offset = new Vector3(0f, 0.0055f, 0f);
    }

    _placeTransform = interactObject.transform.parent.GetChild(1).localPosition + offset;
    Vector3 playerDirection = transform.forward;

    if (handleThing.CompareTag("Ingredient"))
    {
        HandleIngredientDrop(handleThing);
    }
    else if (handleThing.CompareTag("Pot") || handleThing.CompareTag("Pan") || handleThing.CompareTag("Plate"))
    {
        HandleContainerDrop(handleThing, playerDirection);
    }

    Animator.SetBool("isHolding", false);
    IsHolding = false;
}

private void HandleIngredientDrop(GameObject handleThing)
{
    objectHighlight.onSomething = true;
    IsHolding = false;
    var ingredient = handleThing.transform.GetChild(0).transform.GetChild(0).GetComponent<Ingredient>();
    ingredient.isOnDesk = true;
    ingredient.IngredientHandleOff(interactObject.transform.parent, _placeTransform, ingredient.type);
}

private void HandleContainerDrop(GameObject handleThing, Vector3 playerDirection)
{
    if (objectHighlight.objectType == ObjectHighlight.ObjectType.Board)
    {
        return;
    }

    objectHighlight.onSomething = true;
    IsHolding = false;
    var ingredient = handleThing.GetComponent<Ingredient>();
    ingredient.isOnDesk = true;

    if (handleThing.CompareTag("Pot") || handleThing.CompareTag("Pan"))
    {
        ingredient.PlayerHandleOff(interactObject.transform.parent, _placeTransform, Quaternion.LookRotation(playerDirection).normalized);
        if (handleThing.CompareTag("Pan"))
        {
            handleThing.transform.GetChild(0).GetComponent<BoxCollider>().size /= 2f;
        }
    }
    else // Plate
    {
        ingredient.PlayerHandleOff(interactObject.transform.parent, _placeTransform);
    }
}

private bool IsCookedIngredient(GameObject obj)
{
    if (obj.transform.childCount != 3) return false;
    
    var ingredient = obj.transform.GetChild(2).transform.GetChild(0).GetChild(0).GetComponent<Ingredient>();
    return ingredient != null && ingredient.isCooked;
}

private void HandleCookedIngredient(GameObject obj)
{
    objectHighlight.onSomething = true;

    if (obj.CompareTag("Pan"))
    {
        obj.GetComponent<PanOnStove>().inSomething = false;
    }
    else if (obj.CompareTag("Pot"))
    {
        obj.GetComponent<PotOnStove>().inSomething = false;
    }

    var cookedIngredientObj = obj.transform.GetChild(2).gameObject;
    cookedIngredientObj.transform.SetParent(transform);
    var ingredient = cookedIngredientObj.transform.GetChild(0).GetChild(0).GetComponent<Ingredient>();
    ingredient.HandleIngredient(transform, ingredient.type, true);

    Animator.SetBool("isHolding", true);
    IsHolding = true;
}


    private void PickupFromCraft()
    {
        SoundManager.Instance.PlayEffect("itemPickUp");
        // Craft에서 아이템 꺼내기 로직 구현
        interactObject.GetComponent<Craft>().OpenCraftPlayer1();
        objectHighlight.onSomething = false;
        IsHolding = true;
        Animator.SetBool("isHolding", IsHolding);
    }

    private void DisposePlate()
    {
        transform.GetChild(1).gameObject.GetComponent<Plates>().ClearIngredient();
    }

    private void DisposeObject()
    {
        Destroy(transform.GetChild(1).gameObject);
        IsHolding = false;
        Animator.SetBool("isHolding", IsHolding);
    }

    private void PickupObject(GameObject obj, bool isIngredient)
    {
        SoundManager.Instance.PlayEffect("itemPickUp");
        IsHolding = true;
        Animator.SetBool("isHolding", IsHolding);

        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        if (isIngredient)
        {
            var ingredient = obj.transform.GetChild(0).GetComponent<Ingredient>();
            ingredient.HandleIngredient(transform, ingredient.type, true);
        }
        else
        {
            var ingredient = obj.GetComponent<Ingredient>();
            ingredient.HandleIngredient(transform, ingredient.type, true);
        }
    }

    private void PickupIngredient()
    {
        var ingredientObjectParent = interactObject.transform.parent.gameObject;
        PickupObject(ingredientObjectParent, true);
    }

    private void PickupPot()
    {
        var ingredientObjectParent = interactObject.transform.parent.gameObject;
        PickupObject(ingredientObjectParent, false);
    }
    
    private void PickupPlate()
    {
        // 접시 인식
        var plateObject = interactObject.transform.parent.gameObject;
        TryPickupObject(plateObject);
        //SetPositionbetweenPlayerAndObject(plateObject);
    }

    #endregion

    #region OnTriggerEnter

    private void OnTriggerEnter(Collider other)
    {
        if (CheckForIngredientHandling(other)) return;

        HandleActiveObjectInteraction(other);
    }

    private bool CheckForIngredientHandling(Collider other)
    {
        // 손에 잡고있고, interactObject가 Null이 아니고, 그 태그가 Ingredient일때 True
        // 손에 없거나, interactObject가 Null이거나, 그 태그가 Ingredient가 아니면 False -> HandlePickupIngredient
        if (interactObject != null && interactObject.CompareTag("Ingredient") && IsHolding)
        {
            DeactivateObjectHighlight();
            ResetInteractObjects();
            return true;
        }

        return HandlePickupIngredient(other);
    }

    private void ResetInteractObjects()
    {
        interactObject = null;
        objectHighlight = null;
    }

    private void DeactivateObjectHighlight()
    {
        interactObject.GetComponent<ObjectHighlight>().DeactivateHighlight(interactObject.GetComponent<Ingredient>().isCooked);
    }

    private bool HandlePickupIngredient(Collider other)
    {
        if (!other.CompareTag("Ingredient")) return false;
        
        if (interactObject == null && !other.GetComponent<Ingredient>().isOnDesk)
        {
            SetInteractObject(other.gameObject);
            other.GetComponent<ObjectHighlight>().ActivateHighlight(other.GetComponent<Ingredient>().isCooked);
            return true;
        }

        return false;
    }

    private void SetInteractObject(GameObject obj)
    {
        interactObject = obj;
        objectHighlight = obj.GetComponent<ObjectHighlight>();
    }

    private void HandleActiveObjectInteraction(Collider other)
    {
        if (interactObject) return;

        if (!other.GetComponent<ObjectHighlight>()) return;

        CanActive = true;

        SetInteractObject(other.gameObject);
        other.GetComponent<ObjectHighlight>().activeObject = true;

        if (other.CompareTag("Ingredient"))
        {
            other.GetComponent<ObjectHighlight>().ActivateHighlight(other.GetComponent<Ingredient>().isCooked);
        }
        else
        {
            other.GetComponent<ObjectHighlight>().ActivateHighlight(other.GetComponent<ObjectHighlight>().onSomething);
        }

        if (interactObject.GetComponent<ObjectHighlight>().objectType == ObjectHighlight.ObjectType.Board)
        {
            Animator.SetBool("canCut", true);
        }
    }


    #endregion

    #region OnTriggerStay

    private void OnTriggerStay(Collider other)
    {
        if (ShouldReturnEarly(other))
            return;

        if (HandleActiveIngredientSwitch(other))
            return;

        InitializeActiveObjectIfNull(other);
    }

    private bool ShouldReturnEarly(Collider other)
    {
        return (other.CompareTag("Ingredient") || other.CompareTag("Plate") || other.CompareTag("Pan") ||
                other.CompareTag("Pot")) && IsHolding;
    }

    private bool HandleActiveIngredientSwitch(Collider other)
    {
        if (interactObject != null && interactObject.CompareTag("Ingredient") && IsHolding)
        {
            if (nextInteractObject != null)
            {
                SwitchActiveObject();
            }
            else
            {
                ClearActiveObjects();
            }

            return true;
        }

        return false;
    }

    private void SwitchActiveObject()
    {
        interactObject.GetComponent<ObjectHighlight>()
            .DeactivateHighlight(interactObject.GetComponent<Ingredient>().isCooked);
        interactObject = nextInteractObject;
        objectHighlight = interactObject.GetComponent<ObjectHighlight>();
        interactObject.GetComponent<ObjectHighlight>()
            .DeactivateHighlight(interactObject.GetComponent<Ingredient>().isCooked);
        nextInteractObject = null;
    }

    private void ClearActiveObjects()
    {
        interactObject.GetComponent<ObjectHighlight>()
            .DeactivateHighlight(interactObject.GetComponent<Ingredient>().isCooked);
        ResetInteractObjects();
    }

    private void InitializeActiveObjectIfNull(Collider other)
    {
        if (interactObject == null)
        {
            CanActive = true;
            if (other.GetComponent<ObjectHighlight>() != null)
            {
                SetInteractObject(other.gameObject);
                other.GetComponent<ObjectHighlight>().activeObject = true;
                HighlightBasedOnTag(other);
            }
        }
    }

    private void HighlightBasedOnTag(Collider other)
    {
        bool highlightState = !other.CompareTag("Ingredient") || other.GetComponent<Ingredient>().isCooked;
        other.GetComponent<ObjectHighlight>().ActivateHighlight(highlightState);
    }

    #endregion

    #region OnTriggerExit

    private void OnTriggerExit(Collider other)
    {
        if (ShouldDeactivateObjects())
        {
            HandleBoardInteraction();
            DeactivateObjects();
        }
        else if (ShouldSwitchToNextObject(other))
        {
            SwitchActiveToNextObject();
        }
        else
        {
            ClearNextObject();
        }
    }

    private bool ShouldDeactivateObjects()
    {
        return interactObject != null && nextInteractObject == null;
    }

    private bool ShouldSwitchToNextObject(Collider other)
    {
        return other.gameObject == interactObject;
    }

    private void HandleBoardInteraction()
    {
        if (interactObject.GetComponent<ObjectHighlight>().objectType == ObjectHighlight.ObjectType.Board)
        {
            Animator.SetBool("canCut", false);
            interactObject.transform.GetChild(0).GetComponent<CuttingBoard>().PauseSlider(true);
        }
    }

    private void DeactivateObjects()
    {
        CanActive = false;
        OffHighlightCurrentObject();
        interactObject = null;
        objectHighlight = null;
    }

    private void SwitchActiveToNextObject()
    {
        Animator.SetBool("canCut", false);
        OffHighlightCurrentObject();
        interactObject = nextInteractObject;
        objectHighlight = interactObject?.GetComponent<ObjectHighlight>();
        nextInteractObject = null;
        OnHighlightCurrentObject();
    }

    private void ClearNextObject()
    {
        nextInteractObject = null;
    }

    private void OffHighlightCurrentObject()
    {
        if (interactObject != null && interactObject.GetComponent<ObjectHighlight>() != null)
        {
            bool highlightState = !interactObject.CompareTag("Ingredient") || interactObject.GetComponent<Ingredient>().isCooked;
            interactObject.GetComponent<ObjectHighlight>().DeactivateHighlight(highlightState);
        }
    }

    private void OnHighlightCurrentObject()
    {
        if (interactObject != null && interactObject.GetComponent<ObjectHighlight>() != null)
        {
            /*
            bool highlightState = interactObject.CompareTag("Ingredient")
                ? interactObject.GetComponent<Ingredient>().isCooked
                : true;
            */
            bool highlightState = !interactObject.CompareTag("Ingredient") || interactObject.GetComponent<Ingredient>().isCooked;
            interactObject.GetComponent<ObjectHighlight>().DeactivateHighlight(highlightState);
        }
    }

    #endregion

    #region SetPosition

    private void SetHand()
    {
        if (IsHolding) //뭘 집었다면 손 접기
        {
            knife.SetActive(false);
            idleL.SetActive(false);
            idleR.SetActive(false);
            grabL.SetActive(true);
            grabR.SetActive(true);
        }
        else
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("New_Chef@Chop")) //다지는 중이면
            {
                knife.SetActive(true);
                idleL.SetActive(false);
                idleR.SetActive(false);
                grabL.SetActive(true);
                grabR.SetActive(true);
            }
            else
            {
                knife.SetActive(false);
                idleL.SetActive(true);
                idleR.SetActive(true);
                grabL.SetActive(false);
                grabR.SetActive(false);
            }
        }
    }

    private void SetPositionBetweenPlayerAndObject(GameObject obj)
    {
        if (obj.CompareTag("Plate"))
        {
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localPosition = new Vector3(-0.409999996f, 0.4700001f, 1.84000003f);
        }
    }

    #endregion
}