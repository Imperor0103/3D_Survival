using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자원에 붙는 스크립트
public class Resource : MonoBehaviour
{
    // 자원에 필요한 것
    public ItemData itemToGive;     // 이 나무가 내놓는 아이템
    public int quantityPerHit = 1;  // 1번 때릴 때 내놓는 아이템의 수량
    public int capacity;        // 총 몇번 찍어야 하는지

    /// <summary>
    /// 이 메서드는 EquipTool의 OnHit에서 호출한다
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="hitNormal"></param>
    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        // 한번 때렸을 때 여러가지 아이템이 나오는 컨셉을 고려했다
        // quantityPerHit이 2 이상이면 그렇게 된다
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;

            capacity -= 1;  // 1번 벨 때마다 capacity 1 감소
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }
    }
}
