using UnityEngine;

public interface IHandle
{
    void PickUp(Player player);
    void PutDown(Transform targetTransform);
}