using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ingredient;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

[CreateAssetMenu(fileName = "Menu Data", menuName = "Scriptable Object/Menu data", order = int.MaxValue)]

public class Menu : ScriptableObject
{
    public int Level;
    public string MenuName;
    public List<Ingredient.IngredientType> Ingredient;
    public float LimitTime;
    public Sprite MenuIcon;
    public List<Sprite> IngredientIcon;
    public int Price;
}
