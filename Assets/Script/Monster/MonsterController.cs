using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("몬스터 설정")]
    public float moveSpeed = 2f;       // 이동 속도
    public float attackRange = 1.2f;   // 공격 사거리
    public int damage = 10;            // 공격력
    public float attackCooldown = 1.2f;// 선딜레이 및 공격 쿨타임 (1.2초)

    private Transform player;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private float attackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackTimer = attackCooldown; // 시작 시 타이머 초기화 (1.2초)

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null) return;

        // 사거리 내에 들어왔을 때
        if (GetXDistance() <= attackRange)
        {
            // 타이머 차감 (1.2초 카운트다운 시작)
            attackTimer -= Time.deltaTime;

            // 1.2초가 지나면 첫 공격 실행
            if (attackTimer <= 0f)
            {
                Attack();
            }
        }
        else
        {
            // 사거리를 벗어나면 타이머를 다시 1.2초로 리셋 (재진입 시 1.2초 대기)
            attackTimer = attackCooldown;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // 사거리보다 멀 때만 이동
        if (GetXDistance() > attackRange)
        {
            float targetX = Mathf.MoveTowards(rb.position.x, player.position.x, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(new Vector2(targetX, rb.position.y));
        }
    }

    void Attack()
    {
        if (playerHealth != null) playerHealth.TakeDamage(damage);
        attackTimer = attackCooldown; // 공격 후 다음 공격까지 1.2초 대기
        Debug.Log("몬스터가 공격!");
    }

    private float GetXDistance()
    {
        return Mathf.Abs(transform.position.x - player.position.x);
    }
}