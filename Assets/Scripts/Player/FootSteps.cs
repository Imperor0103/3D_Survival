using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;   // ���ڱ� �Ҹ�(AudioClip)���� �����ϴ� �迭.
    private AudioSource audioSource;    // ������� ����ϴ� AudioSource ������Ʈ.

    private Rigidbody _rigidbody;   // �������� ������(�ӵ�, �߷� ��)�� �����ϱ� ���� ���

    public float footstepThreshold; // ���ڱ� �Ҹ��� ���� ���� �ּ� �ӵ�
    public float footstepRate;  // ���ڱ� �Ҹ��� ���� ����(�ð�)
    private float footStepTime; // ������ ���ڱ� �ð�



    // Start is called before the first frame update
    void Start()
    {
        // ������Ʈ�� ĳ���Ѵ�
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // y���� �ӵ��� 0.1f ���� -> ���� ���� �پ����� ���� ȿ���� �ְڴٴ� ��
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
        {
            /// �ӵ�(��ġ�� ��ȭ��)�� ũ�� > 
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
