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

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
