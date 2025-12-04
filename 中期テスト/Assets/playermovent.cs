using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class playermovent : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("射撃設定")]
    [SerializeField] private GameObject bulletPrefab;   // 弾のプレハブ（Rigidbody2D 推奨）
    [SerializeField] private Transform firePoint;       // 発射位置（未設定なら自分の位置）
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float fireCooldown = 0.15f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.right; // 直近の移動方向（射撃に利用）
    private float nextFireTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 2Dトップダウン想定
        rb.gravityScale = 0f;
        rb.freezeRotation = true; // 回転しないように（物理で回らない）
    }

    private void Update()
    {
        // 入力（WASD/矢印）
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (moveInput.sqrMagnitude > 0.0001f)
            lastMoveDir = moveInput;

        // 射撃（Space or 左クリック）
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireCooldown;
        }
    }

    private void FixedUpdate()
    {
        // MovePosition は Dynamic/Kinematic のどちらでも安定して動く
        var nextPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
    }

    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("bulletPrefab が未設定です。インスペクターで割り当ててください。");
            return;
        }

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Vector2 dir = lastMoveDir.sqrMagnitude < 0.0001f ? Vector2.right : lastMoveDir;

        // Rigidbody2D がある弾は速度で飛ばす（互換性の高い velocity を使用）
        var bRb = bullet.GetComponent<Rigidbody2D>();
        if (bRb != null)
        {
            bRb.gravityScale = 0f;
            bRb.linearVelocity = dir.normalized * bulletSpeed;
        }
        else
        {
            // 物理なしの弾は自前移動
            var mover = bullet.AddComponent<SimpleBulletMover>();
            mover.Initialize(dir.normalized * bulletSpeed);
        }
    }
}

/// <summary>
/// 物理なし弾の簡易移動
/// </summary>
public class SimpleBulletMover : MonoBehaviour
{
    private Vector2 velocity;
    [SerializeField] private float lifeTime = 3f;

    public void Initialize(Vector2 vel)
    {
        velocity = vel;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);
    }
}
