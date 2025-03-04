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

    public float noHungerHealthDecay;   // hunger�� 0�� �Ǹ� ü�� ���Ұ� ����

    public event Action onTakeDamage;   // hp ���ҽ� ȭ�� �������� ���� delegate
    // DamageIndicator���� PlayerCondition�� �����Ͽ� onTakeDamage�� ���



    // hunger�� ���������� ������
    void Update()
    {
        // Time.deltaTime: ����� ���� ���̸� �����Ѵ�
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
        Debug.Log("�׾���");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke(); // delegate�� �Լ��� ������ ȣ��
    }
    // ��� �ֵθ��� ���׹̳� �پ���
    // ��� ����ϴ� �ʿ��� UseStamina�� ȣ��
    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)  // �پ�� ���¹̳��� 0���� ������, �� �ൿ�� �� �� ����
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }

}
