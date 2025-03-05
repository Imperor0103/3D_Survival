using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioSource audioSource;

    // fade in out 기능 추가
    public float fadeTime;  // fade in out 하는 시간

    public float maxVolume;
    private float targetVolume;


    // Start is called before the first frame update
    void Start()
    {
        targetVolume = 0.0f;    // 처음에는 소리 0f

        audioSource = GetComponent<AudioSource>();  // 캐싱
        audioSource.volume = targetVolume;
        audioSource.Play();     // 일단 재생
    }

    // Update is called once per frame
    void Update()
    {
        // float는 미세 오차가 있기 때문에 근사치를 적용해야한다
        // Mathf.Approximately: 특정 범위 안에 있으면 같은 값으로 취급한다
        if (!Mathf.Approximately(audioSource.volume, targetVolume))  // 앞에 ! 붙었다(근사값과 같지 않을 때)
        {
            // 소리를 점진적으로 크게 한다
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, (maxVolume / fadeTime) * Time.deltaTime);
            /// (maxVolume / fadeTime)의 의미: 1초당 변하는(증가 또는 감소) volume의 양
            /// (maxVolume / fadeTime) * Time.deltaTime은 한 프레임당 증가할 볼륨의 크기
            // 영역에 들어오면 증가
            // 영역에서 나가면 감소
        }
    }
    // 영역에 들어올 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in");

            targetVolume = maxVolume;
        }
    }
    // 영역에서 나갈 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player out");

            targetVolume = 0.0f;
        }
    }
}
