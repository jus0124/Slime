using UnityEngine;

public class WallAndGround : MonoBehaviour
{
    public Transform groundCheck;
    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isTouchingWall;

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        bool isRightWall = Physics2D.OverlapCircle(wallCheckRight.position, checkRadius, wallLayer);
        bool isLeftWall = Physics2D.OverlapCircle(wallCheckLeft.position, checkRadius, wallLayer);
        isTouchingWall = isRightWall || isLeftWall;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }

        if (wallCheckRight != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheckRight.position, checkRadius);
        }

        if (wallCheckLeft != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheckLeft.position, checkRadius);
        }
    }
}
