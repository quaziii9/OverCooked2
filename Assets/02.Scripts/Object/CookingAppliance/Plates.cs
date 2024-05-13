using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Plates : MonoBehaviour
{
    public List<Ingredient.IngredientType> containIngredients = new List<Ingredient.IngredientType>();
    public int limit = 1;
    public GameObject Canvas;
    [SerializeField] private GameObject IngredientUI;
    [SerializeField] private Sprite[] Icons;
    GameObject madeUI;

    public bool AddIngredient(Ingredient.IngredientType handleType)
    {
        if (!CheckOverlap(handleType) && containIngredients.Count < limit)
        {
            containIngredients.Add(handleType);
            UpdateVisibleChild(handleType);
            return true;
        }
        return false;
    }

    public void ClearIngredient()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        containIngredients.Clear();
        Destroy(madeUI);
    }

    private bool CheckOverlap(Ingredient.IngredientType type)
    {
        return containIngredients.Contains(type);
    }

    private void UpdateVisibleChild(Ingredient.IngredientType handleType)
    {
        int childIndex = (int)handleType - 1; // Assuming children are ordered according to HandleType enum
        if (childIndex < transform.childCount)
        {
            transform.GetChild(childIndex).gameObject.SetActive(true);
        }
    }

    public void InstantiateUI()
    {
        if (madeUI != null) Destroy(madeUI);
        madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
        SetupUI();
    }

    private void SetupUI()
    {
        for (int i = 0; i < containIngredients.Count; i++)
        {
            int childIndex = containIngredients.Count - 1; // Child index to activate
            madeUI.transform.GetChild(childIndex).gameObject.SetActive(true);
            Image image = madeUI.transform.GetChild(childIndex).GetChild(i).GetComponent<Image>();
            image.sprite = Icons[(int)containIngredients[i] - 1];
        }
    }
}
