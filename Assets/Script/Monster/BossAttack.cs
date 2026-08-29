
using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    [Header("플레이어")]
    public Transform player;

    [Header("일반 공격")]
    public float attackRange = 2f;
    public int attackDamage = 10;
    public float attackDelay = 0.5f;

    [Header("3연속 베기")]
    public int comboEvery = 3;
    public float comboDelay = 0.4f;

    [Header("내려찍기")]
    public int slamEvery = 5;
    public float slamRange = 2.5f;
    public int slamDamage = 20;
    public float slamJumpForce = 10f;
    public float slamFallSpeed = 15f;
    public float slamWaitTime = 0.5f;

    private Rigidbody2D rb;
    private bool isAttacking = false;

    private int attackCount = 0;
    private int slamCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (player == null)
            return;

        if (isAttacking)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRange)
        {
            StartCoroutine(AttackPattern());
        }
    }

    IEnumerator AttackPattern()
    {
        isAttacking = true;

        attackCount++;
        slamCount++;

        // 내려찍기
        if (slamCount >= slamEvery)
        {
            slamCount = 0;

            yield return StartCoroutine(SlamAttack());
        }
        // 3연속 베기
        else if (attackCount >= comboEvery)
        {
            attackCount = 0;

            yield return StartCoroutine(ThreeHitCombo());
        }
        // 일반 공격
        else
        {
            NormalAttack();

            yield return new WaitForSeconds(attackDelay);
        }

        isAttacking = false;
    }

    // ============================
    // 일반 공격
    // ============================

    void NormalAttack()
    {
        Collider2D[] targets =
            Physics2D.OverlapCircleAll(
                transform.position,
                attackRange
            );

        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Player"))
            {
                PlayerHealth health =
                    target.GetComponent<PlayerHealth>();

                if (health != null)
                {
                    health.TakeDamage(attackDamage);
                }
            }
        }

        Debug.Log("보스 일반 공격!");
    }

    // ============================
    // 3연속 베기
    // ============================

    IEnumerator ThreeHitCombo()
    {
        Debug.Log("보스 3연속 베기!");

        // 1타
        NormalAttack();

        yield return new WaitForSeconds(comboDelay);

        // 2타
        NormalAttack();

        yield return new WaitForSeconds(comboDelay);

        // 3타
        NormalAttack();

        yield return new WaitForSeconds(attackDelay);
    }

    // ============================
    // 내려찍기
    // ============================

    IEnumerator SlamAttack()
    {
        Debug.Log("보스 내려찍기 준비!");

        if (rb == null)
        {
            yield break;
        }

        // 플레이어의 현재 X 위치 저장
        float targetX = player.position.x;

        // 위로 점프
        rb.velocity = new Vector2(
            0f,
            slamJumpForce
        );

        // 공중에서 대기
        yield return new WaitForSeconds(slamWaitTime);

        // 플레이어가 있던 위치로 이동
        transform.position = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );

        // 아래로 빠르게 이동
        rb.velocity = new Vector2(
            0f,
            -slamFallSpeed
        );

        Debug.Log("보스 내려찍기!");

        // 내려오는 시간
        yield return new WaitForSeconds(0.5f);

        // 내려찍기 공격 판정
        Collider2D[] targets =
            Physics2D.OverlapCircleAll(
                transform.position,
                slamRange
            );

        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Player"))
            {
                PlayerHealth health =
                    target.GetComponent<PlayerHealth>();

                if (health != null)
                {
                    health.TakeDamage(slamDamage);
                }
            }
        }

        Debug.Log("내려찍기 데미지!");

        yield return new WaitForSeconds(attackDelay);
    }

    // ============================
    // 공격 범위 표시
    // ============================

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );

        Gizmos.DrawWireSphere(
            transform.position,
            slamRange
        );
    }
}

