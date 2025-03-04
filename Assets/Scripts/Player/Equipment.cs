using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// �������
public class Equipment : MonoBehaviour
{
    public Equip curEquip;      // ���� �����ϰ� �ִ� �������� ����
    public Transform equipParent;   // ��� �޾��� ��ġ: EquipCamera

    // 
    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        // ĳ��
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
    }

    // ����
    public void EquipNew(ItemData data)
    {   
        UnEquip();  // ������ ��� �ִ� ��� ����
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }
    // ����
    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
    // �ִϸ��̼� ���
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        // InputActionPhase.Performed: ������ �ִ� ����
        // curEquip != null: ���� �������� ���Ⱑ �ִ�
        // controller.canLock: �κ��丮â�� ��Ȱ��ȭ�� ����
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLock)
        {
            curEquip.OnAttackInput();
        }
    }
}
