using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ���� �ϳ��ϳ��� �ٴ� ��ũ��Ʈ
public class ItemSlot : MonoBehaviour
{
    public ItemData item;   // ������ ����

    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;  // ����ǥ�� Text
    private Outline outline;             // ���ý� Outline ǥ������ ������Ʈ


    public UIInventory inventory;   // UI �κ��丮 ����

    public int index;   // ���° �����ΰ�?
    public bool equipped;   // �����ߴ°�?
    public int quantity;    // �?

    private void Awake()
    {
        outline = GetComponent<Outline>();  
    }

    // Ȱ��ȭ�� �� ȣ��ȴ�
    private void OnEnable()
    {
        outline.enabled = equipped; // �����Ѿ������� �ִ� ������ outline
    }

    private void Update()
    {
        
    }

    // UI(���� �� ĭ) ������Ʈ�� ���� �Լ�
    // AddItem���� ������ data�� ���޹�����, �ڵ� ȣ��
    // �����۵����Ϳ��� �ʿ��� ������ �� UI�� ǥ��
    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;   // 0�̸� ǥ�þ��ϰ� 1 �̻� ǥ��

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }
    // ���Կ��� �������� ���� ���(�����ų�, ����� �߰ų�) �ڵ� ȣ��
    // UI(���� �� ĭ)�� ������ ���� �� UI�� ����ִ� �Լ�
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);   // icon�� �����Ѵ�
        quatityText.text = string.Empty;
    }
    // ������ Ŭ������ �� ȣ��Ǵ� �̺�Ʈ�Լ�.
    public void OnClickButton()
    {
        // �κ��丮�� SelectItem ȣ��, ���� ������ �ε����� ����.
        inventory.SelectItem(index);
    }
}
