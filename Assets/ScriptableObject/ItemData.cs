using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Resource,   // �ܼ��ڿ�
    Equipable,  // �ڿ�ä�� ���
    Consumable  // ���밡��
}

public enum ConsumableType  // ���밡���� �������� ����
{
    Hunger, // ü��ȸ��
    Health  // �����ȸ��
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value; // ȸ����
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;  // �̸�
    public string description;  // ����
    public ItemType type;   // Ÿ��
    public Sprite icon;     // icon
    public GameObject dropPrefab;   // ������ ����

    [Header("Stacking")]    // �������� ������ ���� �� �ִ� �͵� �ִ�
    public bool canStack;   // ������ ���� �� �ִ� �������ΰ�?
    public int maxStackAmount;  // �󸶳� ���� ���� �� �ִ°�?

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;    /// 2���� ���ִٸ�, �Ծ��� �� ü��ȸ��, �����ȸ�� ��� ����
}
