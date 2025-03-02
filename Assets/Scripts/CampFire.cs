using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;      // 데미지의 양
    public float damageRate;    // 얼마나 자주 데미지를 줄것인가

    List<IDamagable> things = new List<IDamagable>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);   // 0초부터 damageRate 간격으로 DealDamage 호출
    }
    void DealDamage()
    {
        // things에 있는 오브젝트들에 TakeDamage 호출
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // other가 IDamagable 인터페이스를 가지고 있다면 things에 보관해뒀다가,
        // 이 저장된 오브젝트들을 DealDamage에서 TakeDamage메서드를 호출
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 끝날 때는 things에서 remove
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }
    }
}
