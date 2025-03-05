using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;   // 발자국 소리(AudioClip)들을 저장하는 배열.
    private AudioSource audioSource;    // 오디오를 재생하는 AudioSource 컴포넌트.

    private Rigidbody _rigidbody;   // 물리적인 움직임(속도, 중력 등)을 감지하기 위해 사용

    public float footstepThreshold; // 발자국 소리가 나기 위한 최소 속도
    public float footstepRate;  // 발자국 소리가 나는 간격(시간)
    private float footStepTime; // 마지막 발자국 시각



    // Start is called before the first frame update
    void Start()
    {
        // 컴포넌트를 캐싱한다
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // y방향 속도가 0.1f 이하 -> 거의 땅에 붙어있을 때만 효과를 주겠다는 뜻
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
        {
            /// 속도(위치의 변화량)의 크기 > 
            if (_rigidbody.velocity.magnitude > footstepThreshold)
            {
                if (Time.time - footStepTime > footstepRate)
                {
                    footStepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
                }

            }
        }



    }
}
