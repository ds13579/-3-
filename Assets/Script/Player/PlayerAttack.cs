using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public int attackDamage = 10;

    public GameObject attackEffect;

    private int facingDirection = 1;

    // 공격 이펙트가 플레이어에서 얼마나 떨어질지
    public float effectDistance = 1f;

    // 1. 애니메이션을 제어할 변수 추가
    private Animator anim;

    void Start()
    {
        // 2. 캐릭터에 붙어있는 Animator 부품 가져오기
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 플레이어 방향 확인
        if (Input.GetKey(KeyCode.A))
        {
            facingDirection = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            facingDirection = 1;
        }

        // 마우스 왼쪽 클릭으로 공격
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        // 3. 공격 버튼을 누르면 애니메이션 트리거(attack) 실행!
        if (anim != null)
        {
            anim.SetTrigger("attack");
        }

        // 공격 이펙트 위치 설정
        if (attackEffect != null)
        {
            Vector3 effectPosition = attackEffect.transform.localPosition;
            effectPosition.x = effectDistance * facingDirection;
            attackEffect.transform.localPosition = effectPosition;

            attackEffect.SetActive(true);
            Invoke(nameof(HideAttackEffect), 0.5f);
        }

        // 공격 판정
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange
        );

        foreach (Collider2D enemy in enemies)
        {
            MonsterHealth monster = enemy.GetComponent<MonsterHealth>();

            if (monster != null)
            {
                float direction = enemy.transform.position.x - transform.position.x;

                if (direction * facingDirection > 0)
                {
                    monster.TakeDamage(attackDamage);
                }
            }
        }

        Debug.Log("플레이어 공격!");
    }

    void HideAttackEffect()
    {
        if (attackEffect != null)
        {
            attackEffect.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}