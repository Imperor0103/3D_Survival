using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // PlayerController �����Ͽ� �÷��̾��� ��ɰ� ������ Player�� ��� ��´�
    public PlayerController controller;
    public PlayerCondition condition;   // �÷��̾��� ����(ui�� ����)

    // ������ ���� ǥ��
    public ItemData itemData;   // ���� Interaction ���� ������ ����
    public Action addItem;  // ������ ��ȣ�ۿ��� �� �� ȣ���� �Լ��� ������ delegate�� �����Ѵ�

    private void Awake()
    {
        // �ܺο��� Player������ ������ �� ���� CharacterManager�� ���ؼ� �Ѵ�
        CharacterManager.Instance.Player = this;    

        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

}
