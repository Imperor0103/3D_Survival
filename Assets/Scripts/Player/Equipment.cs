using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 장착기능
public class Equipment : MonoBehaviour
{
    public Equip curEquip;      // 장착하고 있는 정보
    public Transform equipParent;   // 장비를 달아줄 위치: EquipCamera

    // 
    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        // 캐싱
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
    }

    // 장착
    public void EquipNew(ItemData data)
    {   
        UnEquip();  // 기존에 들고 있는 장비 해제
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }
    // 해제
    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
}
