using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 슬롯 하나하나에 붙는 스크립트
public class ItemSlot : MonoBehaviour
{
    public ItemData item;   // 아이템 정보

    public UIInventory inventory;   // UI 인벤토리 정보

    public int index;   // 몇번째 슬롯인가?
    public bool equipped;   // 장착했는가?
    public int quantity;    // 몇개?

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
}
