using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    // 1. 애니메이션과 이미지 반전을 담당할 부품 변수 추가
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 2. 캐릭터에 붙어있는 Animator와 SpriteRenderer 부품 가져오기
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 좌우 이동
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 3. 걷기 애니메이션 및 좌우 반전 처리
        if (moveInput != 0) // 움직이는 중이라면
        {
            anim.SetBool("isWalking", true); // 걷기 애니메이션 켜기

            if (moveInput > 0)
                spriteRenderer.flipX = false; // 오른쪽 보면 원본 그대로
            else if (moveInput < 0)
                spriteRenderer.flipX = true;  // 왼쪽 보면 이미지 좌우 반전
        }
        else // 멈췄다면
        {
            anim.SetBool("isWalking", false); // 걷기 애니메이션 끄기
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 4. 점프 애니메이션 상태 업데이트 (바닥에 안 닿아있으면 점프 애니메이션 켜기)
        anim.SetBool("isJumping", !isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}