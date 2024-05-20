using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;

    [Header("Speed")]
    [SerializeField] public float moveSpeed = 15f;
    [SerializeField] private float turnSpeed = 10f;

    private Vector2 moveInput;
    private bool isDashing = false;

    public int puffCount;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        puffCount = 5;
    }

    void Update()
    {
        // 이동 입력이 없을 때 애니메이터의 이동 파라미터를 false로 설정
        puffCount++;
        if (moveInput.magnitude == 0)
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("isWalking", false);
            puffCount = 5;
        }
        else
        {
            animator.SetBool("isWalking", true);
            if (puffCount >= 5)
            {
                PlayerPuff.Instance.MovePuff(transform);
                puffCount = 0;
            }

        }
    }

    void FixedUpdate()
    {
        if (!isDashing && moveInput.magnitude != 0)
        {
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
            Vector3 movement = moveDir * moveSpeed;
            rb.velocity = movement;
            
            // 플레이어가 이동하는 방향을 바라보도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    public void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }
}
