using UnityEngine;

public class StoveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PotOnStove pot = other.GetComponent<PotOnStove>();
        if (pot != null)
        {
            pot.isOnStove = true;
            pot.StartCooking();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PotOnStove pot = other.GetComponent<PotOnStove>();
        if (pot != null)
        {
            pot.isOnStove = false;
            pot.OffSlider();
        }
    }
}
