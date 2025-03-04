using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);    
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;   // hunger가 0이 되면 체력 감소가 시작

    public event Action onTakeDamage;   // hp 감소시 화면 깜빡임을 받을 delegate
    // DamageIndicator에서 PlayerCondition에 접근하여 onTakeDamage에 등록



    // hunger를 지속적으로 내린다
    void Update()
    {
        // Time.deltaTime: 기기의 성능 차이를 보정한다
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        if (health.curValue == 0f)
        {
            Die();
        }
    }
    public void Heal(float amount)
    {
        health.Add(amount);
    }
    public void Eat(float amount)
    {
        health.Add(amount);
    }
    public void Die()
    {
        Debug.Log("죽었다");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke(); // delegate에 함수가 있으면 호출
    }
    // 장비 휘두르면 스테미나 줄어든다
    // 장비 사용하는 쪽에서 UseStamina를 호출
    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)  // 줄어든 스태미나가 0보다 작으면, 그 행동을 할 수 없다
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }

}
