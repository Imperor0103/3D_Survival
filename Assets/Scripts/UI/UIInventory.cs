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

    private PlayerController controller;    // ������ �ְ���� �÷��̾��� ����(Ư�� delegate�� �������� �����̴�)
    private PlayerCondition condition;  // ������ �ְ���� �÷��̾��� ����


    // ���õ� �������� ���� ����
    ItemData selectedItem;  
    int selectedItemIndex = 0;
    
    // ����, ����
    private int curEquipIndex;


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

    // ItemSlot ��ũ��Ʈ ���� ����
    // ������ ������ ����â�� ������Ʈ ���ִ� �Լ�
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;  // ���Կ� �������� ���ٸ� ����

        // �迭�� �����ؼ� �ش� �ε����� �ִ� �������� �����´�
        selectedItem = slots[index].item;   // selectedItem ������ ������ ���� ����
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        // text�� ������ �־���ϴµ�, ��� �����ۿ� ������ �ִ� ���� �ƴϹǷ� �ϴ� ����
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        /// consumable�� �������� ��� ������ ���(health, hunger ���ϴ°�)
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            // ������ �ִ� �����ۿ� �̾ ���δ�
            selectedItemStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);  // ������ �������� type�� consumable�϶� ����ư Ȱ��ȭ
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);   /// ������ �������� type�� Equipable�̰�, �������� �ʾҴٸ�, ������ư Ȱ��ȭ
        unEquipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);  /// ������ �������� type�� Equipable�̰�, �����ߴٸ�, ������ư Ȱ��ȭ
        dropButton.SetActive(true); // ������ ��ư�� Ȱ��ȭ
    }

    // ��ư �̺�Ʈ �Լ�: ����ϱ�
    public void OnUseButton()
    {
        // ������ type�� consumable�� ���� �����ϴ�
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }

            RemoveSelctedItem();
        }

    }
    // ��ư �̺�Ʈ �Լ�: ������
    public void OnDropButton()
    {
        ThrowItem(selectedItem);   // ���õ� ������ ������
        RemoveSelctedItem();
    }
    // �������� ���� �������� UI ������Ʈ�� �ؾ��Ѵ�
    void RemoveSelctedItem()
    {
        // UI ������Ʈ�� ���� ������ ����
        slots[selectedItemIndex].quantity--;
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;   // ���Կ����� ������ �����ض�
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        
        UpdateUI(); // UI ������Ʈ
    }

    // ��ư �̺�Ʈ �Լ�: ����
    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            // UnEquip
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;   // ����
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }    
    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }
    // ��ư �̺�Ʈ �Լ�: ����
    public void OnUpEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}
