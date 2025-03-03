using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // PlayerController 포함하여 플레이어의 기능과 정보를 Player에 모두 담는다
    public PlayerController controller;
    public PlayerCondition condition;   // 플레이어의 상태(ui와 연결)

    // 아이템 정보 표시
    public ItemData itemData;   // 현재 Interaction 중인 아이템 정보
    public Action addItem;  // 아이템 상호작용을 할 때 호출할 함수를 저장할 delegate를 선언한다

    private void Awake()
    {
        // 외부에서 Player정보를 가지고 올 때는 CharacterManager를 통해서 한다
        CharacterManager.Instance.Player = this;    

        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

}
