using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격, 자원채취
public class EquipTool : Equip
{
    public float attackRate;    // 공격주기
    private bool attacking;     // 공격중인가?
    public float attackDistance;    // 최대 공격 가능한 거리


    [Header("Resource Gathering")]
    public bool doesGatherResources;    // 자원 채취 가능한가?

    [Header("Combat")]
    public bool doesDealDamage; // 데미지를 줄 수 있는가?
    public int damage;     // 데미지 양

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
