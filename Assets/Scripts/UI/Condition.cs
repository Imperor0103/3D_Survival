using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;  // 현재값
    public float startValue;    // 시작값
    public float maxValue;  /// 최대값(rpg, 레벨시스템이 있다면 레벨에 따라 늘어나게 수정해야한다)
    public float passiveValue;  // 시간에 따라 꾸준히 변하는값
    public Image uiBar; // 이미지의 fillAmount

    // Start is called before the first frame update
    void Start()
    {
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        // ui업데이트
        uiBar.fillAmount = GetPercentage();

    }
    float GetPercentage()
    {
        return curValue / maxValue;
    }
    // 외부에서 호출 가능한 Add
    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }
    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
