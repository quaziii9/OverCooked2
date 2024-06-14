using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject bus;
    public Transform target; // 타겟 오브젝트의 Transform

    public Vector3 offset; // 카메라와 타겟 사이의 거리

    public Vector3 startVector;
    public float smoothSpeed = 0.125f; // 카메라 이동 속도

    private void Start()
    {
        startVector = transform.position;
        FindBus();
    }

    // Update is called once per frame
    private void Update()
    {
        FollowBus();
    }

    private void FindBus()
    {
        bus = GameObject.FindWithTag("Bus");
    }

    private void FollowBus()
    {
        offset = bus.transform.position + startVector;
        gameObject.transform.position = new Vector3(offset.x, startVector.y, offset.z);
    }
}
