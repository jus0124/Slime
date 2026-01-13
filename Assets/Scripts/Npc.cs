using UnityEngine;

public class Npc : MonoBehaviour
{
    public Transform player; // 플레이어 Transform을 인스펙터에서 할당
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // 플레이어가 NPC보다 오른쪽에 있으면 오른쪽 보기
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
}
