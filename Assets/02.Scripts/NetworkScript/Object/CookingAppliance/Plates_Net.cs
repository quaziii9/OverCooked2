using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Plates_Net : MonoBehaviour
{
    public List<Ingredient.IngredientType> containIngredients = new List<Ingredient.IngredientType>();
    public int limit = 1;
    public GameObject Canvas;
    [SerializeField] private GameObject IngredientUI;
    [SerializeField] private Sprite[] Icons;
    public GameObject madeUI;

    public bool AddIngredient(Ingredient.IngredientType handleType)
    {
        if (!CheckOverlap(handleType) && containIngredients.Count < limit)
        {
            containIngredients.Add(handleType);

            if (handleType == Ingredient.IngredientType.Fish)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (handleType == Ingredient.IngredientType.Shrimp)
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (handleType == Ingredient.IngredientType.Tomato)
            {
                transform.GetChild(3).gameObject.SetActive(true);
            }
            else if (handleType == Ingredient.IngredientType.Lettuce)
            {
                transform.GetChild(4).gameObject.SetActive(true);
            }
            else if (handleType == Ingredient.IngredientType.Cucumber)
            {
                transform.GetChild(5).gameObject.SetActive(true);
            }
            else if (handleType == Ingredient.IngredientType.Potato)
            {
                transform.GetChild(6).gameObject.SetActive(true);
            }
            else if (handleType == Ingredient.IngredientType.Chicken)
            {
                transform.GetChild(7).gameObject.SetActive(true);
            }
            return true;
        }
        return false;
    }

    public void ClearIngredient()
    {
        if (containIngredients.Count == 1)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        containIngredients.RemoveRange(0, containIngredients.Count); //다 지우고
        Destroy(madeUI); //UI도 지우기
    }
    private bool CheckOverlap(Ingredient.IngredientType type)
    {
        for (int i = 0; i < containIngredients.Count; i++)
        {
            if (containIngredients[i].Equals(type))
            {
                return true;
            }
        }
        return false;
    }

    public void InstantiateUI()
    {
        if (containIngredients.Count == 1)
        {
            madeUI = Instantiate(IngredientUI, Vector3.zero, Quaternion.identity, Canvas.transform);
            madeUI.transform.GetChild(1).gameObject.SetActive(false);
            madeUI.transform.GetChild(2).gameObject.SetActive(false);
            Image image = madeUI.transform.GetChild(0).GetComponent<Image>();
            if (containIngredients[0].Equals(Ingredient.IngredientType.Fish))
            {
                image.sprite = Icons[0];
            }
            else if (containIngredients[0].Equals(Ingredient.IngredientType.Shrimp))
            {
                image.sprite = Icons[1];
            }
            else if (containIngredients[0].Equals(Ingredient.IngredientType.Tomato))
            {
                image.sprite = Icons[2];
            }
            else if (containIngredients[0].Equals(Ingredient.IngredientType.Lettuce))
            {
                image.sprite = Icons[3];
            }
            else if (containIngredients[0].Equals(Ingredient.IngredientType.Cucumber))
            {
                image.sprite = Icons[4];
            }
            else if (containIngredients[0].Equals(Ingredient.IngredientType.Potato))
            {
                image.sprite = Icons[5];
            }
            else if (containIngredients[0].Equals(Ingredient.IngredientType.Chicken))
            {
                image.sprite = Icons[6];
            }
            madeUI.GetComponent<IngredientUI>().Target = transform;
        }
        else if (containIngredients.Count == 2) //접시에 합쳐진게 2개면
        {
            madeUI.transform.GetChild(0).gameObject.SetActive(false);
            madeUI.transform.GetChild(1).gameObject.SetActive(true);
            madeUI.transform.GetChild(2).gameObject.SetActive(false);
            Image[] images = new Image[2];
            images[0] = madeUI.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            images[1] = madeUI.transform.GetChild(1).GetChild(1).GetComponent<Image>();
            for (int i = 0; i < containIngredients.Count; i++)
            {
                if (containIngredients[i].Equals(Ingredient.IngredientType.Fish))
                {
                    images[i].sprite = Icons[0];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Shrimp))
                {
                    images[i].sprite = Icons[1];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Tomato))
                {
                    images[i].sprite = Icons[2];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Lettuce))
                {
                    images[i].sprite = Icons[3];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Cucumber))
                {
                    images[i].sprite = Icons[4];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Potato))
                {
                    images[i].sprite = Icons[5];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Chicken))
                {
                    images[i].sprite = Icons[6];
                }
            }
        }
        else if (containIngredients.Count == 3) //재료가 3개면
        {
            madeUI.transform.GetChild(0).gameObject.SetActive(false);
            madeUI.transform.GetChild(1).gameObject.SetActive(false);
            madeUI.transform.GetChild(2).gameObject.SetActive(true);
            Image[] images = new Image[3];
            images[0] = madeUI.transform.GetChild(2).GetChild(0).GetComponent<Image>();
            images[1] = madeUI.transform.GetChild(2).GetChild(1).GetComponent<Image>();
            images[2] = madeUI.transform.GetChild(2).GetChild(2).GetComponent<Image>();
            for (int i = 0; i < containIngredients.Count; i++)
            {
                if (containIngredients[i].Equals(Ingredient.IngredientType.Fish))
                {
                    images[i].sprite = Icons[0];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Shrimp))
                {
                    images[i].sprite = Icons[1];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Tomato))
                {
                    images[i].sprite = Icons[2];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Lettuce))
                {
                    images[i].sprite = Icons[3];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Cucumber))
                {
                    images[i].sprite = Icons[4];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Potato))
                {
                    images[i].sprite = Icons[5];
                }
                else if (containIngredients[i].Equals(Ingredient.IngredientType.Chicken))
                {
                    images[i].sprite = Icons[6];
                }
            }
        }
    }
}