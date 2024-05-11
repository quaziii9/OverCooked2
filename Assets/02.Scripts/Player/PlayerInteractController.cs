using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteracteController : MonoBehaviour
{
    private IPlayerState currentState;
    private FreeState freeState;
    private HoldState holdState;

    private void Awake()
    {
        freeState = new FreeState(this);
        holdState = new HoldState(this);
        currentState = freeState;  // 초기 상태 설정
    }

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

    public void ChangeState(IPlayerState newState)
    {
        currentState = newState;
    }

    // 손에 물건이 있으면 bool 값이 바뀜.
    private bool isHanded()
    {
        return true;
    }
}
