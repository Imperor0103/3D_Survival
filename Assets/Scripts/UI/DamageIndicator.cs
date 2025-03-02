using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;     // ������ ������ ������ ������ �̹���
    public float flashSpeed;    // �󸶳� ���� ������ �̹����� ������ ���������

    private Coroutine coroutine;    // �ڷ�ƾ�� �����ϱ� ���� �ʿ��� ����

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;   // delegate�� �޼��� �߰�
    }

    public void Flash()
    {
        // �������� ������ �ڷ�ƾ�� �ִٸ� ����
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;   // �ڷ�ƾ ���� ���� �̹����� Ȱ��ȭ
        image.color = new Color(1f, 105f / 255f, 105f / 255f);
        coroutine = StartCoroutine(FadeAway()); // �ڷ�ƾ ����
    }
    // ������ �̹����� ���������� ���� �����ٰ� �������� ó��
    // �ڷ�ƾ�� Ȱ��
    // Main�� �ִ� �۾��� �Ͻ�����, Coroutine ���ٰ� �ٽ� Main����
    // �̸� ���� �������� ��ȭ���� ����
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0.0f)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;    // �ѹ� ���� ������ ������ �����ְ�, �״������ʹ� ������ ��������
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;  // ���� �����ӿ��� ������� ����
        }
        image.enabled = false;  // �̹��� ������Ʈ ��Ȱ��ȭ
    }
}
