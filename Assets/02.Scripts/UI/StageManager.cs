using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] public enum State { start, stage1, stage2, stage3 };   // 게임 상태 열거형
    public State playStage = State.stage1;                                  // 현재 게임 상태 초기값 설정

    private bool isStop = false; // 일시정지 여부

    [SerializeField] public bool isClearMap1 = false;   // 맵1 클리어 여부
    [SerializeField] public bool isClearMap2 = false;   // 맵2 클리어 여부
    [SerializeField] public bool isClearMap3 = false;   // 맵3 클리어 여부
    [SerializeField] public bool isAllClear = false;    // 모든 맵 클리어 여부

    [SerializeField] public int map1Star = 0; // 맵1 별 개수
    [SerializeField] public int map2Star = 0; // 맵2 별 개수
    [SerializeField] public int map3Star = 0; // 맵3 별 개수

    [SerializeField] public bool stage1Space = false; // 스테이지1 공간 여부
    [SerializeField] public bool stage2Space = false; // 스테이지2 공간 여부
    [SerializeField] public bool stage3Space = false; // 스테이지3 공간 여부

    [SerializeField] public int tipMoney = 0;       // 팁 금액
    [SerializeField] public int success = 0;        // 성공 횟수
    [SerializeField] public int fail = 0;           // 실패 횟수
    [SerializeField] public int totalMoney = 0;     // 총 금액
    [SerializeField] public int failMoney = 0;      // 실패 금액
    [SerializeField] public int successMoney = 0;   // 성공 금액

    private const int MAX_STARS = 3;

    public void SetMap(GameObject[] flags, Material[] flagMaterials)
    {
        SetStage(flags[0], flagMaterials, stage1Space, map1Star, "stage1");
        SetStage(flags[1], flagMaterials, stage2Space, map2Star, "stage2");
        SetStage(flags[2], flagMaterials, stage3Space, map3Star, "stage3");
    }

    private void SetStage(GameObject flag, Material[] flagMaterials, bool stageSpace, int starCount, string tag)
    {
        if (!stageSpace) return;

        GameObject[] stageObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in stageObjects)
        {
            obj.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        }

        Material[] materials = flag.GetComponent<MeshRenderer>().materials;
        int starIndex = Mathf.Clamp(starCount, 0, MAX_STARS);
        materials[0].mainTexture = flagMaterials[starIndex].mainTexture;
        flag.GetComponent<MeshRenderer>().materials = materials;
    }
}
