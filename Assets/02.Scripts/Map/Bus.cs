using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Bus : MonoBehaviour
{
    [SerializeField]
    public float speed = 1f;
    [SerializeField]
    public float rotspeed = 2f;
    private Vector3 inputMovement = Vector3.zero;
    private Vector3 move = Vector3.zero;
    public GameObject busobject;
    public GameObject rotbusobjdet;
    public bool isboost=false;

    private void Start()
    {
        isboost = false;
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
        busobject.transform.Translate(move * speed * Time.deltaTime,Space.World);
    }
    public void BusRotate()
    {
        //rotbusobjdet.transform.rotation= move=
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            rotbusobjdet.transform.rotation = Quaternion.Slerp(rotbusobjdet.transform.rotation, targetRotation, rotspeed * Time.deltaTime);
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
        Debug.Log("BOost");

        busobject.transform.Translate(busobject.transform.forward * 1f, Space.World);
        yield return new WaitForSecondsRealtime(0.05f);
        busobject.transform.Translate(busobject.transform.forward * 1f, Space.World);
        yield return new WaitForSecondsRealtime(0.05f);
        busobject.transform.Translate(busobject.transform.forward * 1f, Space.World);
        yield return new WaitForSecondsRealtime(0.05f);
        busobject.transform.Translate(busobject.transform.forward * 1f, Space.World);
        yield return new WaitForSecondsRealtime(0.05f);
        busobject.transform.Translate(busobject.transform.forward * 1f, Space.World);
        yield return new WaitForSecondsRealtime(0.05f);
        yield return new WaitForSecondsRealtime(1f);
        isboost = false;
    }
}

