using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Resource,   // 단순자원
    Equipable,  // 자원채취 장비
    Consumable  // 섭취가능
}

public enum ConsumableType  // 섭취가능한 아이템의 구분
{
    Hunger, // 체력회복
    Health  // 배고픔회복
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value; // 회복량
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;  // 이름
    public string description;  // 설명
    public ItemType type;   // 타입
    public Sprite icon;     // icon
    public GameObject dropPrefab;   /// 프리팹 정보(저장해두어야, 나중에 검색하지 않고 이를 이용하여 인스턴스를 생성)

    [Header("Stacking")]    // 아이템은 여러개 가질 수 있는 것도 있다
    public bool canStack;   // 여러개 가질 수 있는 아이템인가?
    public int maxStackAmount;  // 얼마나 많이 가질 수 있는가?

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;    /// 2개가 들어가있다면, 먹었을 때 체력회복, 배고픔회복 모두 가능

    // 무기 장착,해제 관련 프리팹
    [Header("Equip")]
    public GameObject equipPrefab;

}
