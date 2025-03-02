using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;  // ���簪
    public float startValue;    // ���۰�
    public float maxValue;  /// �ִ밪(rpg, �����ý����� �ִٸ� ������ ���� �þ�� �����ؾ��Ѵ�)
    public float passiveValue;  // �ð��� ���� ������ ���ϴ°�
    public Image uiBar; // �̹����� fillAmount

    // Start is called before the first frame update
    void Start()
    {
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        // ui������Ʈ
        uiBar.fillAmount = GetPercentage();

    }
    float GetPercentage()
    {
        return curValue / maxValue;
    }
    // �ܺο��� ȣ�� ������ Add
    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }
    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
