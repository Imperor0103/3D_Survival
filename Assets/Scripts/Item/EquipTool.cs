using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����, �ڿ�ä��
public class EquipTool : Equip
{
    public float attackRate;    // �����ֱ�
    private bool attacking;     // �������ΰ�?
    public float attackDistance;    // �ִ� ���� ������ �Ÿ�


    [Header("Resource Gathering")]
    public bool doesGatherResources;    // �ڿ� ä�� �����Ѱ�?

    [Header("Combat")]
    public bool doesDealDamage; // �������� �� �� �ִ°�?
    public int damage;     // ������ ��

    // �ִϸ��̼� ����
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    public override void OnAttackInput()
    {
        // isAttacking�� false�϶��� ���η��� ����
        if (!attacking)
        {
            attacking = true;
            animator.SetTrigger("Attack");  // �ִϸ��̼� ���
            Invoke("OnCanAttack", attackRate);      /// �ð���(�����ֱ⸶�� �ѹ� ȣ��)
        }
    }
    void OnCanAttack()
    {
        attacking = false;
    }
}
