using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Oven : MonoBehaviour
{
    public Slider cookingBar;
    public float cookingTime;
    private Coroutine _coTimer;
    private bool pause = false;
    private bool stateIsCooked = false;

    private void Update()
    {
        if (!stateIsCooked)
        {
            UpdateCookingBarPosition();
            UpdateCookingBarValue();
            UpdateIsIngredientState();
        }

        if (stateIsCooked)
            cookingBar.gameObject.SetActive(false);
    }

    private void UpdateIsIngredientState()
    {
        if (transform.childCount == 3)
        {
            stateIsCooked = transform.GetChild(2).GetComponent<Ingredient>().pizzaIsCooked;
        }
    }

    private void UpdateCookingBarPosition()
    {
        cookingBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0)); // 적절한 위치 조정
    }

    private void UpdateCookingBarValue()
    {
        cookingBar.value = cookingTime;
    }

    public void StartCooking(UnityAction EndCallBack = null)
    {
        if (_coTimer == null)
        {
            cookingBar.gameObject.SetActive(true);
            ClearTime();
            _coTimer = StartCoroutine(CoStartCooking(EndCallBack));
        }
        else if (pause)
        {
            pause = false;
        }
    }

    private IEnumerator CoStartCooking(UnityAction EndCallBack = null)
    {
        while (cookingTime <= 1)
        {
            while (pause)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.45f);
            cookingTime += 0.25f;
        }
        EndCallBack?.Invoke();
        OffSlider();
        _coTimer = null;
        pause = false;
        cookingTime = 0;
        transform.GetChild(2).GetComponent<Ingredient>().pizzaIsCooked = true;
    }

    private void ClearTime()
    {
        if (_coTimer != null)
        {
            StopCoroutine(_coTimer);
            _coTimer = null;
        }
        pause = false;
    }

    public void OffSlider()
    {
        cookingBar.value = 0f;
        cookingBar.gameObject.SetActive(false);
    }
}
