using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class PlayerInteractController : MonoBehaviour
{
    // 상태 변화 패턴
    private IPlayerState currentState;
    private FreeState freeState;
    private HoldState holdState;

    // 애니메이션
    public Animator anim;

    // 상호작용 할 수 있는 오브젝트
    public GameObject interactObject;
    public ObjectHighlight objectHighlight;
    public GameObject nextInteractObject;

    // 변경되는 Bool값
    [SerializeField] public bool isHolding = false;
    // 재료를 제외한 모든 오브젝트 활성화 확인값
    [SerializeField] public bool canActive = false; 

    [Header("Grab Object Control")]
    [SerializeField] private GameObject idleR;
    [SerializeField] private GameObject idleL;
    [SerializeField] private GameObject grabR;
    [SerializeField] private GameObject grabL;
    [SerializeField] private GameObject knife;


    private void Awake()
    {
        freeState = new FreeState(this);
        holdState = new HoldState(this);
        currentState = freeState;  // 초기 상태 설정
    }
    
    #region 인터렉션 부분

    #endregion
    

    #region CatchOrKnockback,CookOrThrow, PickupOrPlace
    public void CatchOrKnockback()
    {
    }

    public void CookOrThrow()
    {
    }
    
    public void PickupOrPlace()
    {
        
    }
    #endregion

    #region OnCookOrThrow
    public void OnCookOrThrow(InputValue inputValue)
    {
        Debug.Log("OnCookOrThrow");
    }
    #endregion

    #region OnPickupOrPlace
    public void OnPickupOrPlace(InputValue inputValue)
    {
        Debug.Log("OnPickupOrPlace");
        SetHand();
        ProcessInteraction();
    }

    private void ProcessInteraction()
    {
        if (interactObject == null) return;

        switch (objectHighlight.objectType)
        {
            case ObjectHighlight.ObjectType.CounterTop:
            case ObjectHighlight.ObjectType.Board:
                HandleCounterTopOrBoardInteraction();
                break;
            case ObjectHighlight.ObjectType.Craft:
                HandleCraftInteraction();
                break;
            case ObjectHighlight.ObjectType.Bin:
                HandleBinInteraction();
                break;
            default:
                HandleGeneralObjectInteraction();
                break;
        }
    }

    private void HandleCounterTopOrBoardInteraction()
    {
        if (canActive && interactObject.GetComponent<ObjectHighlight>().onSomething)
        {
            GameObject handleThing = interactObject.transform.parent.GetChild(2).gameObject;

            if (handleThing.CompareTag("Ingredient") && objectHighlight.objectType == ObjectHighlight.ObjectType.Board &&
                interactObject.transform.GetChild(0).GetComponent<CuttingBoard>().cookingBar.IsActive())
            {
                // 손질 중인 재료는 집을 수 없음
            }
            else
            {
                TryPickupObject(handleThing);
            }
        }
        else if (canActive && !objectHighlight.onSomething && isHolding)
        {
            PlaceOrDropObject(false);
        }
        else if (canActive && isHolding && objectHighlight.onSomething)
        {
            PlaceOrDropObject(true);
        }
    }

    private void HandleCraftInteraction()
    {
        if (!objectHighlight.onSomething)
        {
            if (!isHolding)
            {
                PickupFromCraft();
            }
            else
            {
                PlaceOrDropObject(false);
            }
        }
        else if (!isHolding)
        {
            PickupFromCraft();
        }
    }

    private void HandleBinInteraction()
    {
        if (isHolding && transform.GetChild(1).GetComponent<Ingredient>()?.type == Ingredient.IngredientType.Plate)
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

        if (!isHolding && interactObject.CompareTag("Ingredient"))
        {
            PickupIngredient();
        }
        else if (isHolding)
        {
            PlaceOrDropObject(false);
        }
    }

    private void TryPickupObject(GameObject handleThing)
    {
        //SoundManager.instance.PlayEffect("take");
        objectHighlight.onSomething = false;
        isHolding = true;
        anim.SetBool("isHolding", isHolding);
        HandleObject(handleThing);
    }

    private void PlaceOrDropObject(bool drop)
    {
        //SoundManager.instance.PlayEffect(drop ? "put" : "place");
        if (drop)
        {
            DropObject(); // 로직 상세 구현 필요
        }
        else
        {
            // 놓기 로직 구현
            GameObject handlingThing = transform.GetChild(1).gameObject;
            objectHighlight.onSomething = true;
            isHolding = false;
            HandleObject(handlingThing, false);
        }
    }

    private void HandleObject(GameObject obj, bool isPickingUp = true)
    {
        if (isPickingUp)
        {
            // 객체를 들어 올릴 때의 로직
            Debug.Log("올려");
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            obj.transform.SetParent(transform); // 플레이어의 하위 객체로 설정
            anim.SetBool("isHolding", true);
            isHolding = true;
        }
        else
        {
            // 객체를 내려놓을 때의 로직
            Debug.Log("내려");
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            obj.transform.SetParent(null); // 부모 설정 해제
            anim.SetBool("isHolding", false);
            isHolding = false;
        }
    }

    private void PickupFromCraft()
    {
        //SoundManager.instance.PlayEffect("take");
        // Craft에서 아이템 꺼내기 로직 구현
        interactObject.GetComponent<Craft>().OpenCraftPlayer1();
        objectHighlight.onSomething = false;
        isHolding = true;
        anim.SetBool("isHolding", isHolding);
    }

    private void DisposePlate()
    {
        transform.GetChild(1).gameObject.GetComponent<Plates>().ClearIngredient();
    }

    private void DisposeObject()
    {
        Destroy(transform.GetChild(1).gameObject);
        isHolding = false;
        anim.SetBool("isHolding", isHolding);
    }

    private void PickupIngredient()
    {
        //SoundManager.instance.PlayEffect("take");
        isHolding = true;
        anim.SetBool("isHolding", isHolding);
        // 재료 줍기 로직 상세 구현 필요
        GameObject ingredientObj = interactObject.transform.parent.gameObject;
        ingredientObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ingredientObj.transform.GetChild(0).GetComponent<Ingredient>().HandleIngredient(transform, ingredientObj.transform.GetChild(0).GetComponent<Ingredient>().type, true);
    }

    private void DropObject()
    {
        // 떨어트리기 로직 상세 구현 필요
    }

    #endregion

    public void ChangeState(IPlayerState newState)
    {
        currentState = newState;
    }

    #region OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("deadZone"))
        //{
        //    HandleDeadZone();
        //    return;
        //}
        
        if (CheckForIngredientHandling(other))
            return;

        HandleActiveObjectInteraction(other);
    }

    private void HandleDeadZone()
    {
        // 사운드 메니져
        //SoundManager.instance.PlayEffect("fall");
        //DieRespawn();
    }
    
    private bool CheckForIngredientHandling(Collider other)
    {
        // 손에 잡고있고, interactObject가 Null이 아니고, 그 태그가 Ingredient일때 True
        // 손에 없거나, interactObject가 Null이거나, 그 태그가 Ingredient가 아니면 False -> HandlePickupIngredient
        if (interactObject != null && interactObject.CompareTag("Ingredient") && isHolding)
        {
            DeactivateObjectHighlight();
            ResetinteractObjects();
            return true;
        }

        return HandlePickupIngredient(other);
    }

    private void ResetinteractObjects()
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
        if (other.CompareTag("Ingredient"))
        {
            if (interactObject == null && !other.GetComponent<Ingredient>().isOnDesk)
            {
                SetinteractObject(other.gameObject);
                other.GetComponent<ObjectHighlight>().ActivateHighlight(other.GetComponent<Ingredient>().isCooked);
                return true;
            }
        }
        return false;
    }

    private void SetinteractObject(GameObject obj)
    {
        interactObject = obj;
        objectHighlight = obj.GetComponent<ObjectHighlight>();
    }

    private void HandleActiveObjectInteraction(Collider other)
    {
        if (other == interactObject || interactObject != null)
            return;

        if (other.GetComponent<ObjectHighlight>() == null)
            return;

        canActive = true;

        SetinteractObject(other.gameObject);
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
            anim.SetBool("canCut", true);
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
        return (other.CompareTag("Ingredient") || other.CompareTag("Plate")) && isHolding;
    }

    private bool HandleActiveIngredientSwitch(Collider other)
    {
        if (interactObject != null && interactObject.CompareTag("Ingredient") && isHolding)
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
        interactObject.GetComponent<ObjectHighlight>().DeactivateHighlight(interactObject.GetComponent<Ingredient>().isCooked);
        interactObject = nextInteractObject;
        objectHighlight = interactObject.GetComponent<ObjectHighlight>();
        interactObject.GetComponent<ObjectHighlight>().DeactivateHighlight(interactObject.GetComponent<Ingredient>().isCooked);
        nextInteractObject = null;
    }

    private void ClearActiveObjects()
    {
        interactObject.GetComponent<ObjectHighlight>().DeactivateHighlight(interactObject.GetComponent<Ingredient>().isCooked);
        ResetinteractObjects();
    }

    private void InitializeActiveObjectIfNull(Collider other)
    {
        if (interactObject == null)
        {
            canActive = true;
            if (other.GetComponent<ObjectHighlight>() != null)
            {
                SetinteractObject(other.gameObject);
                other.GetComponent<ObjectHighlight>().activeObject = true;
                HighlightBasedOnTag(other);
            }
        }
    }

    private void HighlightBasedOnTag(Collider other)
    {
        bool highlightState = other.CompareTag("Ingredient") ? other.GetComponent<Ingredient>().isCooked : true;
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
            anim.SetBool("canCut", false);
            interactObject.transform.GetChild(0).GetComponent<CuttingBoard>().PauseSlider(true);
        }
    }

    private void DeactivateObjects()
    {
        canActive = false;
        OffHighlightCurrentObject();
        interactObject = null;
        objectHighlight = null;
    }

    private void SwitchActiveToNextObject()
    {
        anim.SetBool("canCut", false);
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
            bool highlightState = interactObject.CompareTag("Ingredient") ? interactObject.GetComponent<Ingredient>().isCooked : true;
            interactObject.GetComponent<ObjectHighlight>().DeactivateHighlight(highlightState);
        }
    }

    private void OnHighlightCurrentObject()
    {
        if (interactObject != null && interactObject.GetComponent<Object>() != null)
        {
            bool highlightState = interactObject.CompareTag("Ingredient") ? interactObject.GetComponent<Ingredient>().isCooked : true;
            interactObject.GetComponent<ObjectHighlight>().DeactivateHighlight(highlightState);
        }
    }
    #endregion

    private void SetHand()
    {
        if (isHolding) //뭘 집었다면 손 접기
        {
            knife.SetActive(false);
            idleL.SetActive(false);
            idleR.SetActive(false);
            grabL.SetActive(true);
            grabR.SetActive(true);
        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("New_Chef@Chop")) //다지는 중이면
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
}
