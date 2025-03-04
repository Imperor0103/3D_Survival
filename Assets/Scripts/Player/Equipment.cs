using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 장착기능
public class Equipment : MonoBehaviour
{
    public Equip curEquip;      // 현재 장착하고 있는 아이템의 정보
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
    // 애니메이션 재생
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        // InputActionPhase.Performed: 눌려져 있는 동안
        // curEquip != null: 현재 장착중인 무기가 있다
        // controller.canLock: 인벤토리창이 비활성화된 상태
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLock)
        {
            curEquip.OnAttackInput();
        }
    }
}
