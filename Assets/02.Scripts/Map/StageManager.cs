using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [SerializeField]
    public int[] stages = {3,1,2,3};
    public bool[] unlock;
    public int starSum = 0;
    public TextMeshProUGUI Stars;
    

    void Start()
    {
        unlock = new bool[StageManager.Instance.stages.Length];
        Debug.Log(unlock.Length);
        Unlocknode();
        Sum();
    }

    void Unlocknode()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i] > 0)
            {
                unlock[i] = true;
            }

        }
    }
    void Sum()
    {
        foreach (int Star in stages)
        {
            starSum += Star;
        }
        Stars.text = "" + starSum;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
