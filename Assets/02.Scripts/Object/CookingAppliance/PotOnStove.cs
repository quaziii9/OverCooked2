using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PotOnStove : MonoBehaviour
{
    public Slider cookingBar;
    public bool isOnStove = false;
    public float cookingTime;
    private Coroutine _coTimer;
    private bool pause = false;

    private void Update()
    {
        if (isOnStove)
        {
            UpdateCookingBarPosition();
            UpdateCookingBarValue();
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
