using UnityEngine;

public class Ingredient : GameItem, IHandle
{
    public override void Interact(Player player)
    {
        // 재료와의 상호작용 로직 구현
        Debug.Log("Player has interacted with an ingredient.");
        // 예를 들어, 재료를 플레이어의 인벤토리에 추가하는 기능
        //player.PickUp(this.gameObject);
    }

    public void PickUp(Player player)
    {
        Debug.Log("Ingredient picked up by: " + player.name);
        // 플레이어의 인벤토리나 손에 오브젝트를 추가하는 로직
        this.transform.parent = player.transform;  // 예제: 플레이어의 자식으로 설정
        this.gameObject.SetActive(false);  // 인벤토리에 추가되면 보이지 않게 처리
    }

    public void PutDown(Transform targetTransform)
    {
        Debug.Log("Ingredient put down");
        this.transform.parent = targetTransform;  // 목표 위치에 오브젝트를 배치
        this.gameObject.SetActive(true);  // 다시 보이게 설정
    }
}
