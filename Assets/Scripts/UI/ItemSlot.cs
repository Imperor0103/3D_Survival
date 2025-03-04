using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 슬롯 하나하나에 붙는 스크립트
public class ItemSlot : MonoBehaviour
{
    public ItemData item;   // 아이템 정보

    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;  // 수량표시 Text
    private Outline outline;             // 선택시 Outline 표시위한 컴포넌트


    public UIInventory inventory;   // UI 인벤토리 정보

    public int index;   // 몇번째 슬롯인가?
    public bool equipped;   // 장착했는가?
    public int quantity;    // 몇개?

    private void Awake()
    {
        outline = GetComponent<Outline>();  
    }

    // 활성화될 때 호출된다
    private void OnEnable()
    {
        outline.enabled = equipped; // 장착한아이템이 있는 슬롯은 outline
    }

    private void Update()
    {
        
    }

    // UI(슬롯 한 칸) 업데이트를 위한 함수
    // AddItem으로 아이템 data를 전달받으면, 자동 호출
    // 아이템데이터에서 필요한 정보를 각 UI에 표시
    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;   // 0이면 표시안하고 1 이상만 표시

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }
    // 슬롯에서 아이템을 비우는 경우(버리거나, 사용을 했거나) 자동 호출
    // UI(슬롯 한 칸)에 정보가 없을 때 UI를 비워주는 함수
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);   // icon은 꺼야한다
        quatityText.text = string.Empty;
    }
    // 슬롯을 클릭했을 때 호출되는 이벤트함수.
    public void OnClickButton()
    {
        // 인벤토리의 SelectItem 호출, 현재 슬롯의 인덱스만 전달.
        inventory.SelectItem(index);
    }
}
