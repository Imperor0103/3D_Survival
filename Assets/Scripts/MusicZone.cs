using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioSource audioSource;

    // fade in out ��� �߰�
    public float fadeTime;  // fade in out �ϴ� �ð�

    public float maxVolume;
    private float targetVolume;


    // Start is called before the first frame update
    void Start()
    {
        targetVolume = 0.0f;    // ó������ �Ҹ� 0f

        audioSource = GetComponent<AudioSource>();  // ĳ��
        audioSource.volume = targetVolume;
        audioSource.Play();     // �ϴ� ���
    }

    // Update is called once per frame
    void Update()
    {
        // float�� �̼� ������ �ֱ� ������ �ٻ�ġ�� �����ؾ��Ѵ�
        // Mathf.Approximately: Ư�� ���� �ȿ� ������ ���� ������ ����Ѵ�
        if (!Mathf.Approximately(audioSource.volume, targetVolume))  // �տ� ! �پ���(�ٻ簪�� ���� ���� ��)
        {
            // �Ҹ��� ���������� ũ�� �Ѵ�
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, (maxVolume / fadeTime) * Time.deltaTime);
            /// (maxVolume / fadeTime)�� �ǹ�: 1�ʴ� ���ϴ�(���� �Ǵ� ����) volume�� ��
            /// (maxVolume / fadeTime) * Time.deltaTime�� �� �����Ӵ� ������ ������ ũ��
            // ������ ������ ����
            // �������� ������ ����
        }
    }
    // ������ ���� ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in");

            targetVolume = maxVolume;
        }
    }
    // �������� ���� ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player out");

            targetVolume = 0.0f;
        }
    }
}
