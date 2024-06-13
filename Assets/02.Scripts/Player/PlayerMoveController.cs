using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    public Animator animator;
    private Rigidbody rb;
    private float soundTimer = 0f;
    private float soundInterval = 0.5f; // 소리 재생 간격을 0.5초로 변경

    [Header("Speed")]
    public float moveSpeed = 15f;
    public float turnSpeed = 10f;
    public float fallMultiplier = 5f; // 떨어질 때 속도 증가
    public float rayDistance = 1f;

    private Vector2 moveInput;
    private bool isDashing = false;
    private int puffCount = 5;

    [Header("State")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isOnStairs;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        puffCount++;

        // 이동 입력이 없을 때 애니메이터의 이동 파라미터를 false로 설정
        if (moveInput == Vector2.zero)
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

        // 매 초마다 소리 재생
        soundTimer += Time.deltaTime;
        if (soundTimer >= soundInterval && moveInput != Vector2.zero)
        {
            soundTimer -= soundInterval;
            SoundManager.Instance.PlayEffect("move");
        }
    }

    void FixedUpdate()
    {
        UpdateGroundAndStairStatus();

        if (!isDashing && moveInput != Vector2.zero)
        {
            MoveCharacter();
        }

        ApplyAdditionalGravity();
        HandleStairMovement();
    }

    private void UpdateGroundAndStairStatus()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            isGrounded = hit.collider.CompareTag("Floor") || hit.collider.CompareTag("Stair");
            isOnStairs = hit.collider.CompareTag("Stair");
        }
        else
        {
            isGrounded = false;
            isOnStairs = false;
        }

        // 레이 디버그용 드로우
        Debug.DrawRay(transform.position, Vector3.down * rayDistance, isGrounded ? Color.red : Color.blue);
    }

    private void MoveCharacter()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        Vector3 movement = moveDir * moveSpeed;
        rb.velocity = movement;

        // 플레이어가 이동하는 방향을 바라보도록 회전
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    private void ApplyAdditionalGravity()
    {
        if (!isGrounded)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
    }

    private void HandleStairMovement()
    {
        if (isOnStairs)
        {
            if (moveInput != Vector2.zero)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Acceleration);
            }
            else
            {
                rb.useGravity = false;
            }
        }
        else
        {
            rb.useGravity = true;
        }
    }

    public void OnMove(InputValue inputValue)
    {
        if (inputValue != null) moveInput = inputValue.Get<Vector2>();
    }
}
