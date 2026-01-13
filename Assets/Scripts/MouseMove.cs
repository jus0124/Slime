using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public Transform crosshair;           // 마우스 커서 (십자선)
    public Transform player;              // 플레이어 Transform
    public SpriteRenderer playerRenderer; // 플레이어 스프라이트 렌더러

    void Update()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.nearClipPlane; // 또는 10f 정도도 괜찮음

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f; // 2D에서는 Z값 고정 필요

        // 십자선 위치 이동
        if (crosshair != null)
            crosshair.position = mouseWorldPos;

        // 마우스가 플레이어 기준으로 왼쪽인가 오른쪽인가?
        if (player != null && playerRenderer != null)
        {
            if (mouseWorldPos.x < player.position.x)
                playerRenderer.flipX = true;   // 왼쪽
            else
                playerRenderer.flipX = false;  // 오른쪽
        }
    }
}