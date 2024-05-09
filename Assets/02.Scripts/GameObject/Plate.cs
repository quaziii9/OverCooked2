using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : GameItem, IHandle
{
    public override void Interact(Player player)
    {
        throw new System.NotImplementedException();
    }

    public void PickUp(Player player)
    {
        throw new System.NotImplementedException();
    }

    public void PutDown(Transform targetTransform)
    {
        throw new System.NotImplementedException();
    }
}