using UnityEngine;

public class Craft : GameItem
{
    public override void Interact(Player player)
    {
        // 작업대와의 상호작용 로직 구현
        Debug.Log("Player has interacted with a craft station.");
        // 예를 들어, 아이템을 제작하거나 기능을 활성화하는 로직
        //player.ActivateCraft(this.gameObject);
    }
}
