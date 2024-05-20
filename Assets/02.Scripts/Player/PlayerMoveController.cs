using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;

    [Header("Speed")]
    [SerializeField] public float moveSpeed = 15f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float fallMultiplier = 2.5f; // 떨어질 때 속도 증가
    private Vector2 moveInput;
    private bool isDashing = false;
    public int puffCount;
    public bool isGrounded;
    public bool isOnStairs;
    public float downforce=1f;


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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, downforce);
        isOnStairs = Physics.Raycast(transform.position, Vector3.down, downforce);

        if (!isDashing && moveInput.magnitude != 0)
        {

            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
            Vector3 movement = moveDir * moveSpeed;
            rb.velocity = movement;
            
            // 플레이어가 이동하는 방향을 바라보도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        // 땅에 닿아있지 않다면 추가적인 중력 적용
        if (!isGrounded)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        if (isOnStairs&& moveInput.magnitude != 0)//언덕위에서 이동중일때
        {
            rb.AddForce(Vector3.down * 5f , ForceMode.Acceleration);
        }
        else if(isOnStairs&& moveInput.magnitude == 0)
        {
            rb.velocity = Vector3.zero;
        }
    }


    public void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * downforce);
    }
     void OnCollisionEnter(Collision other)
    {
        // Print how many points are colliding with this transform
        Debug.Log("Points colliding: " + other.contacts.Length);

        // Print the normal of the first point in the collision.
        Debug.Log("Normal of the first point: " + other.contacts[0].normal);

        // Draw a different colored ray for every normal in the collision
        foreach (var item in other.contacts)
        {
            Debug.DrawRay(item.point, item.normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
        }
    }
}

