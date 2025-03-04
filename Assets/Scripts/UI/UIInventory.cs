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

        controller.inventory += Toggle; // delegate�� �Լ� ���

        inventoryWindow.SetActive(false);   // ó������ �κ��丮â ��Ȱ��ȭ
        slots = new ItemSlot[slotPanel.childCount]; // ���� �ʱ�ȭ, childCount:�ڽ��� ����
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;  // �κ��丮�� UIInventory��ũ��Ʈ�� ���� ������Ʈ �ϳ����̴�
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
}
