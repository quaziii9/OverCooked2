using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCameraMoving : StateMachineBehaviour
{
    private Vector3 targetTransform = new Vector3(-3.07999992f, -0.449999988f, -9.89999962f);
    private Quaternion targetRotation = Quaternion.Euler(new Vector3(0.326138616f, 12.5265751f, 0.704141259f));
    private Vector3 velocity = Vector3.zero;

    public virtual void OnStateExit()
    {
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetTransform, ref velocity, 3f);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, targetRotation, 3f);

    }
}
