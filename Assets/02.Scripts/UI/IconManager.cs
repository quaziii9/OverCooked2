using System;
using UnityEngine;

public class IconManager : Singleton<IconManager>
{
    public Sprite[] icons; // 재료 아이콘 배열

    public Sprite GetIcon(Ingredient.IngredientType type)
    {
        // 재료 타입에 맞는 아이콘을 반환합니다.
        return type switch
        {
            Ingredient.IngredientType.Fish => icons[0],
            Ingredient.IngredientType.Shrimp => icons[1],
            Ingredient.IngredientType.Tomato => icons[2],
            Ingredient.IngredientType.Lettuce => icons[3],
            Ingredient.IngredientType.Cucumber => icons[4],
            Ingredient.IngredientType.Potato => icons[5],
            Ingredient.IngredientType.Chicken => icons[6],
            Ingredient.IngredientType.SeaWeed => icons[7],
            Ingredient.IngredientType.Tortilla => icons[8],
            Ingredient.IngredientType.Rice => icons[9],
            Ingredient.IngredientType.Pepperoni => icons[10],
            Ingredient.IngredientType.Meat => icons[11],
            Ingredient.IngredientType.Dough => icons[12],
            Ingredient.IngredientType.Cheese => icons[13],
            Ingredient.IngredientType.SushiRice => icons[9],
            Ingredient.IngredientType.SushiFish => icons[0],
            Ingredient.IngredientType.SushiCucumber => icons[4],
            Ingredient.IngredientType.PizzaTomato => icons[2],
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}