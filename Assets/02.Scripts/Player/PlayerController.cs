using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState;
    private FreeState freeState;
    private HoldState holdState;

    public Animator animator;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float turnSpeed = 10f;

    private Vector2 moveInput;

    private void Awake()
    {
        freeState = new FreeState(this);
        holdState = new HoldState(this);
        currentState = freeState;  // 초기 상태 설정
    }

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // 이동 입력이 없을 때 애니메이터의 이동 파라미터를 false로 설정
        if (moveInput.magnitude == 0)
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }

    void FixedUpdate()
    {
        if(moveInput.magnitude != 0)
        {
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
            Vector3 movement = moveDir * moveSpeed;
            rb.velocity = movement;

            // transform.forward = moveDir;

            // 플레이어가 이동하는 방향을 바라보도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    public void CatchOrKnockback()
    {
        // 대쉬 로직 구현
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

    public void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    public void OnDash(InputValue inputValue)
    {
        // 일정 거리 
    }

    public void OnCookOrThrow(InputValue inputValue)
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

    // 손에 물건이 있으면 bool 값이 바뀜.
    private bool isHanded()
    {
        return true;
    }

}
