using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 12f;
    public float wallSlideSpeed = 2f;
    public float wallJumpDuration = 0.2f;

    public Transform groundCheck;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallJumping = false;
    private bool canWallJump = false;
    private bool hasWallJumped = false;
    private float wallJumpTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // 점프 입력
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                hasWallJumped = false;
            }
            else if (canWallJump && !hasWallJumped && isTouchingWall)
            {
                isWallJumping = true;
                wallJumpTimer = wallJumpDuration;
                hasWallJumped = true;
                canWallJump = false;

                float wallDir = isTouchingWallOnRight() ? -1 : 1;
                rb.linearVelocity = new Vector2(wallDir * wallJumpForceX, wallJumpForceY);
            }
        }

        // 벽 점프 타이머
        if (isWallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                isWallJumping = false;
            }
        }

        UpdateAnimator();  // ✅ 애니메이터 값 업데이트
    }

    void FixedUpdate()
    {
        // 충돌 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        bool wallLeft = Physics2D.OverlapCircle(wallCheckLeft.position, checkRadius, wallLayer);
        bool wallRight = Physics2D.OverlapCircle(wallCheckRight.position, checkRadius, wallLayer);
        isTouchingWall = wallLeft || wallRight;

        // 벽 점프 조건 설정
        if (isTouchingWall && !isGrounded && !isWallJumping && !hasWallJumped)
        {
            canWallJump = true;
        }

        if (isGrounded)
        {
            isWallJumping = false;
            hasWallJumped = false;
            canWallJump = false;
        }

        // 이동
        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        // 벽 슬라이딩
        if (isTouchingWall && !isGrounded && !isWallJumping)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, -wallSlideSpeed);
        }
    }

    // 애니메이션 파라미터 업데이트
    private void UpdateAnimator()
    {
        bool isMoving = Mathf.Abs(moveInput) > 0.01f;
        bool isJumpingUp = !isGrounded && rb.linearVelocity.y > 0.1f;

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isJumpingUp", isJumpingUp);
    }

    private bool isTouchingWallOnRight()
    {
        return Physics2D.OverlapCircle(wallCheckRight.position, checkRadius, wallLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (groundCheck != null) Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        if (wallCheckLeft != null) Gizmos.DrawWireSphere(wallCheckLeft.position, checkRadius);
        if (wallCheckRight != null) Gizmos.DrawWireSphere(wallCheckRight.position, checkRadius);
    }
}
