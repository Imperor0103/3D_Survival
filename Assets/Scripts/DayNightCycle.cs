using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;  /// time�� ������ 0~1f(0%~100%)
    public float fullDayLength;
    public float startTime = 0.4f;  /// 0.5f�϶�: ����
    private float timeRate;
    public Vector3 noon;    // ����: Vector�� rotation �� (90, 0, 0)

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity; // ������ intensity�� ������ �ø��� ���δ�

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;    // ������ intensity�� ������ �ø��� ���δ�

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;   
    public AnimationCurve reflectionIntensityMultiplier;     

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;    // fullDayLength�� 30f�� �����ϸ� �Ϸ簡 30f��
        time = startTime;   // ���� startTime�� 0.4f��. �� ���¿��� �����Ѵ�
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        // ��⿡ ���� Ȱ��ȭ, ��Ȱ��ȭ ������ sun, moon ��� ����
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // other light ����
        // �������� �����ϴ� ���: RenderSettings
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);   // time�� �°� ������ ��(Evaluate(time))�� ���� 
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);  // time�� �°� ������ ��(Evaluate(time))�� ���� 

    }

    // ����
    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        // ���� ������� 0.5f �ε�, �̶� Sun�� rotation�� (90, 0, 0)�� �Ǿ���Ѵ�
        // 360���� 0.5�� 180�� �̹Ƿ� 0.25��ŭ �� ���� 90���� ����� ���̴�
        // noon: ������ ����(90��)
        // 90���� 0.25f�ε� ���⿡ noon���� 90�� ���ص� 90���� �ȳ��´�
        // �׷��� �������� 4f�� ���ѰŴ�

        lightSource.color = colorGradiant.Evaluate(time);   // �̹� ���س��� �ִ�,�ּҰ� ���̿��� time�� �����ϸ� �׿� �´� ������ �� ����
        lightSource.intensity = intensity;

        // �ش� ������ ����, ���� ���ֱ⸸�Ѵ�
        GameObject go = lightSource.gameObject; // ��,�� ��� ��Ȱ��ȭ, Ȱ��ȭ ������Ѵ�
        if (lightSource.intensity == 0 && go.activeInHierarchy) // ��Ⱑ ������ ������ && ���̷�Űâ�� Ȱ��ȭ
            go.SetActive(false);    
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);     
    }
}
