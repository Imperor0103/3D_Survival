using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();  // 화면에 띄울 prompt 관련 함수
    public void OnInteract();   // 어떤 효과를 발생시킬것인가
}


public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        // prompt에 띄울 정보
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    // E키를 누르면 호출되는 메서드
    public void OnInteract()
    {
        //Player 스크립트 먼저 수정
        CharacterManager.Instance.Player.itemData = data;   // 플레이어의 itemData에 대입
        CharacterManager.Instance.Player.addItem?.Invoke(); // addItem에 구독되어있는 함수가 있으면 실행
        Destroy(gameObject);    // 인벤토리로 이동한 아이템은 씬에서 삭제
    }
}
