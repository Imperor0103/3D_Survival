using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;


public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;    // 모든 슬롯정보를 가지고 있다

    public GameObject inventoryWindow;  // 인벤토리 창
    public Transform slotPanel; // 슬롯의 판넬

    public Transform dropPosition;  // Player의 dropPosition을 저장

    [Header("Selected Item")]
    public TextMeshProUGUI selectedItemName;    // 선택아이템의 이름
    public TextMeshProUGUI selectedItemDescription; // 설명
    public TextMeshProUGUI selectedItemStatName;    // 스탯
    public TextMeshProUGUI selectedItemStatValue;   // 값
    /// Button이지만, 활성화 비활성화만 해줄 것이므로 GameObject로 들고 있는 것이 편하다
    public GameObject useButton;    // 사용버튼
    public GameObject equipButton;  // 장착버튼
    public GameObject unEquipButton;    // 해제버튼
    public GameObject dropButton;   // 버리기버튼

    private int curEquipIndex;

    private PlayerController controller;    // 정보를 주고받을 플레이어의 정보(특히 delegate를 가져오기 위함이다)
    private PlayerCondition condition;  // 정보를 주고받을 플레이어의 상태


    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle; // delegate에 함수 등록
        CharacterManager.Instance.Player.addItem += AddItem;  // delegate에 함수 등록

        inventoryWindow.SetActive(false);   // 처음에는 인벤토리창 비활성화
        slots = new ItemSlot[slotPanel.childCount]; // 슬롯 초기화, childCount:자식의 개수
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;  /// 인벤토리는 UIInventory스크립트가 붙은 오브젝트 하나뿐이다
                                        /// 런타임중에 프로그램 코드상으로 연결한다(직접 연결X)
        }

        ClearSelectedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 아이템을 클릭하면 표시되는 정보 초기화
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
    // tab 키 누르면 인벤토리창 활성화/비활성화 
    // PlayerController에서 설정
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
        // activeInHierarchy: 하이러키에 활성화 되어있다면 true 리턴
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;  // 상호작용 중인 아이템 데이터를 받아온다

        // 중복 가능한 아이템인가? (canStack이 true일 때 가능하다)
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data); // 슬롯 가져온다
            // 숫자 올린다(최대 12까지)
            if (slot != null)
            {
                slot.quantity++;

                // UIUpdate
                UpdateUI();

                CharacterManager.Instance.Player.itemData = null;   // 데이터 초기화(일 끝냈으면 비워라)
                return;
            }
        }
        // 아니라면 비어있는 슬롯 가져온다
        ItemSlot emptySlot = GetEmptySlot();

        // 비어있는 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;  // 아이템 추가
            emptySlot.quantity = 1;

            // UIUpdate
            UpdateUI();

            CharacterManager.Instance.Player.itemData = null;   // 일 끝냈으면 비워라
            return;
        }
        // 없다면 파밍한 아이템을 버린다
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;   // 데이터 초기화(일 끝냈으면 비워라)
    }
    void UpdateUI()
    {
        // 모든 슬롯을 조사하여, 슬롯에 데이터가 있으면 setting
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
    // 중복할 수 있는 아이템이라면 
    ItemSlot GetItemStack(ItemData data)
    {
        // 모든 슬롯을 조사하여, data가 있는 슬롯이 있다면 리턴
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];    // 해당 슬롯을 반환
            }
        }
        return null;
    }
    // 비어있는 슬롯 가져오기
    ItemSlot GetEmptySlot()
    {
        // 모든 슬롯을 조사하여, data가 있는 슬롯이 있다면 리턴
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        // 모든 슬롯에 데이터가 있다면
        return null;
    }
    // 아이템 버리기
    void ThrowItem(ItemData data)
    {
        /// 다시 검색하지 않고, 미리 저장한 프리팹 리소스를 이용하여 인스턴스를 생성
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));    
    }
}
