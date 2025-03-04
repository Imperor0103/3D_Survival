using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����, �ڿ�ä��
public class EquipTool : Equip
{
    public float attackRate;    // �����ֱ�
    private bool attacking;     // �������ΰ�?
    public float attackDistance;    // �ִ� ���� ������ �Ÿ�

    public float useStamina;    // 1ȸ �ൿ�Ҷ� ���׹̳� �Ҹ�


    [Header("Resource Gathering")]
    public bool doesGatherResources;    /// �ɼ�1.�ڿ� ä�� �����Ѱ�?(Gather�� �����Ѱ�?)

    [Header("Combat")]
    public bool doesDealDamage; /// �ɼ�2.�������� �� �� �ִ°�?(������ �ϴ°ǰ�?
    public int damage;     // ������ ��

    // �ִϸ��̼� ����
    private Animator animator;

    private Camera camera;  // ray�� ��� ī�޶�(���� ī�޶�)


    private void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }


    public override void OnAttackInput()
    {
        // isAttacking�� false�϶��� ���η��� ����
        if (!attacking)
        {
            // ���׹̳��� ������ ���� ��밡��
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");  // �ִϸ��̼� ���
                Invoke("OnCanAttack", attackRate);      /// �ð���(�����ֱ⸶�� �ѹ� ȣ��)
            }
        }
    }
    void OnCanAttack()
    {
        attacking = false;
    }

    // �̺�Ʈ �Լ�
    // aim �������� ������ �� EquipTool���� attackDistance �̳��� ������ ȣ��
    /// <summary>
    /// �ڿ� ä�� �ִϸ��̼ǿ��� �����ϴ� �� key frame���� OnHit�� ȣ���ؾ��Ѵ�
    /// </summary>
    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            /// doesGatherResources: �ڿ� ä�� �����ؾ��Ѵ�(Gather�� ����)
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                /// ���� Resource ������Ʈ�� ������ �ִٸ� resource�� Gather�� ȣ���Ѵ�
                resource.Gather(hit.point, hit.normal);
            }
        }
    }
}
