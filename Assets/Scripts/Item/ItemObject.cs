using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();  // ȭ�鿡 ��� prompt ���� �Լ�
    public void OnInteract();   // � ȿ���� �߻���ų���ΰ�
}


public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        // prompt�� ��� ����
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    // EŰ�� ������ ȣ��Ǵ� �޼���
    public void OnInteract()
    {
        //Player ��ũ��Ʈ ���� ����
        CharacterManager.Instance.Player.itemData = data;   // �÷��̾��� itemData�� ����
        CharacterManager.Instance.Player.addItem?.Invoke(); // addItem�� �����Ǿ��ִ� �Լ��� ������ ����
        Destroy(gameObject);    // �κ��丮�� �̵��� �������� ������ ����
    }
}
