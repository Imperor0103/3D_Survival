using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;  /// time의 범위는 0~1f(0%~100%)
    public float fullDayLength;
    public float startTime = 0.4f;  /// 0.5f일때: 정오
    private float timeRate;
    public Vector3 noon;    // 정오: Vector의 rotation 값 (90, 0, 0)

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity; // 조명의 intensity을 서서히 늘리고 줄인다

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;    // 조명의 intensity을 서서히 늘리고 줄인다

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;   
    public AnimationCurve reflectionIntensityMultiplier;     

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;    // fullDayLength를 30f로 설정하면 하루가 30f초
        time = startTime;   // 현재 startTime는 0.4f다. 이 상태에서 시작한다
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        // 밝기에 따른 활성화, 비활성화 조절은 sun, moon 모두 적용
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // other light 접근
        // 설정값에 접근하는 방법: RenderSettings
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);   // time에 맞게 보간된 값(Evaluate(time))을 대입 
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);  // time에 맞게 보간된 값(Evaluate(time))을 대입 

    }

    // 조명
    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        // 만약 정오라면 0.5f 인데, 이때 Sun의 rotation은 (90, 0, 0)이 되어야한다
        // 360도의 0.5는 180도 이므로 0.25만큼 더 빼서 90도를 만드는 것이다
        // noon: 정오의 각도(90도)
        // 90도는 0.25f인데 여기에 noon값인 90을 곱해도 90도가 안나온다
        // 그래서 마지막에 4f를 곱한거다

        lightSource.color = colorGradiant.Evaluate(time);   // 이미 정해놓은 최대,최소값 사이에서 time을 대입하면 그에 맞는 일정한 값 추출
        lightSource.intensity = intensity;

        // 해는 실제로 돌고, 달은 떠있기만한다
        GameObject go = lightSource.gameObject; // 해,달 모두 비활성화, 활성화 해줘야한다
        if (lightSource.intensity == 0 && go.activeInHierarchy) // 밝기가 완전히 없어짐 && 하이러키창에 활성화
            go.SetActive(false);    
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);     
    }
}
