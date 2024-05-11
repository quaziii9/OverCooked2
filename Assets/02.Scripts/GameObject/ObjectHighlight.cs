using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour
{
    // 플레이어 앞에 놓인 오브젝트가 활성화가 가능한 물건인지 확인
    public bool activeObject = false;
    // 오브젝트 MeshRenderer에 교체한 Highligt Material 배열
    [SerializeField] private Material[] hightlightMaterials;

    [HideInInspector] public enum ObjectType { Counter, Craft, Return, Station, Sink, Ingredient, Board, Bin, Plate };
    public ObjectType objectType;

    //public void Activate
}
