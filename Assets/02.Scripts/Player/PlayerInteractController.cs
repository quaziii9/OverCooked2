using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
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

    [Header("Grab Object Control")]
    [SerializeField] private GameObject idleR;
    [SerializeField] private GameObject idleL;
    [SerializeField] private GameObject grabR;
    [SerializeField] private GameObject grabL;

    private void Awake()
    {
        freeState = new FreeState(this);
        holdState = new HoldState(this);
        currentState = freeState;  // 초기 상태 설정
    }

    #region CatchOrKnockback,CookOrThrow, PickupOrPlace
    public void CatchOrKnockback()
    {
        if (isHanded())
        {
            // 맞는 로직 구현
            ChangeState(holdState);  // 상태 전환
        }
        else
        {
            // 잡는 로직 구현
            ChangeState(freeState);  // 상태 전환
        }
    }

    public void CookOrThrow()
    {
        if (isHanded())
        {
            // 집기 로직 구현
            ChangeState(holdState);  // 상태 전환
        }
        else
        {
            // 내려놓기 로직 구현
            ChangeState(freeState);  // 상태 전환
        }
    }
    
    public void PickupOrPlace()
    {
        
    }
    #endregion

    #region OnCookOrThrow, OnPickupOrPlace
    public void OnCookOrThrow(InputValue inputValue)
    {
        if (isHanded())
        {
            // 요리 로직 구현
            ChangeState(holdState);  // 상태 전환
        }
        else
        {
            // 던지기 로직 구현
            ChangeState(freeState);  // 상태 전환
        }
    }

    public void OnPickupOrPlace(InputValue inputValue)
    {
        if (isHanded())
        {
            // 집기 로직 구현
            ChangeState(holdState);  // 상태 전환
        }
        else
        {
            // 내려놓기 로직 구현
            ChangeState(freeState);  // 상태 전환
        }
    }
    #endregion

    public void ChangeState(IPlayerState newState)
    {
        currentState = newState;
    }

    // 손에 물건이 있으면 bool 값이 바뀜.
    private bool isHanded()
    {
        return true;
    }
    /*
    #region OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("deadZone"))
        {
            HandleDeadZone();
            return;
        }

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
            if (interactObject == null && !other.GetComponent<Handle>().isOnDesk)
            {
                SetinteractObject(other.gameObject);
                other.GetComponent<Object>().OnHighlight(other.GetComponent<Handle>().isCooked);
                return true;
            }
        }
        return false;
    }

    private void SetinteractObject(GameObject obj)
    {
        interactObject = obj;
        interactObjectOb = obj.GetComponent<Object>();
    }

    private void HandleActiveObjectInteraction(Collider other)
    {
        if (other == interactObject || interactObject != null)
            return;

        if (other.GetComponent<Object>() == null)
            return;

        canActive = true;

        SetinteractObject(other.gameObject);
        other.GetComponent<Object>().canActive = true;

        if (other.CompareTag("Ingredient"))
        {
            other.GetComponent<Object>().OnHighlight(other.GetComponent<Handle>().isCooked);
        }
        else
        {
            other.GetComponent<Object>().OnHighlight(other.GetComponent<Object>().onSomething);
        }

        if (interactObject.GetComponent<Object>().type == Object.ObjectType.Board)
        {
            anim.SetBool("canCut", true);
        }
    }
    #endregion
    */
}
