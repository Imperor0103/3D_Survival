using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // PlayerController �����Ͽ� �÷��̾��� ��ɰ� ������ Player�� ��� ��´�
    public PlayerController controller;

    private void Awake()
    {
        // �ܺο��� Player������ ������ �� ���� CharacterManager�� ���ؼ� �Ѵ�
        CharacterManager.Instance.Player = this;    

        controller = GetComponent<PlayerController>();
    }

}
