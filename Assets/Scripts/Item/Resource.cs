using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڿ��� �ٴ� ��ũ��Ʈ
public class Resource : MonoBehaviour
{
    // �ڿ��� �ʿ��� ��
    public ItemData itemToGive;     // �� ������ ������ ������
    public int quantityPerHit = 1;  // 1�� ���� �� ������ �������� ����
    public int capacity;        // �� ��� ���� �ϴ���

    /// <summary>
    /// �� �޼���� EquipTool�� OnHit���� ȣ���Ѵ�
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="hitNormal"></param>
    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        // �ѹ� ������ �� �������� �������� ������ ������ ����ߴ�
        // quantityPerHit�� 2 �̻��̸� �׷��� �ȴ�
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;

            capacity -= 1;  // 1�� �� ������ capacity 1 ����
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }
    }
}
