using UnityEngine;

public class Ingredient : GameItem
{
    public enum IngredientType { Fish, Shrimp, Plate, Lettuce, Tomato, Cucumber, Chicken, Potato };
    public IngredientType type; // 재료 유형: 채소, 고기 등
    
    public enum IngredientState { Raw, Cooking, Cooked }
    public IngredientState currentState = IngredientState.Raw;

    public Vector3 fishLocalPos = new Vector3(0, 0.138f, 0.08f);

    public override void Interact(PlayerInteractController player)
    {
        // 기본 상호작용: 재료를 주움
        //Pickup(player);
    }

    // 상태 변경 메소드
    public void ChangeState(IngredientState newState)
    {
        currentState = newState;
        Debug.Log("State changed to: " + currentState);
    }
}
