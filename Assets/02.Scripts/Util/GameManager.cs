using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("Coroutine Management")]
    private Coroutine alpahCoroutine = null; // 알파값 변화 코루틴 저장용 변수
    public Coroutine activeCoroutine = null; // 현재 활성화된 코루틴

    [Header("Game State Management")]
    public bool isPaused = true; // 일시정지 여부
    private bool isMoving = false; // 이동 여부

    public enum State { Stage1, Stage2, Stage3 }; // 게임 상태 열거형
    [SerializeField] private State state = State.Stage1; // 현재 게임 상태

    [Header("UI Elements")]
    [SerializeField] private GameObject canvas; // 캔버스
    [SerializeField] private GameObject ready; // Ready UI
    [SerializeField] private GameObject go; // Go UI

    [Space(10)]
    [SerializeField] private GameObject timesUp; // Time's up UI
    [SerializeField] private Slider timeSlider; // 시간 슬라이더 UI
    [SerializeField] private Text textTime; // 시간 텍스트 UI
    [SerializeField] private GameObject sandTimer; // 모래시계 오브젝트

    [Space(10)]
    [SerializeField] private GameObject oppositeUI; // 상대방 UI
    [SerializeField] private GameObject coinOb; // 동전 오브젝트
    [SerializeField] private Slider tipSlider; // 팁 슬라이더 UI
    [SerializeField] private Text textCoin; // 동전 텍스트 UI
    [SerializeField] private Text textTip; // 팁 텍스트 UI

    [Space(10)]
    [SerializeField] private GameObject flame; // 불꽃 이펙트
    [SerializeField] private GameObject textPrefabs; // 텍스트 프리팹
    [SerializeField] private GameObject wrong; // 잘못된 경우 UI

    [Header("Game Timing")]
    [SerializeField] private float gameTime = 160f; // 게임 시간
    [SerializeField] private float startTime = 0f; // 시작 시간
    private bool playOnce = false; // Ready UI 표시 여부
    private bool playTwice = false; // Go UI 표시 여부
    private bool startSetting = false; // 게임 시작 설정 여부
    private bool once = false; // 한번만 실행되도록 하는 플래그
    private bool isDone = false; // 게임 종료 여부
    private float lastSec = 0f; // 마지막 시간
    private float countSec = 0f; // 초 카운트

    [Header("Player Money")]
    public int originalMoney; // 원래 돈
    public int player1Money; // 플레이어 1의 돈
    public int player2Money; // 플레이어 2의 돈
    public int coin; // 동전 개수
    public int tip; // 팁
    public int tipCombo; // 팁 콤보

    [Header("Order Management")]
    [SerializeField] private Menu[] menus; // 이번 스테이지에 등장할 메뉴들
    [SerializeField] private int maxMenuLimit; // 이번 스테이지에서 최대로 쌓일 수 있는 메뉴 개수들

    [Header("Menu UI Pools")]
    [SerializeField] private GameObject[] singleDoublePoolUIs; // 오브젝트 풀링으로 쓸 단일 메뉴 UI들
    [SerializeField] private GameObject[] triplePoolUIs; // 오브젝트 풀링으로 쓸 3개짜리 메뉴 UI들
    [SerializeField] private GameObject[] quadruplePoolUIs; // 오브젝트 풀링으로 쓸 4개짜리 메뉴 UI들
    public Vector3 poolPos; // 풀 위치

    [Space(10)]
    public List<Menu> currentOrder; // 현재 주문 목록
    public List<GameObject> currentOrderUI; // 현재 주문 UI 목록
    private int i = -1; // 메뉴 인덱스
    private int j = -1; // UI 인덱스

    [Header("Respawn Management")]
    // 접시 리스폰 관련 변수들
    [SerializeField] private GameObject platePrefabs; // 접시 프리팹
    [SerializeField] private float respawnTime = 3f; // 리스폰 시간
    [SerializeField] private GameObject returnCounter; // 반환 카운터

    [Header("Color Change Settings")]
    private float duration = 75f; // 색상 변화 시간
    private float smoothness = 0.1f; // 색상 변화의 부드러움 정도
    private Color startColor = new Color(0, 192 / 255f, 5 / 255f, 255 / 255f); // 초록색
    private Color middleColor = new Color(243 / 255f, 239 / 255f, 0, 255 / 255f); // 노랑색
    private Color endColor = new Color(215 / 255f, 11 / 255f, 0, 1f); // 빨강색
    private Color currentColor; // 현재 색상

    protected override void Awake()
    {
        base.Awake();

        InitializeVariables();      // 변수 초기화
        InitializeStageManager();   // 스테이지 매니저 초기화
        InitializeGameSettings();   // 게임 세팅 초기화
    }

    private void InitializeVariables()
    {
        duration = gameTime / 2;
        currentColor = timeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color;
        timeSlider.maxValue = gameTime;
        timeSlider.value = timeSlider.maxValue;
    }

    private void InitializeStageManager()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.success = 0;
            StageManager.Instance.fail = 0;
            StageManager.Instance.tipMoney = 0;
            StageManager.Instance.totalMoney = 0;
            StageManager.Instance.successMoney = 0;
            StageManager.Instance.failMoney = 0;

            switch (state)
            {
                case State.Stage1:
                    StageManager.Instance.playStage = StageManager.State.Stage1;
                    break;
                case State.Stage2:
                    StageManager.Instance.playStage = StageManager.State.Stage2;
                    break;
                case State.Stage3:
                    StageManager.Instance.playStage = StageManager.State.Stage3;
                    break;
            }
        }
    }

    private void InitializeGameSettings()
    {
        isPaused = true;
        coin = 0;
        SetCoinText();
        StartCoroutine(StartColorAnimation());
    }

    private IEnumerator StartColorAnimation()
    {
        yield return StartCoroutine(LerpColor(startColor, middleColor, gameTime / 2));
        yield return StartCoroutine(LerpColor(middleColor, endColor, gameTime / 2));
    }

    private void Update()
    {
        HandleGameStart();
        HandleGamePause();
        UpdateGameTime();
    }

    private void HandleGameStart()
    {
        if (isPaused && !startSetting)
        {
            ToClock();
            startTime += Time.unscaledDeltaTime;
            Time.timeScale = 0;

            if (startTime > 1 && !playOnce)
            {
                ShowReadyMessage();
            }
            else if (startTime > 1 && playOnce && ready.transform.localScale.x < 1)
            {
                ScaleUp(ready.transform);
            }
            else if (startTime > 4 && playOnce && ready.transform.localScale.x >= 1 && !playTwice)
            {
                ShowGoMessage();
            }
            else if (playTwice && go.transform.localScale.x < 1)
            {
                ScaleUp(go.transform);
            }
            else if (startTime > 6 && playTwice && go.transform.localScale.x > 1)
            {
                StartGame();
            }
        }
        else if (startSetting && !once)
        {
            StartGame();
        }
    }

    private void ShowReadyMessage()
    {
        SoundManager.Instance.PlayEffect("ready");
        ready.SetActive(true);
        playOnce = true;
    }

    private void ShowGoMessage()
    {
        ready.SetActive(false);
        playTwice = true;
        go.SetActive(true);
        SoundManager.Instance.PlayEffect("go");
    }

    private void StartGame()
    {
        go.SetActive(false);
        isPaused = false;
        startSetting = true;
        Time.timeScale = 1;
        once = true;
        Invoke("MakeOrder", 0.5f);
        Invoke("MakeOrder", 5f);
        Invoke("MakeOrder", 30f);
        Invoke("MakeOrder", 80f);
        Invoke("MakeOrder", 150f);
    }

    private void HandleGamePause()
    {
        if (SoundManager.Instance.isSingle && startSetting && once)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                isPaused = !isPaused;
                Time.timeScale = isPaused ? 0 : 1;
            }
        }
    }

    private void UpdateGameTime()
    {
        gameTime -= Time.deltaTime;
        ToClock();

        AdjustBgmPitch();

        if (gameTime < 30)
        {
            countSec += Time.deltaTime;
            sandTimer.GetComponent<Animator>().SetTrigger("shake");
            if (countSec >= 1)
            {
                countSec = 0;
                SoundManager.Instance.PlayEffect("beep");
            }
        }

        if (gameTime <= 0)
        {
            EndGame();
        }
    }

    private void AdjustBgmPitch()
    {
        if (gameTime < 30 && SoundManager.Instance.bgmAudioSource.pitch == 1)
        {
            SoundManager.Instance.bgmAudioSource.pitch = 1.5f;
        }
        else if (gameTime < 15 && SoundManager.Instance.bgmAudioSource.pitch == 1.5f)
        {
            SoundManager.Instance.bgmAudioSource.pitch = 2;
        }
    }

    private void EndGame()
    {
        if (!isDone)
        {
            SoundManager.Instance.bgmAudioSource.pitch = 1;
            SoundManager.Instance.bgmAudioSource.Stop();
            SoundManager.Instance.PlayEffect("timesUp");
            Time.timeScale = 0;
            if (StageManager.Instance != null)
            {
                StageManager.Instance.totalMoney = coin;
                switch (state)
                {
                    case State.Stage1:
                        StageManager.Instance.isClearMap1 = true;
                        break;
                    case State.Stage2:
                        StageManager.Instance.isClearMap2 = true;
                        break;
                    case State.Stage3:
                        StageManager.Instance.isClearMap3 = true;
                        break;
                }
            }
            timesUp.SetActive(true);
            isDone = true;
        }
        else
        {
            ScaleUp(timesUp.transform);
            if (SoundManager.Instance.isSingle)
            {
                lastSec += Time.unscaledDeltaTime;
                if (lastSec > 1)
                {
                    SceneManager.LoadScene("ResultScene");
                }
            }
        }
    }

    private void ScaleUp(Transform transform)
    {
        Vector3 scale = transform.localScale;
        scale.x += Time.unscaledDeltaTime;
        scale.y += Time.unscaledDeltaTime;
        scale.z += Time.unscaledDeltaTime;
        transform.localScale = scale;
    }

    public void LoadResult()
    {
        // 결과 씬 로드
        Debug.Log("실행");
    }

    private void ToClock()
    {
        // 게임 시간을 시계 형식으로 변환
        timeSlider.value = gameTime;
        int min = 0;
        int sec = 0;
        min = (int)(gameTime / 60);
        sec = (int)(gameTime % 60);
        textTime.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);
    }

    public void PlateReturn()
    {
        // 접시 반환 처리
        StartCoroutine(PlateReturnCo());
    }

    IEnumerator PlateReturnCo()
    {
        // 일정 시간 후에 접시를 생성하여 반환 카운터에 추가
        yield return new WaitForSeconds(respawnTime);
        GameObject newPlate = Instantiate(platePrefabs, Vector3.zero, Quaternion.identity);
        if (state == State.Stage1)
        {
            newPlate.GetComponent<Plates>().limit = 3;
        }
        else if (state == State.Stage2)
        {
            newPlate.GetComponent<Plates>().limit = 3;
        }
        else if (state == State.Stage3)
        {
            newPlate.GetComponent<Plates>().limit = 3;
        }
        newPlate.transform.SetParent(returnCounter.transform);
        newPlate.transform.localScale = Vector3.one;
        returnCounter.transform.GetChild(1).GetComponent<Return>().returnPlates.Add(newPlate);
        Vector3 spawnPos = returnCounter.transform.GetChild(1).GetComponent<Return>().SetPosition();
        newPlate.transform.localPosition = spawnPos;
        newPlate.GetComponent<Plates>().canvas = canvas;
    }

    public void MakeOrder()
    {
        // 새로운 주문 생성
        if (currentOrder.Count >= maxMenuLimit)
        {
            return; // 현재 주문이 최대 주문 수를 초과하면 함수를 종료
        }

        i = Random.Range(0, menus.Length); // 랜덤으로 메뉴 선택
        Menu selectedMenu = menus[i];
        int ingredientCount = selectedMenu.Ingredient.Count;

        GameObject[] targetPoolUIs = null;

        // 메뉴의 재료 개수에 따라 사용할 UI 풀을 선택
        switch (ingredientCount)
        {
            case 1:
            case 2:
                targetPoolUIs = singleDoublePoolUIs;
                break;
            case 3:
                targetPoolUIs = triplePoolUIs;
                break;
            case 4:
                targetPoolUIs = quadruplePoolUIs;
                break;
        }

        // 선택된 UI 풀에서 비활성화된 UI를 찾기
        foreach (var ui in targetPoolUIs)
        {
            if (!ui.activeSelf)
            {
                ui.SetActive(true);

                // 재료 아이콘 설정
                for (int k = 0; k < ingredientCount; k++)
                {
                    ui.transform.GetChild(0).GetChild(k).GetComponent<Image>().sprite = selectedMenu.IngredientIcon[k];
                }

                // 두 번째 재료 UI 비활성화 (재료가 2개 이하일 경우)
                if (ingredientCount <= 2)
                {
                    ui.transform.GetChild(1).gameObject.SetActive(ingredientCount == 2);
                }

                // 메뉴 아이콘 및 슬라이더 설정
                ui.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = selectedMenu.MenuIcon;
                var slider = ui.transform.GetChild(2).GetChild(0).GetComponent<Slider>();
                slider.maxValue = selectedMenu.LimitTime;
                slider.value = selectedMenu.LimitTime; // 슬라이더 현재 시간 설정 (풀로 시작)

                // 현재 주문에 추가
                currentOrder.Add(selectedMenu);
                currentOrderUI.Add(ui);
                return;
            }
        }
    }

    private void SuccessEffect()
    {
        // 성공 효과
    }

    public bool CheckMenu(List<Ingredient.IngredientType> containIngredients) // plate의 재료 list들 통으로 받아서 비교
    {
        // 메뉴 확인 및 처리
        originalMoney = coin; // 원래 돈을 저장
        if (containIngredients == null) // 빈 접시만 내면 무조건 실패
        {
            // 빨간색 띵 소리 및 애니메이션 (주석 처리됨)
            // StartCoroutine(TurnAlpha(wrong));
            return false; // 실패 반환
        }

        foreach (var order in currentOrder)
        {
            if (containIngredients.Count != order.Ingredient.Count) // 재료 개수가 다르면 실패
            {
                continue; // 다음 주문으로 넘어감
            }

            if (CheckIngredients(containIngredients, order.Ingredient)) // 재료들이 모두 일치하는지 확인
            {
                ProcessOrder(currentOrder.IndexOf(order)); // 주문 처리
                return true; // 성공 반환
            }
        }

        // StartCoroutine(TurnAlpha(wrong)); // 실패 애니메이션 (주석 처리됨)
        return false; // 실패 반환
    }

    // 재료들이 모두 일치하는지 확인하는 함수
    private bool CheckIngredients(List<Ingredient.IngredientType> containIngredients, List<Ingredient.IngredientType> orderIngredients)
    {
        return !containIngredients.Except(orderIngredients).Any() && !orderIngredients.Except(containIngredients).Any();
    }

    // 주문을 처리하는 함수
    private void ProcessOrder(int index)
    {
        if (index == 0) // 순서대로 메뉴를 냈다면 콤보 증가
        {
            tipCombo++;
            if (tipCombo >= 4)
            {
                if (!flame.activeSelf)
                {
                    flame.SetActive(true); // 불꽃 효과 활성화
                }
                tipCombo = 4; // 최대 4콤보까지 제한
            }
        }
        else
        {
            flame.SetActive(false); // 불꽃 효과 비활성화
            tipCombo = 0; // 콤보 초기화
        }

        var orderUI = currentOrderUI[index];
        orderUI.transform.position = poolPos; // 주문 UI 위치 초기화
        orderUI.SetActive(false); // 주문 UI 비활성화

        if (StageManager.Instance != null)
        {
            StageManager.Instance.successMoney += currentOrder[index].Price; // 성공한 주문의 금액 추가
            StageManager.Instance.success++; // 성공한 주문 수 증가
        }

        coin += currentOrder[index].Price; // 동전 증가
        coinOb.transform.parent.GetChild(2).GetComponent<Animator>().SetTrigger("spin"); // 애니메이션 트리거
        AddTip(index); // 팁 추가
        SetPosition(index); // 위치 설정

        currentOrder.RemoveAt(index); // 완료된 주문 제거
        currentOrderUI.RemoveAt(index); // 완료된 주문 UI 제거
    }

    private void AddTip(int index)
    {
        var orderUI = currentOrderUI[index];
        var timerValue = orderUI.GetComponent<OrderUI>().timer.value;
        var maxTimerValue = orderUI.GetComponent<OrderUI>().timer.maxValue;

        tip = timerValue > maxTimerValue * 0.6f ? 8 :
              timerValue > maxTimerValue * 0.3f ? 5 : 3;

        SetCoinText();
        StartCoroutine(StartBigger()); //---> 커졌다가 작아지는 코루틴 
    }

    private void SetCoinText()
    {
        // 동전 텍스트 업데이트
        textTip.text = tipCombo < 2 ? "" : $"팁 x {tipCombo}";
        if (tipCombo >= 2)
        {
            tip *= tipCombo;
        }
        tipSlider.value = tipCombo;

        if (StageManager.Instance != null)
        {
            StageManager.Instance.tipMoney += tip;
        }

        coin += tip;
        textCoin.text = coin.ToString(); // 돈 얼마됐다고 업데이트

        if (tip != 0)
        {
            GameObject tipText = Instantiate(textPrefabs, Camera.main.WorldToScreenPoint(FindObjectOfType<ScrollMT>().transform.position), Quaternion.identity, canvas.transform);
            tipText.GetComponent<Text>().text = $"+{tip} 팁!";
        }
    }

    IEnumerator StartBigger()
    {
        // 동전 텍스트 커지는 애니메이션
        yield return ScaleCoinText(Vector3.one * 2, Color.white, startColor);
        StartCoroutine(StartSmaller());
    }

    IEnumerator StartSmaller()
    {
        // 동전 텍스트 작아지는 애니메이션
        yield return ScaleCoinText(Vector3.one, startColor, Color.white);
    }

    private IEnumerator ScaleCoinText(Vector3 targetScale, Color startColor, Color endColor)
    {
        float progress = 0;
        while ((targetScale == Vector3.one && coinOb.transform.localScale.x > 1) ||
               (targetScale == Vector3.one * 2 && coinOb.transform.localScale.x < 2))
        {
            progress += Time.deltaTime * 3;
            textCoin.GetComponent<Text>().color = Color.Lerp(startColor, endColor, progress);
            coinOb.transform.localScale = Vector3.Lerp(coinOb.transform.localScale, targetScale, Time.deltaTime * 3);
            yield return null;
        }
    }

    public void MenuFail(GameObject whichUI) // 메뉴를 주어진 시간 내로 전달 못했을 때 작동
    {
        // 메뉴 실패 처리
        if (StageManager.Instance != null)
        {
            StageManager.Instance.fail += 1;
        }

        int index = currentOrderUI.IndexOf(whichUI);
        if (index >= 0)
        {
            HandleFailedOrder(index);
        }

        if (flame.activeSelf)
        {
            flame.SetActive(false);
        }
    }

    private void HandleFailedOrder(int index)
    {
        tipCombo = 0;
        tip = 0;
        textTip.text = "";
        tipSlider.value = tipCombo;

        if (StageManager.Instance != null)
        {
            StageManager.Instance.failMoney += (int)(currentOrder[index].Price * 0.5f);
        }

        coin -= (int)(currentOrder[index].Price * 0.5f);
        textCoin.text = coin.ToString(); // 돈 얼마됐다고 업데이트

        if (alpahCoroutine == null)
        {
            alpahCoroutine = StartCoroutine(TurnAlpha(wrong));
        }

        GameObject failedOrderUI = currentOrderUI[index];
        failedOrderUI.transform.position = poolPos;
        failedOrderUI.SetActive(false);

        SetPosition(index);

        currentOrderUI.RemoveAt(index);
        currentOrder.RemoveAt(index);

        MakeOrder();
    }

    public void SetPosition(int index)
    {
        // UI 위치 조정
        float width = currentOrderUI[index].GetComponent<BoxCollider2D>().size.x;

        for (int i = index + 1; i < currentOrderUI.Count; i++)
        {
            if (!currentOrderUI[i].GetComponent<OrderUI>().goLeft)
            {
                Vector3 currentPosition = currentOrderUI[i].transform.position;
                currentPosition.x -= width * 0.92f;
                currentOrderUI[i].transform.position = currentPosition;
            }
        }
    }

    private IEnumerator LerpColor(Color start, Color end, float duration)
    {
        float progress = 0;
        float increment = Time.deltaTime / duration;

        while (progress < 1)
        {
            currentColor = Color.Lerp(start, end, progress);
            timeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = currentColor;
            progress += increment;
            yield return null;
        }
    }

    public IEnumerator TurnAlpha(GameObject panel)
    {
        // 패널의 알파값을 증가시키는 코루틴
        while (panel.GetComponent<Image>().color.a < 0.4f)
        {
            float alpha = panel.GetComponent<Image>().color.a;
            alpha += 0.05f;
            panel.GetComponent<Image>().color = new Color(panel.GetComponent<Image>().color.r, panel.GetComponent<Image>().color.g, panel.GetComponent<Image>().color.b, alpha);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(TurnAlphaZero(panel));
    }

    public IEnumerator TurnAlphaZero(GameObject panel)
    {
        // 패널의 알파값을 감소시키는 코루틴
        while (panel.GetComponent<Image>().color.a > 0)
        {
            float alpha = panel.GetComponent<Image>().color.a;
            alpha -= 0.05f;
            panel.GetComponent<Image>().color = new Color(panel.GetComponent<Image>().color.r, panel.GetComponent<Image>().color.g, panel.GetComponent<Image>().color.b, alpha);
            yield return new WaitForSeconds(0.01f);
        }
        alpahCoroutine = null; // 알파값 변화 코루틴 종료
    }
}
