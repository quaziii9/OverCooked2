using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    private Rigidbody rb;

    [Header("Dashing")]
    public float dashForce;
    public float dashDuration;
    private bool isDashing = false;

    [Header("Settings")]
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Dash Curve")]
    public AnimationCurve dashCurve; // AnimationCurve를 대시의 세기 변화에 사용

    [Header("Player Master Controller")]
    public PlayerMasterController2 masterController;

    [Header("Mobile Button")]
    public Button dashButton; // UI 버튼 참조

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitializeDashButton();
    }

    private void Update()
    {
        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void InitializeDashButton()
    {
        if (dashButton != null)
        {
            dashButton.onClick.AddListener(MobileDash); // 버튼 클릭 이벤트에 MobileDash 메서드 연결
        }
    }

    public void OnDash(InputValue inputButton)
    {
        if (inputButton.isPressed && !isDashing && dashCdTimer <= 0 && masterController.currentPlayer == this.gameObject)
        {
            Dash();
        }
    }

    // UI 버튼을 클릭할 때 호출될 메서드
    private void MobileDash()
    {
        if (!isDashing && dashCdTimer <= 0 && masterController.currentPlayer == this.gameObject)
        {
            Dash();
        }
    }

    private void Dash()
    {
        if (dashCdTimer > 0 || isDashing)
            return;

        dashCdTimer = dashCd;
        if (disableGravity)
            rb.useGravity = false;

        if (resetVel)
            rb.velocity = Vector3.zero;

        StartCoroutine(ExecuteDash());
    }

    private IEnumerator ExecuteDash()
    {
        isDashing = true; // 코루틴 시작 시 isDashing 설정
        float elapsedTime = 0f;
        PlayerPuff.Instance.BoostPuff(transform);

        Vector3 dashDirection = transform.forward; // 대시 방향을 미리 계산

        while (elapsedTime < dashDuration)
        {
            SoundManager.Instance.PlayEffect("dash");
            float dashProgress = elapsedTime / dashDuration;
            float curveValue = dashCurve.Evaluate(dashProgress); // AnimationCurve에서 값을 평가
            Vector3 forceToApply = dashDirection * (dashForce * curveValue);
            rb.AddForce(forceToApply, ForceMode.Impulse);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        if (disableGravity)
            rb.useGravity = true;
    }
}
