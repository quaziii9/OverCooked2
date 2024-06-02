using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Plates : MonoBehaviour
{
    [Header("UI")]
    public GameObject canvas;
    public GameObject ingredientUI;
    [SerializeField] private GameObject madeUI;

    public List<Ingredient.IngredientType> containIngredients = new List<Ingredient.IngredientType>();
    public int limit = 1;

    public Sprite[] icons;

    private Dictionary<Ingredient.IngredientType, int> ingredientToChildIndex = new Dictionary<Ingredient.IngredientType, int>
    {
        { Ingredient.IngredientType.Fish, 1 },
        { Ingredient.IngredientType.Shrimp, 2 },
        { Ingredient.IngredientType.Tomato, 3 },
        { Ingredient.IngredientType.Lettuce, 4 },
        { Ingredient.IngredientType.Cucumber, 5 },
        { Ingredient.IngredientType.Potato, 6 },
        { Ingredient.IngredientType.Dough, 9 },
        { Ingredient.IngredientType.Pepperoni, 10 },
        { Ingredient.IngredientType.Cheese, 11 },
        { Ingredient.IngredientType.PizzaTomato, 12 },
        { Ingredient.IngredientType.SeaWeed, 7 },
        { Ingredient.IngredientType.Tortilla, 8 }
    };

    public bool AddIngredient(Ingredient.IngredientType handleType)
    {
        // Dough가 활성화되어야 하는 재료들
        var requiresDough = new[] {
            Ingredient.IngredientType.Pepperoni,
            Ingredient.IngredientType.Cheese,
            Ingredient.IngredientType.PizzaTomato
        };

        var requiresTortilla = new[] {
            Ingredient.IngredientType.Meat,
            Ingredient.IngredientType.Chicken,
            Ingredient.IngredientType.Rice
        };

        var requiresSeaWeed = new[] {
            Ingredient.IngredientType.SushiRice,
            Ingredient.IngredientType.SushiFish,
            Ingredient.IngredientType.SushiCucumber
        };

        // Dough가 포함되지 않은 상태에서 requiresDough 타입이 들어오면 false 반환
        if (requiresDough.Contains(handleType) && !containIngredients.Contains(Ingredient.IngredientType.Dough))
        {
            return false;
        }

        // Tortilla가 포함되지 않은 상태에서 requiresTortilla 타입이 들어오면 false 반환
        if (requiresTortilla.Contains(handleType) && !containIngredients.Contains(Ingredient.IngredientType.Tortilla))
        {
            return false;
        }

        // SeaWeed가 포함되지 않은 상태에서 requiresTortilla 타입이 들어오면 false 반환
        if (requiresSeaWeed.Contains(handleType) && !containIngredients.Contains(Ingredient.IngredientType.SeaWeed))
        {
            return false;
        }

        if (!CheckOverlap(handleType) && containIngredients.Count < limit)
        {
            containIngredients.Add(handleType);
            ActivateIngredientUI(handleType);
            return true;
        }
        return false;
    }

    private void ActivateIngredientUI(Ingredient.IngredientType handleType)
    {
        // 해당 재료에 맞는 UI를 활성화
        if (ingredientToChildIndex.ContainsKey(handleType))
        {
            transform.GetChild(ingredientToChildIndex[handleType]).gameObject.SetActive(true);
        }
        else if (handleType == Ingredient.IngredientType.SeaWeed || handleType == Ingredient.IngredientType.SushiRice || handleType == Ingredient.IngredientType.SushiFish || handleType == Ingredient.IngredientType.SushiCucumber)
        {
            UpdateCombinationUI(13, new[] { Ingredient.IngredientType.SeaWeed, Ingredient.IngredientType.SushiRice, Ingredient.IngredientType.SushiFish, Ingredient.IngredientType.SushiCucumber });
        }
        else if (handleType == Ingredient.IngredientType.Tortilla || handleType == Ingredient.IngredientType.Meat || handleType == Ingredient.IngredientType.Chicken || handleType == Ingredient.IngredientType.Rice)
        {
            UpdateCombinationUI(14, new[] { Ingredient.IngredientType.Tortilla, Ingredient.IngredientType.Rice, Ingredient.IngredientType.Meat, Ingredient.IngredientType.Chicken });
        }
    }

    private void UpdateCombinationUI(int parentIndex, Ingredient.IngredientType[] ingredientTypes)
    {
        // 모든 조합 비활성화
        for (int i = 0; i < transform.GetChild(parentIndex).childCount; i++)
        {
            transform.GetChild(parentIndex).GetChild(i).gameObject.SetActive(false);
        }

        // 재료 조합 상태에 따라 UI를 활성화
        var activeIndexes = ingredientTypes.Select((type, index) => containIngredients.Contains(type) ? index : -1).Where(index => index >= 0).ToArray();
        if (activeIndexes.Length > 0)
        {
            transform.GetChild(parentIndex).GetChild(activeIndexes.Length - 1).gameObject.SetActive(true);
        }
    }

    public void ClearIngredient()
    {
        // 모든 재료를 제거하고 UI를 초기화
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        containIngredients.Clear();
        Destroy(madeUI);
    }

    private bool CheckOverlap(Ingredient.IngredientType type)
    {
        // 재료가 이미 포함되어 있는지 확인
        return containIngredients.Contains(type);
    }

    public void InstantiateUI()
    {
        // 기존 UI가 존재하면 제거
        if (madeUI != null)
        {
            Destroy(madeUI);
        }

        // 재료 UI를 생성합니다.
        if (containIngredients.Count > 0)
        {
            madeUI = Instantiate(ingredientUI, Vector3.zero, Quaternion.identity, canvas.transform);
            for (int i = 0; i < 4; i++)
            {
                madeUI.transform.GetChild(i).gameObject.SetActive(i == containIngredients.Count - 1);
            }
            Image[] images = madeUI.transform.GetChild(containIngredients.Count - 1).GetComponentsInChildren<Image>();
            AddIconMethod(images);
            madeUI.GetComponent<IngredientUI>().target = transform;
        }
    }

    private void AddIconMethod(Image[] images)
    {
        // 각 재료에 맞는 아이콘을 설정합니다.
        for (int i = 0; i < containIngredients.Count; i++)
        {
            images[i].sprite = GetIcon(containIngredients[i]);
        }
    }

    private Sprite GetIcon(Ingredient.IngredientType type)
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
            _ => null,
        };
    }
}

