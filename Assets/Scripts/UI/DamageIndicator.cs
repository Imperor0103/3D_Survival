using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;     // 데미지 입을때 켜지는 빨간색 이미지
    public float flashSpeed;    // 얼마나 빨리 빨간색 이미지가 켜졌다 사라지는지

    private Coroutine coroutine;    // 코루틴을 실행하기 위해 필요한 변수

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;   // delegate에 메서드 추가
    }

    public void Flash()
    {
        // 실행중인 기존의 코루틴이 있다면 종료
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;   // 코루틴 실행 전에 이미지를 활성화
        image.color = new Color(1f, 105f / 255f, 105f / 255f);
        coroutine = StartCoroutine(FadeAway()); // 코루틴 실행
    }
    // 빨간색 이미지가 켜질때마다 값이 보였다가 없어지는 처리
    // 코루틴을 활용
    // Main에 있는 작업을 일시정지, Coroutine 갔다가 다시 Main간다
    // 이를 통해 연속적인 변화값을 적용
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0.0f)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;    // 한번 켜진 다음에 빨간색 보여주고, 그다음부터는 색깔이 옅어진다
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;  // 다음 프레임에는 여기부터 시작
        }
        image.enabled = false;  // 이미지 컴포넌트 비활성화
    }
}
