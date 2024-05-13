using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [SerializeField]
    public int[] stages = {3,1,2,3};
    public bool[] unlock;
    //= new bool[StageManager.Instance.stages.Length];
    public Sprite YellowStar;

    void Start()
    {
        unlock = new bool[StageManager.Instance.stages.Length];
        Debug.Log(unlock.Length);
        unlocknode();
    }

    void unlocknode()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i] > 0)
            {
                unlock[i] = true;
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
