using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum AIState
{
    Idle,
    Wandering,  // 자동 순찰
    Attacking
}

// 몬스터도 데미지를 받기 위해서는 IDamagable을 상속받고, 구현해야한다
public class NPC : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;  // 드랍아이템 정보

    [Header("AI")]
    private UnityEngine.AI.NavMeshAgent agent;  // NavMeshAgent를 캐싱
    public float detectDistance;    // 자동으로 위치를 찍어서 이동할 수 있는 최소거리
    private AIState aiState;    //

    [Header("Wandering")]   // 자동으로 순찰할때 필요한 값
    public float minWanderDistance; // 최소거리
    public float maxWanderDistance; // 최대거리
    public float minWanderWaitTime; // 새로운 목적지를 찍을 때 걸리는 최소시간
    public float maxWanderWaitTime; // 새로운 목적지를 찍을 때 걸리는 최대시간

    [Header("Combat")]
    public int damage;
    public float attackRate;    // 공격 사이의 term
    private float lastAttackTime;   // 마지막으로 공격한 시간
    public float attackDistance;    // 공격 가능한 거리

    private float playerDistance;   // 플레이어와 떨어진 거리(플레이어 정보를 캐싱해야한다)

    public float fieldOfView = 120f;    // 시야각 내에서만 공격상태로 바꿀 수 있다

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;    // 몬스터가 가진 mesh정보를 가져온다 -> 색깔 바꾼다(공격 받았거나 등)


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // 색깔을 바꿔준다
    }

    private void Start()
    {
        SetState(AIState.Wandering);    // 처음 상태
    }

    private void Update()
    {
        // 플레이어와의 거리 계산
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);    // Idle이 아니라면 Moving이 true

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

        animator.speed = agent.speed / walkSpeed;   // agent.speed가 빨라지면 animator.speed도 빨라진다
    }

    void PassiveUpdate()
    {
        // agent가 목표지점을 찍을거고, 그 목표지점과 남은거리가 0.1f 미만
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle); // 잠시 멈춰라

            /// 다음 목표지정을 설정해라
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }
        // 남은거리가 0.1f 이상이면서 플레이어와 거리가 탐지거리보다 가깝다면
        if (playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);    // 공격
        }
    }
    // 새로운 목표지점 설정
    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());  // 실제로 목표지점을 찍고 이동
    }
    // 실제로 목표지점을 찍고 이동
    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        // SamplePosition: 일정 영역을 지정해주면 그 안에서 최단 경로를 hit에 리턴한다, layer도 필터링이 가능
        // onUnitSphere: 반지름이 1인 구
        // NavMesh.AllAreas: 모든 영역
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance) // hit.position까지 거리가 탐지거리보다 작다면
        {
            // 최단경로를 
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break; // 무한루프 방지
        }

        return hit.position;
    }

    // 공격
    void AttackingUpdate()
    {
        // 공격 애니메이션을 재생하기 위해서는 플레이어와의 거리가 공격거리 이내, 시야각 이내에 있어야한다
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true; // 공격 애니메이션을 실행하기 위해 멈춘다
            if (Time.time - lastAttackTime > attackRate)    // 시간계산
            {
                lastAttackTime = Time.time;

                // 플레이어에게 데미지 입힌다
                CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);

                animator.speed = 1;
                animator.SetTrigger("Attack");  // 공격애니메이션 재생
            }
        }
        else
        {
            if (playerDistance < detectDistance) // 공격거리 내
            {
                agent.isStopped = false;    // 일단 추적

                NavMeshPath path = new NavMeshPath();   /// 가져온 경로를 저장할 NavMeshPath 선언
                // 갈 수 있는 곳인지 판단하여 path에 저장
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
                    // 목표지점을 플레이어로 설정
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                // 갈 수 없는 곳(강으로 들어간다든가)
                else
                {
                    // 멈추고 임의의 지점으로 이동
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else // 공격거리 외
            {
                // 추적을 멈춘다
                agent.SetDestination(transform.position);   // 목표지점을 현재 내 자리로 바꾼다
                agent.isStopped = true; // 멈추고
                SetState(AIState.Wandering);    // 상태 변경
            }
        }
    }
    // 플레이어가 시야각 내부인가?
    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;   // 방향: 목표지점-내위치
        float angle = Vector3.Angle(transform.forward, directionToPlayer);  // 각도의 절반
        return angle < fieldOfView * 0.5f;  // 시야각의 절반보다 작나?
    }

    // 몬스터도 데미지를 받는다
    /// <summary>
    /// Sword를 사용했을 때는 EquipTool에서 Gathering만 작성했었다
    /// 어떻게 하면 TakePhysicalDamage를 호출할 수 있을지 각자 작성해본다
    /// 
    /// EquipTool에서 Combat 관련 속성을 참고한다
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakePhysicalDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();  // 죽는다
        }
        // 데미지 효과(코루틴)
        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        // 죽을때 모든 드랍아이템을 내려놓는다
        for (int x = 0; x < dropOnDeath.Length; x++)
        {
            Instantiate(dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }
        Destroy(gameObject);    // 죽은 몬스터는 사라진다
    }

    /// <summary>
    /// 플레이어와 마찬가지
    /// 몬스터도 데미지 입을 때 빨간색으로 색깔 바꾸는 효과를 코루틴을 통해 구현
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageFlash()
    {
        // 미리 캐싱했던 모든renderer의 색깔을 바꾼다
        for (int x = 0; x < meshRenderers.Length; x++)
        {
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);  // 색깔 바꾸고
        }
        yield return new WaitForSeconds(0.1f);  // 0.1f 초 기다린 후 아래의 로직 시작
        for (int x = 0; x < meshRenderers.Length; x++)
        {
            meshRenderers[x].material.color = Color.white;  // 하얀 섬광이 일어나는 듯한 효과
        }

    }
}
