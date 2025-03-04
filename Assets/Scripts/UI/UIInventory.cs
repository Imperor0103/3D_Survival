using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;


public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;    // ��� ���������� ������ �ִ�

    public GameObject inventoryWindow;  // �κ��丮 â
    public Transform slotPanel; // ������ �ǳ�

    public Transform dropPosition;  // Player�� dropPosition�� ����

    [Header("Selected Item")]
    public TextMeshProUGUI selectedItemName;    // ���þ������� �̸�
    public TextMeshProUGUI selectedItemDescription; // ����
    public TextMeshProUGUI selectedItemStatName;    // ����
    public TextMeshProUGUI selectedItemStatValue;   // ��
    /// Button������, Ȱ��ȭ ��Ȱ��ȭ�� ���� ���̹Ƿ� GameObject�� ��� �ִ� ���� ���ϴ�
    public GameObject useButton;    // ����ư
    public GameObject equipButton;  // ������ư
    public GameObject unEquipButton;    // ������ư
    public GameObject dropButton;   // �������ư

    private int curEquipIndex;

    private PlayerController controller;    // ������ �ְ���� �÷��̾��� ����(Ư�� delegate�� �������� �����̴�)
    private PlayerCondition condition;  // ������ �ְ���� �÷��̾��� ����


    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle; // delegate�� �Լ� ���
        CharacterManager.Instance.Player.addItem += AddItem;  // delegate�� �Լ� ���

        inventoryWindow.SetActive(false);   // ó������ �κ��丮â ��Ȱ��ȭ
        slots = new ItemSlot[slotPanel.childCount]; // ���� �ʱ�ȭ, childCount:�ڽ��� ����
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;  /// �κ��丮�� UIInventory��ũ��Ʈ�� ���� ������Ʈ �ϳ����̴�
                                        /// ��Ÿ���߿� ���α׷� �ڵ������ �����Ѵ�(���� ����X)
        }

        ClearSelectedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �������� Ŭ���ϸ� ǥ�õǴ� ���� �ʱ�ȭ
    void ClearSelectedItemWindow()
    {

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }
    // tab Ű ������ �κ��丮â Ȱ��ȭ/��Ȱ��ȭ 
    // PlayerController���� ����
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }
    public bool IsOpen()
    {
        // activeInHierarchy: ���̷�Ű�� Ȱ��ȭ �Ǿ��ִٸ� true ����
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;  // ��ȣ�ۿ� ���� ������ �����͸� �޾ƿ´�

        // �ߺ� ������ �������ΰ�? (canStack�� true�� �� �����ϴ�)
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data); // ���� �����´�
            // ���� �ø���(�ִ� 12����)
            if (slot != null)
            {
                slot.quantity++;

                // UIUpdate
                UpdateUI();

                CharacterManager.Instance.Player.itemData = null;   // ������ �ʱ�ȭ(�� �������� �����)
                return;
            }
        }
        // �ƴ϶�� ����ִ� ���� �����´�
        ItemSlot emptySlot = GetEmptySlot();

        // ����ִ� ������ �ִٸ�
        if (emptySlot != null)
        {
            emptySlot.item = data;  // ������ �߰�
            emptySlot.quantity = 1;

            // UIUpdate
            UpdateUI();

            CharacterManager.Instance.Player.itemData = null;   // �� �������� �����
            return;
        }
        // ���ٸ� �Ĺ��� �������� ������
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;   // ������ �ʱ�ȭ(�� �������� �����)
    }
    void UpdateUI()
    {
        // ��� ������ �����Ͽ�, ���Կ� �����Ͱ� ������ setting
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    // �ߺ��� �� �ִ� �������̶�� 
    ItemSlot GetItemStack(ItemData data)
    {
        // ��� ������ �����Ͽ�, data�� �ִ� ������ �ִٸ� ����
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];    // �ش� ������ ��ȯ
            }
        }
        return null;
    }
    // ����ִ� ���� ��������
    ItemSlot GetEmptySlot()
    {
        // ��� ������ �����Ͽ�, data�� �ִ� ������ �ִٸ� ����
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        // ��� ���Կ� �����Ͱ� �ִٸ�
        return null;
    }
    // ������ ������
    void ThrowItem(ItemData data)
    {
        /// �ٽ� �˻����� �ʰ�, �̸� ������ ������ ���ҽ��� �̿��Ͽ� �ν��Ͻ��� ����
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));    
    }
}
