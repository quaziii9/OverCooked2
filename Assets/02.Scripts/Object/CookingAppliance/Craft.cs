using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Craft : MonoBehaviour
{
    [SerializeField] private Animator CraftAnim;
    public enum FoodType { Fish, Shrimp, Lettuce, Cucumber, Tomato, Chicken, Potato }
    public FoodType food;
    public GameObject foodPrefabs;

    public void OpenCraftPlayer1()
    {
        OpenCraft(FindObjectOfType<PlayerInteractController>());
    }

    public void OpenCraftPlayer2()
    {
        OpenCraft(FindObjectOfType<Player2InteractController>());
    }

    private void OpenCraft(PlayerInteractController player)
    {
        CraftAnim.SetTrigger("Open");
        GameObject newFood = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
        player.isHolding = true;
        newFood.transform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        newFood.transform.GetChild(0).transform.GetChild(0).GetComponent<Ingredient>().HandleIngredient(player.transform, ConvertFoodTypeToHandleType(food), true);
    }
    private void OpenCraft(Player2InteractController player)
    {
        CraftAnim.SetTrigger("Open");
        GameObject newFood = Instantiate(foodPrefabs, Vector3.zero, Quaternion.identity);
        player.isHolding = true;
        newFood.transform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        newFood.transform.GetChild(0).transform.GetChild(0).GetComponent<Ingredient>().HandleIngredient(player.transform, ConvertFoodTypeToHandleType(food), true);
    }
    private Ingredient.IngredientType ConvertFoodTypeToHandleType(FoodType food)
    {
        switch (food)
        {
            case FoodType.Fish:
                return Ingredient.IngredientType.Fish;
            case FoodType.Shrimp:
                return Ingredient.IngredientType.Shrimp;
            case FoodType.Lettuce:
                return Ingredient.IngredientType.Lettuce;
            case FoodType.Cucumber:
                return Ingredient.IngredientType.Cucumber;
            case FoodType.Tomato:
                return Ingredient.IngredientType.Tomato;
            case FoodType.Chicken:
                return Ingredient.IngredientType.Chicken;
            case FoodType.Potato:
                return Ingredient.IngredientType.Potato;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(food), "Invalid food type");
        }
    }
}
