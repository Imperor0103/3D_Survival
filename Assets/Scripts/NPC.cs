using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum AIState
{
    Idle,
    Wandering,  // �ڵ� ����
    Attacking
}

// ���͵� �������� �ޱ� ���ؼ��� IDamagable�� ��ӹް�, �����ؾ��Ѵ�
public class NPC : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;  // ��������� ����

    [Header("AI")]
    private UnityEngine.AI.NavMeshAgent agent;  // NavMeshAgent�� ĳ��
    public float detectDistance;    // �ڵ����� ��ġ�� �� �̵��� �� �ִ� �ּҰŸ�
    private AIState aiState;    //

    [Header("Wandering")]   // �ڵ����� �����Ҷ� �ʿ��� ��
    public float minWanderDistance; // �ּҰŸ�
    public float maxWanderDistance; // �ִ�Ÿ�
    public float minWanderWaitTime; // ���ο� �������� ���� �� �ɸ��� �ּҽð�
    public float maxWanderWaitTime; // ���ο� �������� ���� �� �ɸ��� �ִ�ð�

    [Header("Combat")]
    public int damage;
    public float attackRate;    // ���� ������ term
    private float lastAttackTime;   // ���������� ������ �ð�
    public float attackDistance;    // ���� ������ �Ÿ�

    private float playerDistance;   // �÷��̾�� ������ �Ÿ�(�÷��̾� ������ ĳ���ؾ��Ѵ�)

    public float fieldOfView = 120f;    // �þ߰� �������� ���ݻ��·� �ٲ� �� �ִ�

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;    // ���Ͱ� ���� mesh������ �����´� -> ���� �ٲ۴�(���� �޾Ұų� ��)


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // ������ �ٲ��ش�
    }

    private void Start()
    {
        SetState(AIState.Wandering);    // ó�� ����
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ� ���
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);    // Idle�� �ƴ϶�� Moving�� true

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;   // agent.speed�� �������� animator.speed�� ��������
    }

    void PassiveUpdate()
    {
        // agent�� ��ǥ������ �����Ű�, �� ��ǥ������ �����Ÿ��� 0.1f �̸�
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle); // ��� �����

            /// ���� ��ǥ������ �����ض�
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }
        // �����Ÿ��� 0.1f �̻��̸鼭 �÷��̾�� �Ÿ��� Ž���Ÿ����� �����ٸ�
        if (playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);    // ����
        }
    }
    // ���ο� ��ǥ���� ����
    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());  // ������ ��ǥ������ ��� �̵�
    }
    // ������ ��ǥ������ ��� �̵�
    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        // SamplePosition: ���� ������ �������ָ� �� �ȿ��� �ִ� ��θ� hit�� �����Ѵ�, layer�� ���͸��� ����
        // onUnitSphere: �������� 1�� ��
        // NavMesh.AllAreas: ��� ����
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance) // hit.position���� �Ÿ��� Ž���Ÿ����� �۴ٸ�
        {
            // �ִܰ�θ� 
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break; // ���ѷ��� ����
        }

        return hit.position;
    }

    // ����
    void AttackingUpdate()
    {
        // ���� �ִϸ��̼��� ����ϱ� ���ؼ��� �÷��̾���� �Ÿ��� ���ݰŸ� �̳�, �þ߰� �̳��� �־���Ѵ�
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true; // ���� �ִϸ��̼��� �����ϱ� ���� �����
            if (Time.time - lastAttackTime > attackRate)    // �ð����
            {
                lastAttackTime = Time.time;

                // �÷��̾�� ������ ������
                CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);

                animator.speed = 1;
                animator.SetTrigger("Attack");  // ���ݾִϸ��̼� ���
            }
        }
        else
        {
            if (playerDistance < detectDistance) // ���ݰŸ� ��
            {
                agent.isStopped = false;    // �ϴ� ����

                NavMeshPath path = new NavMeshPath();   /// ������ ��θ� ������ NavMeshPath ����
                // �� �� �ִ� ������ �Ǵ��Ͽ� path�� ����
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
                    // ��ǥ������ �÷��̾�� ����
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                // �� �� ���� ��(������ ���ٵ簡)
                else
                {
                    // ���߰� ������ �������� �̵�
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else // ���ݰŸ� ��
            {
                // ������ �����
                agent.SetDestination(transform.position);   // ��ǥ������ ���� �� �ڸ��� �ٲ۴�
                agent.isStopped = true; // ���߰�
                SetState(AIState.Wandering);    // ���� ����
            }
        }
    }
    // �÷��̾ �þ߰� �����ΰ�?
    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;   // ����: ��ǥ����-����ġ
        float angle = Vector3.Angle(transform.forward, directionToPlayer);  // ������ ����
        return angle < fieldOfView * 0.5f;  // �þ߰��� ���ݺ��� �۳�?
    }

    // ���͵� �������� �޴´�
    /// <summary>
    /// Sword�� ������� ���� EquipTool���� Gathering�� �ۼ��߾���
    /// ��� �ϸ� TakePhysicalDamage�� ȣ���� �� ������ ���� �ۼ��غ���
    /// 
    /// EquipTool���� Combat ���� �Ӽ��� �����Ѵ�
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakePhysicalDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();  // �״´�
        }
        // ������ ȿ��(�ڷ�ƾ)
        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        // ������ ��� ����������� �������´�
        for (int x = 0; x < dropOnDeath.Length; x++)
        {
            Instantiate(dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }
        Destroy(gameObject);    // ���� ���ʹ� �������
    }

    /// <summary>
    /// �÷��̾�� ��������
    /// ���͵� ������ ���� �� ���������� ���� �ٲٴ� ȿ���� �ڷ�ƾ�� ���� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageFlash()
    {
        // �̸� ĳ���ߴ� ���renderer�� ������ �ٲ۴�
        for (int x = 0; x < meshRenderers.Length; x++)
        {
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);  // ���� �ٲٰ�
        }
        yield return new WaitForSeconds(0.1f);  // 0.1f �� ��ٸ� �� �Ʒ��� ���� ����
        for (int x = 0; x < meshRenderers.Length; x++)
        {
            meshRenderers[x].material.color = Color.white;  // �Ͼ� ������ �Ͼ�� ���� ȿ��
        }

    }
}
