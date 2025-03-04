using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격, 자원채취
public class EquipTool : Equip
{
    public float attackRate;    // 공격주기
    private bool attacking;     // 공격중인가?
    public float attackDistance;    // 최대 공격 가능한 거리

    public float useStamina;    // 1회 행동할때 스테미나 소모량


    [Header("Resource Gathering")]
    public bool doesGatherResources;    /// 옵션1.자원 채취 가능한가?(Gather가 가능한가?)

    [Header("Combat")]
    public bool doesDealDamage; /// 옵션2.데미지를 줄 수 있는가?(공격을 하는건가?
    public int damage;     // 데미지 양

    // 애니메이션 실행
    private Animator animator;

    private Camera camera;  // ray를 쏘는 카메라(메인 카메라)


    private void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }


    public override void OnAttackInput()
    {
        // isAttacking이 false일때만 내부로직 실행
        if (!attacking)
        {
            // 스테미나가 남았을 때만 사용가능
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");  // 애니메이션 재생
                Invoke("OnCanAttack", attackRate);      /// 시간차(공격주기마다 한번 호출)
            }
        }
    }
    void OnCanAttack()
    {
        attacking = false;
    }

    // 이벤트 함수
    // aim 기준으로 때렸을 때 EquipTool마다 attackDistance 이내에 있으면 호출
    /// <summary>
    /// 자원 채취 애니메이션에서 공격하는 그 key frame에서 OnHit을 호출해야한다
    /// </summary>
    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            /// doesGatherResources: 자원 채취 가능해야한다(Gather가 가능)
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                /// 만약 Resource 컴포넌트를 가지고 있다면 resource의 Gather를 호출한다
                resource.Gather(hit.point, hit.normal);
            }
        }
    }
}
