using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travelator : MonoBehaviour
{
    public float speed = 1.0f; // 트레블레이터의 기본 이동 속도
    public float playerSpeedMultiplier = 2.0f; // 플레이어가 트레블레이터의 방향과 같은 경우 속도 배율
    public float playerSpeedReduction = 0.5f; // 플레이어가 트레블레이터의 방향과 반대인 경우 속도 배율

    private Vector3 moveDirection;

    private void Start()
    {
        // y 값에 따라 이동 방향을 설정
        if (Mathf.Approximately(transform.eulerAngles.y, 180.0f))
        {
            moveDirection = Vector3.right; // y 값이 180이면 오른쪽으로 이동
        }
        else if (Mathf.Approximately(transform.eulerAngles.y, 0.0f))
        {
            moveDirection = Vector3.left; // y 값이 0이면 왼쪽으로 이동
        }
        else
        {
            Debug.LogWarning("Unexpected y rotation value. Defaulting to right direction.");
            moveDirection = Vector3.right;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트레블레이터 위에 올라간 오브젝트가 Rigidbody를 가지고 있는지 확인
        if (other.attachedRigidbody != null)
        {
            if (other.CompareTag("Player"))
            {
                // 플레이어의 경우 별도 처리
                StartCoroutine(MovePlayer(other.attachedRigidbody));
            }
            else
            {
                StartCoroutine(MoveObject(other.attachedRigidbody));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 트레블레이터를 벗어난 오브젝트가 Rigidbody를 가지고 있는지 확인
        if (other.attachedRigidbody != null)
        {
            if (other.CompareTag("Player"))
            {
                StopCoroutine(MovePlayer(other.attachedRigidbody));
            }
            else
            {
                StopCoroutine(MoveObject(other.attachedRigidbody));
            }
        }
    }

    private IEnumerator MoveObject(Rigidbody obj)
    {
        while (true)
        {
            // Rigidbody의 위치를 이동 방향으로 업데이트
            obj.MovePosition(obj.position + moveDirection.normalized * speed * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }
    }

    private IEnumerator MovePlayer(Rigidbody playerRb)
    {
        while (true)
        {
            // 플레이어가 현재 이동하고 있는 방향을 가져옴
            Vector3 playerMoveDirection = playerRb.velocity.normalized;

            // 트레블레이터와 같은 방향으로 이동 중인지 확인
            float dotProduct = Vector3.Dot(playerMoveDirection, moveDirection.normalized);

            if (dotProduct > 0) // 같은 방향
            {
                playerRb.MovePosition(playerRb.position + moveDirection.normalized * speed * playerSpeedMultiplier * Time.deltaTime);
            }
            else if (dotProduct < 0) // 반대 방향
            {
                playerRb.MovePosition(playerRb.position + moveDirection.normalized * speed * playerSpeedReduction * Time.deltaTime);
            }
            else // 직각 방향
            {
                playerRb.MovePosition(playerRb.position + moveDirection.normalized * speed * Time.deltaTime);
            }

            yield return null; // 다음 프레임까지 대기
        }
    }

    private void OnDrawGizmos()
    {
        // 이동 방향을 표시하기 위한 Gizmo 화살표 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + moveDirection);
        Gizmos.DrawSphere(transform.position + moveDirection, 0.1f);
    }
}
