using UnityEngine;

public class Player : MonoBehaviour
{
    public void InteractWithObject(GameItem Item)
    {
        Item.Interact(this);
    }

    public void CookWithObject(ICookable cookable)
    {
        cookable.Cook(this);
    }
}
