using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject bus;
    public Transform target; // 타겟 오브젝트의 Transform

    public Vector3 offset; // 카메라와 타겟 사이의 거리

    public float smoothSpeed = 0.125f; // 카메라 이동 속도


    void Start()
    {
        BusFind();

    }

    // Update is called once per frame
    void Update()
    {
        BusFind();
        FlowBus();
    }

    void BusFind()
    {
        if (bus == null)
        {
            bus = GameObject.FindWithTag("Bus");
        }
        else
        {
            Debug.Log("Bus Find Fall!!");
        }
    }
    
    void FlowBus()
    {
       
        //gameObject.transform.position = new Vector3(bus.transform.position.x, gameObject.transform.position.y, bus.transform.position.z);

    }
}
