using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;  // 총알 발사 위치
    public GameObject[] bulletPrefabs; // 총알 프리팹들 (속성별)
    public GameObject[] weaponPrefabs; // 무기 프리팹들 (1~4번 무기)

    private Transform player;
    private int currentBulletIndex = 0;
    private int currentWeaponIndex = 0;

    private GameObject currentWeaponInstance;

    private bool isFiring = false;

    void Start()
    {
        player = GameObject.Find("PlayerChar").transform;

        // 첫 번째 무기 초기화
        SetWeapon(currentWeaponIndex);
    }

    void Update()
    {
        RotateToMouse();
        FollowPlayer();
        HandleWeaponSwitch();
        HandleBulletSwitch();
        HandleFire();
    }

    void RotateToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Vector3 localScale = transform.localScale;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }
        transform.localScale = localScale;
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            transform.position = player.position + new Vector3(0, 0.3f, -0.2f);
        }
    }

    void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetWeapon(3);
    }

    void SetWeapon(int index)
    {
        // 무기 인덱스가 배열 크기보다 크지 않도록 하기
        if (index < 0 || index >= weaponPrefabs.Length) return;

        // 기존에 활성화된 무기 비활성화
        if (currentWeaponInstance != null)
            currentWeaponInstance.SetActive(false);

        // 새로운 무기 인스턴스를 생성하고 활성화
        currentWeaponInstance = Instantiate(weaponPrefabs[index], transform);
        firePoint = currentWeaponInstance.transform.Find("firehole");
        currentWeaponIndex = index;

        // 새 무기 활성화
        currentWeaponInstance.SetActive(true);
    }

    void HandleBulletSwitch()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            currentBulletIndex = (currentBulletIndex + 1) % bulletPrefabs.Length;
        }
        else if (scroll < 0f)
        {
            currentBulletIndex--;
            if (currentBulletIndex < 0)
                currentBulletIndex = bulletPrefabs.Length - 1;
        }
    }

    void HandleFire()
    {
        if (Input.GetMouseButtonDown(0) && firePoint != null && !isFiring)
        {
            isFiring = true;
            GameObject bullet = Instantiate(bulletPrefabs[currentBulletIndex], firePoint.position, firePoint.rotation);
            Destroy(bullet, 2f);  // 2초 뒤 총알 제거
            isFiring = false;
        }
    }
}
