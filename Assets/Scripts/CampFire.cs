using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;      // �������� ��
    public float damageRate;    // �󸶳� ���� �������� �ٰ��ΰ�

    List<IDamagable> things = new List<IDamagable>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);   // 0�ʺ��� damageRate �������� DealDamage ȣ��
    }
    void DealDamage()
    {
        // things�� �ִ� ������Ʈ�鿡 TakeDamage ȣ��
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // other�� IDamagable �������̽��� ������ �ִٸ� things�� �����ص״ٰ�,
        // �� ����� ������Ʈ���� DealDamage���� TakeDamage�޼��带 ȣ��
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���� ���� things���� remove
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }
    }
}
