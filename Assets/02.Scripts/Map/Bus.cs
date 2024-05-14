using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bus : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotspeed = 2f;

    private Vector3 inputMovement = Vector3.zero;
    private Vector3 move = Vector3.zero;
    
    public GameObject busObject;
    public GameObject rotBusObject;
    public Rigidbody busRigid;

    public bool isboost=false;

    private void Start()
    {
        isboost = false;
        busRigid= GetComponent<Rigidbody>();
    }

    void Update()
    {
        BusMove();
        BusRotate();
        OnBoost();
    }

    private void OnMove(InputValue inputValue)
    {
        Debug.Log("input1");
        inputMovement = inputValue.Get<Vector2>(); 
        move = new  Vector3(inputMovement.x, 0f, inputMovement.y);
    }

    public void BusMove()
    {
        busObject.transform.Translate(move * speed * Time.deltaTime,Space.World);
        busRigid.AddForce(move * speed * Time.deltaTime, ForceMode.Impulse);
    }
    public void BusRotate()
    {
        //rotbusobjdet.transform.rotation= move=
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            rotBusObject.transform.rotation = Quaternion.Slerp(rotBusObject.transform.rotation, targetRotation, rotspeed * Time.deltaTime);
        }
    }

    public void OnBoost()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            //Debug.Log("BOost");
            if (!isboost)
            {
                isboost = true;
                StartCoroutine("BoostCoroutine");
            }
        }
    }

    IEnumerator BoostCoroutine()
    {
        Debug.Log("Boost");

        Rigidbody rb= GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 20,ForceMode.Impulse);

        for (int i = 0; i < 4; i++)
        {
            rb.AddForce(transform.forward, ForceMode.Impulse);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        rb.velocity = Vector3.zero;
        isboost = false;
    }
}

