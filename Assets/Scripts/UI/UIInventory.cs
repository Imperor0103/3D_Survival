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

        controller.inventory += Toggle; // delegate에 함수 등록

        inventoryWindow.SetActive(false);   // 처음에는 인벤토리창 비활성화
        slots = new ItemSlot[slotPanel.childCount]; // 슬롯 초기화, childCount:자식의 개수
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;  // 인벤토리는 UIInventory스크립트가 붙은 오브젝트 하나뿐이다
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
}
