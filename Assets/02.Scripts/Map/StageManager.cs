using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [SerializeField]
    public int[] stages = {3,1,2,3};

    public Sprite YellowStar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
